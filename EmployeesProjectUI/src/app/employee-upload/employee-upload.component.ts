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
  error: string | null = null;

  constructor(private http: HttpClient) {}

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    if(file.type !== "text/csv"){
      this.error = "Please upload a valid CSV file.";
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    this.http.post<any[]>('https://localhost:7168/api/employee/upload', formData)
      .subscribe({
        next: (data) => {
          this.results = data;
          this.error = null;
        },
        error: (err) => {
          console.error('Upload failed:', err);
          this.error = 'Failed to upload file. Please try again.';
        }
      });
  }
}