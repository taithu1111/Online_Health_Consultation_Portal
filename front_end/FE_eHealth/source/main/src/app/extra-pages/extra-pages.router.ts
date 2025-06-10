import { Route } from "@angular/router";
import { ProfileComponent } from "./profile/profile.component";
// import { PricingComponent } from "./pricing/pricing.component";
// import { InvoiceComponent } from "./invoice/invoice.component";
export const EXTRA_PAGES_ROUTE: Route[] = [
    {
        path: "profile",
        component: ProfileComponent,
    },
    //   {
    //     path: "pricing",
    //     component: PricingComponent,
    //   },
    //   {
    //     path: "invoice",
    //     component: InvoiceComponent,
    //   },
];