import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficekanbanComponent } from './officekanban.component';

describe('OfficekanbanComponent', () => {
  let component: OfficekanbanComponent;
  let fixture: ComponentFixture<OfficekanbanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OfficekanbanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficekanbanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
