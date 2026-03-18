import { Component, Inject, OnInit } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleService } from 'src/app/services/common/models/role.service';
import { List_Role, Roles } from 'src/app/contracts/role/List_Role';
import { MatSelectionList } from '@angular/material/list';
import { AuthorizationEndpointService } from 'src/app/services/common/models/authorization-endpoint.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';

@Component({
  selector: 'app-authorize-menu-dialog',
  templateUrl: './authorize-menu-dialog.component.html',
  styleUrls: ['./authorize-menu-dialog.component.scss']
})
export class AuthorizeMenuDialogComponent extends BaseDialog<AuthorizeMenuDialogComponent> implements OnInit {

  constructor(dialogRef: MatDialogRef<AuthorizeMenuDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AuthorizeMenuState | any
    , private roleService: RoleService, private authorizationEndpointsService: AuthorizationEndpointService,
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

    const response = await this.authorizationEndpointsService.getRolesToEndpoint(this.data.code, this.data.menuName);
    this.assignedRoles = response.roles;
  }

  assignRoles(rolesComponent: MatSelectionList) {
    const roles: string[] = rolesComponent.selectedOptions.selected.map(o => o._text.nativeElement.innerText);
    this.spinner.show(SpinnerType.BallAtom);
    this.authorizationEndpointsService.assignRoleEndpoint(roles, this.data.code, this.data.menuName,
      () => {
        this.spinner.hide(SpinnerType.BallAtom);
      }, error => {

      });
  };

  isSelected(name: string): boolean {
    return this.assignedRoles?.some(r => r.toLowerCase() === name.toLowerCase());
  }

}

export enum AuthorizeMenuState {
  Yes, No
}