import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-employee-upload',
  standalone: true,
  imports: [CommonModule, HttpClientModule, MatTableModule],
  templateUrl: './employee-upload.component.html',
})
export class EmployeeUploadComponent {
  results: any[] = [];
  displayedColumns = ['empID1', 'empID2', 'projectID', 'daysWorked'];

  constructor(private http: HttpClient) {}

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    this.http.post<any[]>('https://localhost:7168/api/employee/upload', formData)
      .subscribe((data) => this.results = data);
  }
}