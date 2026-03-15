import { Inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor(@Inject("baseSignalRUrl") private baseSignalRUrl: string) { }

  //başlatılmış hub al
  start(hubUrl: string) {
    hubUrl = this.baseSignalRUrl + hubUrl;

    const builder: HubConnectionBuilder = new HubConnectionBuilder();

    const HubConnection: HubConnection = builder.withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    HubConnection.start()
      .then(() => console.log("connected"))
      .catch(error => setTimeout(() => this.start(hubUrl), 2000));

    HubConnection.onreconnected(connectionId => console.log("Reconnected"));
    HubConnection.onreconnecting(error => console.log("Reconnecting"));
    HubConnection.onclose(error => console.log("Close Reconnection"));

    return HubConnection;
  }

  //client'tan diğer client'lara mesaj gönder
  invoke(hubUrl: string, procedureName: string, message: any, successCallBack?: (value) => void, errorCallBack?: () => void) {
    this.start(hubUrl).invoke(procedureName, message)
      .then(successCallBack)
      .catch(errorCallBack);
  }

  //server'dan gelen mesajları runtime'da yakala
  on(hubUrl: string, procedureName: string, callBack: (...message: any) => void) {
    this.start(hubUrl).on(procedureName, callBack);
  }
}
