import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, catchError, throwError, firstValueFrom } from "rxjs";
import { Employee } from "../model/employee";
import { ApiResponse } from "../model/api-response";

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {
    private apiUrl = 'http://localhost:5294/api/employees'

    constructor(private http: HttpClient) { }

    //fetch all employees from backend
    getAllEmployees(): Observable<ApiResponse<[Employee[], number, string]>> {
        return this.http.get<ApiResponse<[Employee[], number, string]>>(`${this.apiUrl}`).pipe(
            catchError((error) => {
                console.error('getAllEmployees failed', error);
                return throwError(() => error);
            })
        );
    }

    async getAllEmployeesAsync(): Promise<ApiResponse<[Employee[], number, string]>> {
        try {
            return await firstValueFrom(this.getAllEmployees());
        } catch (error) {
            console.error('getAllEmployeesAsync failed', error);
            throw error;
        }
    }

    //create or update employee
    saveEmployee(employee: Employee): Observable<ApiResponse<[Employee, string, string]>> {
        const req$ = employee.id === 0
            ? this.http.post<ApiResponse<[Employee, string, string]>>(`${this.apiUrl}`, employee)
            : this.http.put<ApiResponse<[Employee, string, string]>>(`${this.apiUrl}/${employee.id}`, employee);
        return req$.pipe(
            catchError((error) => {
                console.error('saveEmployee failed', error);
                return throwError(() => error);
            })
        );
    }

    async saveEmployeeAsync(employee: Employee): Promise<ApiResponse<[Employee, string, string]>> {
        try {
            return await firstValueFrom(this.saveEmployee(employee));
        } catch (error) {
            console.error('saveEmployeeAsync failed', error);
            throw error;
        }
    }

    // delete employee by ID
    deleteEmployee(id: number): Observable<ApiResponse<[number, string, string]>> {
        return this.http.delete<ApiResponse<[number, string, string]>>(`${this.apiUrl}/${id}`).pipe(
            catchError((error) => {
                console.error('deleteEmployee failed', error);
                return throwError(() => error);
            })
        );
    }

    async deleteEmployeeAsync(id: number): Promise<ApiResponse<[number, string, string]>> {
        try {
            return await firstValueFrom(this.deleteEmployee(id));
        } catch (error) {
            console.error('deleteEmployeeAsync failed', error);
            throw error;
        }
    }
}