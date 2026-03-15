import { ComponentFactoryResolver, Injectable, ViewContainerRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DynamicLoadComponentService {

  //ViewContainerRef          :dinamik olarak yüklenecek component'i içerisinde barındıran container. her dinamik yükleme sürecinde önceki view'leri clear edilmesi gerekiyor
  //ComponentFactory          :component'lerin instance'larını oluşturmak için kullanılan nesne
  //ComponentFactoryResolver  :belirli component için ComponentFactory'i resolve eden sınıf. "resolveComponentFactory" fonksiyonu aracılığıyla ilgili componente dair nesne oluşturur ve componentfactory döner

  constructor() { }

  async loadComponent(component: ComponentType, viewContainerRef: ViewContainerRef) {
    try {
      let _component: any = null;

      switch (component) {
        case ComponentType.BasketsComponent:
          _component = (await (import("../../ui/components/baskets/baskets.component"))).BasketsComponent;
          break;
      }
      viewContainerRef.clear();
      return viewContainerRef.createComponent(_component);
    } catch (err) {
      console.log(err);
      return null;
    }
  }
}

export enum ComponentType {
  BasketsComponent
}