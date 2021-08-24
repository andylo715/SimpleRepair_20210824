using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using SimpleRepair_API._Repositories.Interfaces;
using SimpleRepair_API._Repositories.Repositories;
using SimpleRepair_API._Services.Interfaces;
using SimpleRepair_API._Services.Services;
using SimpleRepair_API.Data;
using SimpleRepair_API.Helpers.AutoMapper;

namespace SimpleRepair_API
{
  public class Startup
  {
    private string factoryConnection; // 廠區
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      factoryConnection = configuration.GetSection("AppSettings:Factory").Value + "Connection"; // 選擇連線字串
    }
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      // 4/14 CORS
      //   services.AddCors(options =>
      // {
      //     options.AddDefaultPolicy(builder => 
      //         builder.SetIsOriginAllowed(_ => true)
      //         .AllowAnyMethod()
      //         .AllowAnyHeader()
      //         .AllowCredentials());
      // });
      // services.AddCors(options =>
      //  {
      //    // CorsPolicy 是自訂的 Policy 名稱
      //    options.AddPolicy("CorsPolicy", policy =>
      //      {
      //        policy.AllowAnyOrigin().SetPreflightMaxAge(new TimeSpan(86400))
      //                 .AllowAnyHeader() //這邊是自己要用的，先允許全部了
      //                                   //.WithOrigins("http://localhost:3000") 可以指定哪個前端網頁才能呼叫
      //                 .AllowAnyMethod();
      //                 // .AllowCredentials();
      //      });
      //  });

      services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString(factoryConnection)));
      services.AddDbContext<SHCDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SHCConnection")));
      services.AddDbContext<CBDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CBConnection")));
      services.AddDbContext<SPCDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SPCConnection")));
      services.AddDbContext<TSHDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TSHConnection")));
      services.AddControllers().AddNewtonsoftJson();
      // Auto Mapper
      services.AddAutoMapper(typeof(Startup));
      services.AddScoped<IMapper>(sp =>
      {
        return new Mapper(AutoMapperConfig.RegisterMappings());
      });
      services.AddSingleton(AutoMapperConfig.RegisterMappings());
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                      .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                  };
                });
      // Repository
      services.AddScoped<IOfficeKanbanRepository, OfficeKanbanRepository>();
      services.AddScoped<ITOrgRepository, TOrgRepository>();
      services.AddScoped<ILineStationRepository, LineStationRepository>();

      // Services
      services.AddScoped<IOfficeKanbanService, OfficeKanbanService>();
      services.AddScoped<ITOrgService, TOrgService>();
      services.AddScoped<ILineStationService, LineStationService>();

      // swagger
      services.AddSwaggerGen(c =>
            {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleRepair_API", Version = "v1" });
              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
              });
              c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        new string[] { }
                        }
                    });
            });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        // app.UseCors("CorsPolicy");
      }
      else
      {
        app.UseHsts();
        // app.UseCors("CorsPolicy");
      }
      app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseStaticFiles();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleRepair_API v1"));
    }
  }
}
