using Microsoft.EntityFrameworkCore;
using SimpleRepair_API.Models;

namespace SimpleRepair_API.Data
{
  public class TSHDataContext : DbContext
  {
    public TSHDataContext(DbContextOptions<TSHDataContext> options) : base(options) { }
    public DbSet<ITOfficeKanban> ITOfficeKanban { get; set; }
    public DbSet<TOrg> TOrg { get; set; }
    public DbSet<LineStation> LineStation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ITOfficeKanban>().HasKey(x => x.code);
      modelBuilder.Entity<TOrg>().HasKey(x => new { x.Factory_ID, x.PDC_ID, x.Line_ID, x.Dept_ID });
      modelBuilder.Entity<LineStation>().HasKey(x => new { x.Line_ID, x.Station_ID });
    }
  }
}