# 仕様(予定)

## EasyDeviceDiscoveryProtocolClient.Requester
GameObjectとして配置してください。  
探索の要求、応答の受信を行います。

### void StartDiscover(Action<string> OnDeviceFound, Action<string> OnDeviceAccepted, Action<string> OnError)
事前に定義された情報載せながら、探索を開始します。  
デバイスが見つかると、OnDeviceFoundがコールされます。  
デバイスから承認されると、OnDeviceAcceptedがコールされます。  
エラーが発生すると、OnErrorがコールされます。  


## EasyDeviceDiscoveryProtocolClient.Responder
GameObjectとして配置してください。  
探索要求の受信、応答の送信を行います。  
  
探索を受けると、OnRequestedがコールされます。  

### void Accept()
デバイスへ承認を通知します。
