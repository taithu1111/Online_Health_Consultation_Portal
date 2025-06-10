import { Route } from '@angular/router';
import { Page404Component } from '../../authentication/page404/page404.component';
import { EmployeeSalaryComponent } from './employee-salary/employee-salary.component';
import { PayslipComponent } from './payslip/payslip.component';
export const HR_ROUTE: Route[] = [
  {
    path: 'employee-salary',
    component: EmployeeSalaryComponent,
  },
  {
    path: 'payslip',
    component: PayslipComponent,
  },
  { path: '**', component: Page404Component },
];
