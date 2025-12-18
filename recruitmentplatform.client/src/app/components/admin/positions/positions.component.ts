// position-management.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AdminApiService } from '../../../services/admin-api.service';
import { PositionDto } from '../../../models/admin.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-position-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './positions.component.html',
  styleUrls: ['./positions.component.css']
})
export class PositionManagementComponent implements OnInit {
  positions: any[] = [];
  positionForm: FormGroup;
  isEditing = false;
  editingId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private adminApiService: AdminApiService
  ) {
    this.positionForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      level: ['Junior', Validators.required],
      techStack: ['', Validators.required],
      location: ['Remote', Validators.required],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.loadPositions();
  }

  loadPositions(): void {
    this.adminApiService.getAllPositions().subscribe({
      next: (positions) => {
        this.positions = positions;
      },
      error: () => {
        Swal.fire('Error', 'Failed to load positions', 'error');
      }
    });
  }

  createPosition(): void {
    if (this.positionForm.invalid) {
      Swal.fire('Warning', 'Please fill all required fields', 'warning');
      return;
    }

    const formValue = this.positionForm.value;
    const positionData: PositionDto = {
      ...formValue,
      level: this.convertLevelToNumber(formValue.level)
    };

    if (this.isEditing && this.editingId) {
      positionData.id = this.editingId;
      this.adminApiService.updatePosition(this.editingId, positionData).subscribe({
        next: () => {
          Swal.fire('Success', 'Position updated successfully', 'success');
          this.loadPositions();
          this.resetForm();
        },
        error: () => {
          Swal.fire('Error', 'Failed to update position', 'error');
        }
      });
    } else {
      this.adminApiService.createPosition(positionData).subscribe({
        next: () => {
          Swal.fire('Success', 'Position created successfully', 'success');
          this.loadPositions();
          this.resetForm();
        },
        error: () => {
          Swal.fire('Error', 'Failed to create position', 'error');
        }
      });
    }
  }

  editPosition(position: any): void {
    this.isEditing = true;
    this.editingId = position.id;
    this.positionForm.patchValue({
      title: position.title,
      description: position.description,
      level: this.convertNumberToLevel(position.level),
      techStack: position.techStack,
      location: position.location,
      isActive: position.isActive
    });
  }

  resetForm(): void {
    this.isEditing = false;
    this.editingId = null;
    this.positionForm.reset({ level: 'Junior', location: 'Remote', isActive: true });
  }

  private convertNumberToLevel(level: number): string {
    switch (level) {
      case 1: return 'Junior';
      case 2: return 'MidLevel';
      case 3: return 'Senior';
      case 4: return 'Lead';
      default: return 'Junior';
    }
  }

  private convertLevelToNumber(level: string): number {
    switch (level) {
      case 'Junior': return 1;
      case 'MidLevel': return 2;
      case 'Senior': return 3;
      case 'Lead': return 4;
      default: return 0;
    }
  }

  deletePosition(id: string): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You won\'t be able to revert this!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.adminApiService.deletePosition(id).subscribe({
          next: () => {
            Swal.fire('Deleted!', 'Position has been deleted.', 'success');
            this.loadPositions();
          },
          error: () => {
            Swal.fire('Error', 'Failed to delete position', 'error');
          }
        });
      }
    });
  }
}
