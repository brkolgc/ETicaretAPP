import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard extends BaseComponent implements CanActivate {

  constructor(private jwtHelper: JwtHelperService, private router: Router, private toastrService: CustomToastrService, spinner: NgxSpinnerService) {
    super(spinner);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.showSpinner(SpinnerType.BallAtom);
    const token: string = localStorage.getItem("accessToken");
    let expired: boolean;

    try {
      expired = this.jwtHelper.isTokenExpired(token);
    } catch {
      //geçerli token olmaması durumunda expired = true
      expired = true;
    }

    //token yoksa veya expired edilmişse
    if (!token || expired) {
      this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
      this.toastrService.message("Oturum açmanız gerekmektedir", "Yetkisiz Erişim", {
        messageType: ToastrMessageType.Warning,
        position: ToastrPosition.TopRight
      })
    }

    this.hideSpinner(SpinnerType.BallAtom);
    return true;
  }
}
