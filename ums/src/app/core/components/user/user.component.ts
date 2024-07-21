import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService,ConfirmationService } from 'primeng/api';
import { User } from '../../interfaces/user';
import { UserService } from '../../services/user.service';
 
@Component({
  selector: 'app-User',
  templateUrl: './user.component.html',
  providers: [MessageService, ConfirmationService],
})
export class UserComponent implements OnInit {
  createForm!: FormGroup;
  editForm!: FormGroup;
  User: User = {} as User; 
  Users: User[] = [];
  cols!: any[];
  displayCreate: boolean = false;
  displayEdit: boolean = false;
  constructor(
    private UserService: UserService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}
  showMssgSuccess() {
    this.messageService.add({
      severity: 'success',
      summary: 'Operation Successful',
      detail: 'The operation completed successfully',
    });
  }
  showMssgError() {
    this.messageService.add({
      severity: 'error',
      summary: 'Operation Failed',
      detail: 'The operation could not be completed',
    });
  }
  confirmAction(message: string, acceptCallback: () => void) {
    this.confirmationService.confirm({
      message: message,
      accept: acceptCallback,
    });
  }
  ngOnInit(): void {
    this.cols = [
      { field: 'userName', header: 'UserName' },
      { field: 'email', header: 'Email' },
      { field: 'role', header: 'Role' },
      
    ];
    this.createForm = this.fb.group({
      id: ['0'],  
      userName: [this.User.userName, [Validators.required]],
      email: [this.User.email, [Validators.required, Validators.email]],
      password: [this.User.password, [Validators.required,Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)]], 
      role: [this.User.role],
    });
    this.editForm = this.fb.group({
      id: [''],
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]], 
    });
    this.getUsers();
  }
  getUsers(): void {
    this.UserService.getAllUsers().subscribe({
      next: (data: User[]) => {
        this.Users = data;    
      },
      error: (error) => {
        if (error.status === 404) {
          this.messageService.add({
            severity: 'info',
            summary: 'No Records Found',
            detail: 'No users have been added',
          });
        }
        else
        this.messageService.add({
          severity: 'error',
          summary: 'Server Error',
          detail: 'Unable to connect to the server',
        });
        console.error('Error retrieving Users', error);
      },
    });
  }
  openCreateDialog() {
    this.createForm.reset();
    this.createForm.patchValue({id:0, role:0});
    this.displayCreate = true;
  }
  onCreateSubmit() {
    this.createForm.markAllAsTouched();
    if (this.createForm.valid) {
      this.UserService.createUser(this.createForm.value).subscribe(
        (response) => {
          this.displayCreate = false;
          this.getUsers();
          this.showMssgSuccess();
        },
        (error) => {
          this.showMssgError();
          console.error('An error occurred while creating a user ', error);
        }
      );
    }
  }
  openEditDialog(UserData: User) {
    this.editForm.patchValue(UserData);
    this.displayEdit = true;
  }
  onEditSubmit() {
    this.editForm.markAllAsTouched();
    if (this.editForm.valid) {
      this.confirmAction(
        'Are you sure that you want to edit this User?',
        () => {
          this.UserService
            .updateUser(this.editId!.value, this.editForm.value)
            .subscribe(
              (response) => {
                this.displayEdit = false;
                this.getUsers();
                this.showMssgSuccess();
              },
              (error) => {
                this.showMssgError();
                console.error('An error occurred while updating a user ', error);
              }
            );
        }
      );
    }
  }
  onDeleteSubmit(id: number) {
    this.confirmAction(
      'Are you sure that you want to delete this User?',
      () => {
        this.UserService.deleteUser(id).subscribe(
          (response) => {
            this.getUsers();
            this.showMssgSuccess();
          },
          (error) => {
            this.showMssgError();
            console.error('An error occurred while deleting a user', error);
          }
        );
      }
    );
  }
  get createUserName() {
    return this.createForm.get('userName');
  }
  get createEmail() {
    return this.createForm.get('email');
  }
  get createPassword() {
    return this.createForm.get('password');
  }
  get editId() {
    return this.editForm.get('id');
  }
  get editUserName() {
    return this.editForm.get('userName');
  }
  get editEmail() {
    return this.editForm.get('email');
  }
  get editPassword() {
    return this.editForm.get('password');
  }
  errMssgCreateUserName() {
    return this.createUserName?.hasError('required')
      ? 'Ce champ est requis'
      : '';
  }
  errMssgCreateEmail() {
    if (this.createEmail?.hasError('required')) {
      return 'Ce champ est requis';
    } else if (this.createEmail?.hasError('email')) {
      return 'invalid email';
    }
    return '';
  }
  errMssgCreatePassword() {
    if(this.createPassword?.hasError('required')){
     return 'Ce champ est requis';
    }else if(this.createPassword?.hasError('pattern')){
      return 'Your password should contain a minimum of 8 characters, with at least one uppercase, one lowercase, one number, and one special symbol'
    }
    return '';
  }

  errMssgEditUserName() {
    return this.editUserName?.hasError('required')
      ? 'Ce champ est requis'
      : '';
  }
  errMssgEditEmail() {
    if (this.editEmail?.hasError('required')) {
      return 'Ce champ est requis';
    } else if (this.editEmail?.hasError('email')) {
      return 'invalid email';
    }
    return '';
  }
  errMssgEditPassword() {
    if(this.editPassword?.hasError('required')){
      return 'Ce champ est requis';
     }else if(this.editPassword?.hasError('pattern')){
       return 'Your password should contain a minimum of 8 characters, with at least one uppercase, one lowercase, one number, and one special symbol'
     }
     return '';
  }
}
