import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() { }

  private _connection: HubConnection
  get connection(): HubConnection {
    return this._connection;
  }

  //başlatılmış hub al
  start(hubUrl: string) {
    if (!this.connection || this.connection?.state == HubConnectionState.Disconnected) {
      const builder: HubConnectionBuilder = new HubConnectionBuilder();

      const HubConnection: HubConnection = builder.withUrl(hubUrl)
        .withAutomaticReconnect()
        .build();

      HubConnection.start()
        .then(() => console.log("connected"))
        .catch(error => setTimeout(() => this.start(hubUrl), 2000));

      this._connection = HubConnection;
    }

    this.connection.onreconnected(connectionId => console.log("Reconnected"));
    this.connection.onreconnecting(error => console.log("Reconnecting"));
    this.connection.onclose(error => console.log("Close Reconnection"));
  }

  //client'tan diğer client'lara mesaj gönder
  invoke(procedureName: string, message: any, successCallBack?: (value) => void, errorCallBack?: () => void) {
    this.connection.invoke(procedureName, message)
      .then(successCallBack)
      .catch(errorCallBack);
  }

  //server'dan gelen mesajları runtime'da yakala
  on(procedureName: string, callBack: (...message: any) => void) {
    this.connection.on(procedureName, callBack);
  }
}
