import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";

import { TodayDeleteComponent } from "./delete.component";

describe("DeleteComponent", () => {
  let component: TodayDeleteComponent;
  let fixture: ComponentFixture<TodayDeleteComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        imports: [TodayDeleteComponent],
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(TodayDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
