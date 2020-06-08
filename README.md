# EasyDeviceDiscoveryProtocolForUnity(EDDP for Unity)
UnityでUDPブロードキャストを使い、同一LAN内の対応アプリを探索するサンプルです。  
探索される側も同じ機能を搭載したアプリケーションを動作させている必要があります。  
mDNSなどの劣化版と考えてください。  
  
違いは、

+ アプリケーション固有の情報を載せられること
+ 要求側、応答側でお互いのIPアドレスが同時に取得できること

です。  
UDPパケットにjsonを載せているため、RequestJson.csを拡張することで様々な情報を載せることができます。  
  
+ Windows
+ Mac
+ iOS

で動作を確認しています。  

+ 多分Linuxでも動くと思われるがまだ動作未確認。  
+ Androidは手持ちのNexus 5Xでは動作しましたが、[機種によってはMulticast lockの取得処理が必要とのことです。](https://answers.unity.com/questions/250732/android-build-is-not-receiving-udp-broadcasts.html)
  
<img src="https://github.com/gpsnmeajp/EasyDeviceDiscoveryProtocolForUnity/blob/master/img/image.png?raw=true"></img>

# [仕様](doc/doc.md)
簡単な使い方です

# [スクリプト本体](Assets/EasyDeviceDiscoveryProtocol/)
スクリプト本体は [Assets/EasyDeviceDiscoveryProtocol/](Assets/EasyDeviceDiscoveryProtocol/) にあります。
