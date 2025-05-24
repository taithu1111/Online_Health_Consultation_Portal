import { Route } from '@angular/router';
import { MainLayoutComponent } from './layout/app-layout/main-layout/main-layout.component';
import { AuthGuard } from '@core/guard/auth.guard';
import { AuthLayoutComponent } from './layout/app-layout/auth-layout/auth-layout.component';
import { Page404Component } from './authentication/page404/page404.component';
import { Role } from '@core';

export const APP_ROUTE: Route[] = [
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: '/authentication/signin', pathMatch: 'full' },
            {
                path: 'admin',
                canActivate: [AuthGuard],
                data: {
                    role: Role.Admin,
                },
                loadChildren: () =>
                    import('./admin/admin.routes').then((m) => m.ADMIN_ROUTE),
            },
            {
                path: 'doctor',
                canActivate: [AuthGuard],
                data: {
                    role: Role.Doctor,
                },
                loadChildren: () =>
                    import('./doctor/doctor.routes').then((m) => m.DOCTOR_ROUTE),
            },
            {
                path: 'patient',
                canActivate: [AuthGuard],
                data: {
                    role: Role.Patient,
                },
                loadChildren: () =>
                    import('./patient/patient.routes').then((m) => m.PATIENT_ROUTE),
            },

            {
                path: 'calendar',
                loadChildren: () =>
                    import('./calendar/calendar.routes').then((m) => m.CALENDAR_ROUTE),
            },
            {
                path: 'task',
                loadChildren: () =>
                    import('./task/task.routes').then((m) => m.TASK_ROUTE),
            },
            {
                path: 'contacts',
                loadChildren: () =>
                    import('./contacts/contacts.routes').then((m) => m.CONTACT_ROUTE),
            },
            {
                path: 'apps',
                loadChildren: () =>
                    import('./apps/apps.routes').then((m) => m.APPS_ROUTE),
            },
            {
                path: 'maps',
                loadChildren: () =>
                    import('./maps/maps.routes').then((m) => m.MAPS_ROUTE),
            },
        ],
    },
    {
        path: 'authentication',
        component: AuthLayoutComponent,
        loadChildren: () =>
            import('./authentication/auth.routes').then((m) => m.AUTH_ROUTE),
    },
    { path: '**', component: Page404Component },
];
