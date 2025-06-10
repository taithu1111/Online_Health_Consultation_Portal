import { ComponentFixture, TestBed, waitForAsync } from "@angular/core/testing";
import { BookAppointmentComponent } from "./bookappointment.component";
describe("BookappointmentComponent", () => {
  let component: BookAppointmentComponent;
  let fixture: ComponentFixture<BookAppointmentComponent>;
  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        imports: [BookAppointmentComponent],
      }).compileComponents();
    })
  );
  beforeEach(() => {
    fixture = TestBed.createComponent(BookAppointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
