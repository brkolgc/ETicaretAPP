import { Component } from '@angular/core';
import { AuthService } from './services/common/auth.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
import { Router } from '@angular/router';
import { HttpClientService } from './services/common/http-client.service';
declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public authService: AuthService, private toastrService: CustomToastrService, private router: Router, private httpClientService: HttpClientService) {
    authService.identityCheck();


    //#region  basket get,post,put,delete test services
    // httpClientService.put({
    //   controller: "baskets"
    // },{basketItemId: "601b7ef7-65d9-43e6-af38-7cfc2b09b649", quantity:10}).subscribe(data => {
    //   console.log(data);
    //   debugger;
    // });

    // httpClientService.post({
    //   controller: "baskets"
    // }, { productId: "55b4f5a4-30e9-4ae9-9053-fb4834069a36", quantity: 20 }).subscribe(data => {
    //   console.log(data);
    //   debugger;
    // });

    //  httpClientService.delete({
    //   controller: "baskets"
    // }, "601b7ef7-65d9-43e6-af38-7cfc2b09b649").subscribe(data => {
    //   console.log(data);
    //   debugger;
    // });


    // httpClientService.get({
    //   controller: "baskets"
    // }).subscribe(data => {
    //   console.log(data);
    //   debugger;
    // });
    //#endregion
  }

  signOut() {
    localStorage.removeItem("accessToken");
    this.authService.identityCheck();
    this.router.navigate([""]);
    this.toastrService.message("Oturum kapatılmıştır", "Oturum Kapatıldı", {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopRight
    });
  }
}
//$.get("https://localhost:7130/api/products" , data =>{console.log(data)});

