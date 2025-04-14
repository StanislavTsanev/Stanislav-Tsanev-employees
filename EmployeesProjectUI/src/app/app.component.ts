import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EmployeeUploadComponent } from './employee-upload/employee-upload.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, EmployeeUploadComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'EmployeesProjectUI';
}
