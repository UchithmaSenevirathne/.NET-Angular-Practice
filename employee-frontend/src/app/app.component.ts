import { Component } from '@angular/core';
import { HeaderComponent } from './shared/header/header.component';
import { EmployeeDetailComponent } from './master/employee-detail/employee-detail.component';
import { FooterComponent } from './shared/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HeaderComponent,
    EmployeeDetailComponent,
    FooterComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'employee-frontend';
}
