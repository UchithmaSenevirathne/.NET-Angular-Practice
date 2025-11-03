import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Employee } from '../../model/employee';
import { FormBuilder, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.css'
})
export class EmployeeFormComponent {
  @Input() employee: Employee = { id: 0, name: '', email: '', position: '', salary: 0, hireDate: new Date().toISOString().split('T')[0] };
  @Output() save = new EventEmitter<Employee>();
  @Output() cancel = new EventEmitter<void>();

  employeeForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.employeeForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      position: ['', Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      hireDate: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.employeeForm.patchValue(this.employee);
  }

  onSave() {
    if (this.employeeForm.valid) {
      this.save.emit(this.employeeForm.value);
    }
  }

  onCancel() {
    this.cancel.emit();
  }
}
