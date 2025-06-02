import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";
import { AllDoctorsComponent } from "./alldoctors.component";
describe("AlldoctorsComponent", () => {
  let component: AllDoctorsComponent;
  let fixture: ComponentFixture<AllDoctorsComponent>;
  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
    imports: [AllDoctorsComponent],
}).compileComponents();
    })
  );
  beforeEach(() => {
    fixture = TestBed.createComponent(AllDoctorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
