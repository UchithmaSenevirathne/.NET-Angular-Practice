import { Injectable } from '@angular/core';

import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {

  constructor(private snackBar: MatSnackBar) {}

  private showMessage(message: string, colorClass: string) {
    const config: MatSnackBarConfig = {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: [colorClass],
    };

    this.snackBar.open(message, 'Close', config);
  }

  // Reusable alert methods
  success(message: string) {
    this.showMessage(message, 'notif-success');
  }

  info(message: string) {
    this.showMessage(message, 'notif-info');
  }

  warning(message: string) {
    this.showMessage(message, 'notif-warning');
  }

  error(message: string) {
    this.showMessage(message, 'notif-error');
  }

  // Custom shortcuts for CRUD actions
  created(message: string = 'Created successfully!') {
    this.success(message);
  }

  updated(message: string = 'Updated successfully!') {
    this.info(message);
  }

  deleted(message: string = 'Deleted successfully!') {
    this.warning(message);
  }

}

