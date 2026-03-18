import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { List_Role, Roles } from 'src/app/contracts/role/List_Role';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { RoleService } from 'src/app/services/common/models/role.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {

  constructor(private roleService: RoleService, spinner: NgxSpinnerService, private alertifyService: AlertifyService, private dialogService: DialogService) {
    super(spinner);
  }

  displayedColumns: string[] = ['name',  'edit', 'delete'];
  dataSource: MatTableDataSource<Roles> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  async getRoles() {
    this.showSpinner(SpinnerType.BallAtom);

    const allRoles: { totalRoleCount: number; roles: { [key: string]: string } } =
      await this.roleService.getRoles(
        this.paginator ? this.paginator.pageIndex : 0,
        this.paginator ? this.paginator.pageSize : 5,
        () => { this.hideSpinner(SpinnerType.BallAtom) },
        (errorMessage) => {
          this.hideSpinner(SpinnerType.BallAtom);

          this.alertifyService.message(errorMessage, {
            dismissOthers: true,
            messageType: MessageType.Error,
            position: Position.TopRight
          });
        });

    this.dataSource = new MatTableDataSource<Roles>(
      Object.entries(allRoles.roles).map(([key, value]) => ({
        id: key,
        name: value
      }))
    );

    // this.dataSource.paginator = this.paginator;
    this.paginator.length = allRoles.totalRoleCount;
  }

  async pageChanged() {
    await this.getRoles();
  }

  async ngOnInit() {
    await this.getRoles();
  }

}
