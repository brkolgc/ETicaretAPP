import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { DomSanitizer } from '@angular/platform-browser';
import { NgxScannerQrcodeComponent } from 'ngx-scanner-qrcode';
import { MatButton } from '@angular/material/button';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';
import { ProductService } from 'src/app/services/common/models/product.service';
import { SpinnerType } from 'src/app/base/base.component';

@Component({
  selector: 'app-qrcode-reading-dialog',
  templateUrl: './qrcode-reading-dialog.component.html',
  styleUrls: ['./qrcode-reading-dialog.component.scss'],
})
export class QrcodeReadingDialogComponent extends BaseDialog<QrcodeReadingDialogComponent> implements OnInit, OnDestroy {

  constructor(dialogRef: MatDialogRef<QrcodeReadingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,
    private spinner: NgxSpinnerService,
    private domSanitizer: DomSanitizer,
    private toastrService: CustomToastrService,
    private productService: ProductService) {

    super(dialogRef);
  }
  ngOnDestroy(): void {
    this.scanner.stop();
  }
  @ViewChild("scanner", { static: true }) scanner: NgxScannerQrcodeComponent;
  @ViewChild("txtStock", { static: true }) txtStock: ElementRef;
  @ViewChild("btnClose", { static: true }) btnClose: MatButton;

  scanned: boolean = false;

  ngOnInit(): void {
    this.scanner.start();
  }

  onEvent(e: any) {
    if (this.scanned) return;

    if (Array.isArray(e) && e.length > 0) {
      this.spinner.show(SpinnerType.BallAtom);
      const first = e[0];
      const jsonData = JSON.parse(first.value);
      if (jsonData != null && jsonData != "") {
        const stockValue = (this.txtStock.nativeElement as HTMLInputElement).value;

        this.scanned = true;
        this.btnClose._elementRef.nativeElement.click();

        this.productService.updateStockQrCodeToProduct(jsonData.Id, parseInt(stockValue), () => {
          this.spinner.hide(SpinnerType.BallAtom);
          this.toastrService.message(`${jsonData.Name} ürünün stok bilgisi ${stockValue} adet olarak güncellenmiştir.`, "Stok Başarıyla Güncellendi", {
            messageType: ToastrMessageType.Success,
            position: ToastrPosition.TopRight
          });
        });
      }
    }
  }
}