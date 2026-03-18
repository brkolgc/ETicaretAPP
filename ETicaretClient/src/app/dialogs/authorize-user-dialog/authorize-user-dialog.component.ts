import { Component, Inject, OnInit } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleService } from 'src/app/services/common/models/role.service';
import { SpinnerType } from 'src/app/base/base.component';
import { MatSelectionList } from '@angular/material/list';
import { UserService } from 'src/app/services/common/models/user.service';

@Component({
  selector: 'app-authorize-user-dialog',
  templateUrl: './authorize-user-dialog.component.html',
  styleUrls: ['./authorize-user-dialog.component.scss']
})
export class AuthorizeUserDialogComponent extends BaseDialog<AuthorizeUserDialogComponent> implements OnInit {


  constructor(dialogRef: MatDialogRef<AuthorizeUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    , private roleService: RoleService, private userService: UserService,
    private spinner: NgxSpinnerService) {
    super(dialogRef);


  }
  datas: { roles: { [key: string]: string }, totalRoleCount: number };
  roles: { id: string; name: string }[] = [];
  assignedRoles: string[] = [];

  async ngOnInit() {
    this.datas = await this.roleService.getRoles(-1, -1);
    this.roles = Object.entries(this.datas.roles).map(([id, name]) => ({
      id,
      name
    }));
    this.spinner.show(SpinnerType.BallAtom)
    this.assignedRoles = await this.userService.getRolesToUser(this.data,
      () => {
        this.spinner.hide(SpinnerType.BallAtom)
      }, (error) => {
        console.log(error);
      });
  }

  assignRoles(rolesComponent: MatSelectionList) {
    const roles: string[] = rolesComponent.selectedOptions.selected.map(o => o._text.nativeElement.innerText);
    this.spinner.show(SpinnerType.BallAtom);
    this.userService.assignRoleToUser(this.data, roles,
      () => {
        debugger;
        this.spinner.hide(SpinnerType.BallAtom);
      }, error => {
        console.log(error);
      });
  };

  isSelected(name: string): boolean {
    return this.assignedRoles?.some(r => r.toLowerCase() === name.toLowerCase());
  }

}