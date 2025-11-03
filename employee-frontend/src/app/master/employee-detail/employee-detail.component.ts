import { Component, OnInit } from '@angular/core';
import { Employee } from '../../model/employee';
import { EmployeeService } from '../../service/employee.service';
import { NotificationService } from '../../service/notification.service';
import { EmployeeFormComponent } from '../employee-form/employee-form.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [
    CommonModule,
    EmployeeFormComponent
  ],
  templateUrl: './employee-detail.component.html',
  styleUrl: './employee-detail.component.css'
})
export class EmployeeDetailComponent implements OnInit {
  employees: Employee[] = [];
  selectedEmployee: Employee = { id: 0, name: '', email: '', position: '', salary: 0, hireDate: '' };
  showModal = false;
  isEditing = false;

  constructor(
    private employeeService: EmployeeService,
    private notificationService: NotificationService
  ){}

  async ngOnInit(): Promise<void> {
    await this.loadEmployees();
  }

  // Load all employees from backend
  async loadEmployees(): Promise<void> {
    try {
      const response = await this.employeeService.getAllEmployeesAsync();
      
      if (response.code >= 200 && response.code < 300) {
        // Handle both array tuple (old format) and object (new format) response
        let employees: Employee[] = [];
        let count: number = 0;
        let timestamp: string = '';
        
        if (Array.isArray(response.data)) {
          // Legacy: If data is an array (tuple serialized as array)
          [employees, count, timestamp] = response.data;
        } else if (response.data && typeof response.data === 'object') {
          const data = response.data as any;
          // New format: EmployeeListResponse with Employees, Count, Timestamp
          if (data.Employees || data.employees) {
            employees = data.Employees || data.employees || [];
            count = data.Count || data.count || employees.length;
            timestamp = data.Timestamp || data.timestamp || '';
          } else {
            // Fallback: Check for tuple properties (Item1, Item2, Item3)
            employees = data.Item1 || data.item1 || [];
            count = data.Item2 || data.item2 || 0;
            timestamp = data.Item3 || data.item3 || '';
          }
          
          // Convert timestamp to string if needed
          const timestampValue: any = timestamp;
          if (timestampValue instanceof Date) {
            timestamp = timestampValue.toISOString();
          } else if (typeof timestampValue === 'number') {
            timestamp = new Date(timestampValue).toISOString();
          } else if (!timestampValue || typeof timestampValue !== 'string') {
            timestamp = timestampValue ? String(timestampValue) : '';
          }
        }
        
        // Ensure employees is an array
        if (!Array.isArray(employees)) {
          employees = [];
        }
        
        this.employees = employees;
        console.log(`Success: ${response.message}, Count: ${count}, Timestamp: ${timestamp}`);
        console.log(`Loaded ${employees.length} employees:`, employees);
      } else {
        console.error(`Error: ${response.message}`);
        this.notificationService.error(response.message || 'Failed to load employees');
        this.employees = [];
      }
    } catch (err) {
      console.error('Error loading employees:', err);
      this.notificationService.error('Failed to load employees. Please try again.');
      this.employees = [];
    }
  }

  // Open modal to add new employee
  openAddModal(): void {
    this.selectedEmployee = { id: 0, name: '', email: '', position: '', salary: 0, hireDate: new Date().toISOString().split('T')[0] };
    this.isEditing = false;
    this.showModal = true;
  }

  // Open modal to edit existing employee
  openEditModal(employee: Employee): void {
    this.selectedEmployee = { ...employee };
    this.isEditing = true;
    this.showModal = true;
  }

  // Handle save from form (create or update)
  async onSaveEmployee(employee: Employee): Promise<void> {
    try {
      const response = await this.employeeService.saveEmployeeAsync(employee);
      if (response.code >= 200 && response.code < 300) {
        // Handle both array tuple and object response formats
        let savedEmployee: Employee;
        let status: string;
        let timestamp: string;
        
        if (Array.isArray(response.data)) {
          [savedEmployee, status, timestamp] = response.data;
        } else {
          savedEmployee = (response.data as any).employee || (response.data as any).data || employee;
          status = (response.data as any).status || '';
          timestamp = (response.data as any).timestamp || '';
        }
        
        console.log(`Success: ${response.message}, Status: ${status}, Timestamp: ${timestamp}`);
        
        // Show success notification based on operation type
        if (employee.id === 0) {
          this.notificationService.created('Employee created successfully!');
        } else {
          this.notificationService.updated('Employee updated successfully!');
        }
        
        this.showModal = false;
        await this.loadEmployees();
      } else {
        console.error(`Error: ${response.message}`);
        this.notificationService.error(response.message || 'Failed to save employee');
      }
    } catch (err) {
      console.error('Save failed:', err);
      this.notificationService.error('Failed to save employee. Please try again.');
    }
  }

  // Handle delete confirmation
  async onDeleteEmployee(id: number): Promise<void> {
    if (confirm('Are you sure you want to delete this employee?')) {
      try {
        const response = await this.employeeService.deleteEmployeeAsync(id);
        if (response.code >= 200 && response.code < 300) {
          // Handle both array tuple and object response formats
          let deletedId: number;
          let status: string;
          let timestamp: string;
          
          if (Array.isArray(response.data)) {
            [deletedId, status, timestamp] = response.data;
          } else {
            deletedId = (response.data as any).id || (response.data as any).deletedId || id;
            status = (response.data as any).status || '';
            timestamp = (response.data as any).timestamp || '';
          }
          
          console.log(`Success: ${response.message}, Status: ${status}, Timestamp: ${timestamp}`);
          this.notificationService.deleted('Employee deleted successfully!');
          await this.loadEmployees();
        } else {
          console.error(`Error: ${response.message}`);
          this.notificationService.error(response.message || 'Failed to delete employee');
        }
      } catch (err) {
        console.error('Delete failed:', err);
        this.notificationService.error('Failed to delete employee. Please try again.');
      }
    }
  }

  // Close modal
  onCancel(): void {
    this.showModal = false;
  }

}
