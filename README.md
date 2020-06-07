# EasyDeviceDiscoveryProtocolForUnity(EDDP for Unity)
UnityでUDPブロードキャストを使い、同一LAN内の対応アプリを探索するサンプルです。  
探索される側も同じ機能を搭載したアプリケーションを動作させている必要があります。  
mDNSなどの劣化版と考えてください。  
  
違いは、アプリケーション固有の情報を載せられることです。  
UDPパケットにjsonを載せているため、RequestJson.csを拡張することで様々な情報を載せることができます。  
  
+ Windows
+ Mac
+ iOS

で動作を確認しています。  
(多分Androidでも動くと思われるがまだ動作未確認)
  
<img src="https://github.com/gpsnmeajp/EasyDeviceDiscoveryProtocolForUnity/blob/master/img/image.png?raw=true"></img>

# [仕様](doc/doc.md)
簡単な使い方です

# [スクリプト本体](Assets/EasyDeviceDiscoveryProtocol/)
スクリプト本体は [Assets/EasyDeviceDiscoveryProtocol/](Assets/EasyDeviceDiscoveryProtocol/) にあります。
