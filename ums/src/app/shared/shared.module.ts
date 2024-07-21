import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { StyleClassModule } from 'primeng/styleclass';
import { DialogModule } from 'primeng/dialog';
import { AvatarModule } from 'primeng/avatar';
import { ToastModule } from 'primeng/toast';
import {ConfirmDialogModule} from 'primeng/confirmdialog';


@NgModule({
  exports: [
    CommonModule,
    ButtonModule,
    InputTextModule,
    StyleClassModule,
    DialogModule,
    AvatarModule,
    TableModule,
    ToastModule,
    ConfirmDialogModule
  ]
})
export class SharedModule { }
