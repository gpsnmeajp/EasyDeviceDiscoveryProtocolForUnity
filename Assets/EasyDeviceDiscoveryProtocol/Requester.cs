/*
MIT License

Copyright (c) 2020 gpsnmeajp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyDeviceDiscoveryProtocolClient
{
    public class Requester : MonoBehaviour
    {
        [Header("Settings")]
        public int discoverPort = 39500; //待受ポート

        [Header("Properties")]
        public string deivceName = "mydevice_client";//自分のデバイス名
        public int servicePort = 11111;//自分が使ってほしいと思っているポート
        public string ignoreDeivceName = ""; //無視するデバイス名
        public bool desktopMode = false; //デスクトップモード(false: 非アクティブ時にポートを閉じる, true: 閉じない)

        [Header("Response Info(Read only)")]
        public string responseIpAddress = ""; //応答帰ってきたアドレス
        private int responsePort = 0; //応答帰ってきたポート
        public int responseProtocolVersion = 0; //要求のプロトコルバージョン
        public int foundDevices = 0; //見つかった台数

        [Header("Response Data(Read only)")]
        public string responseDeviceName = "";//データとして含まれるデバイス名
        public int responseServicePort = 0;//データとして含まれるポート

        [Header("Test")]
        public bool exec = false;//テスト実行

        public Action OnDeviceFound = null;

        UdpClient udpClient = null;
        UTF8Encoding utf8 = new UTF8Encoding(false); //BOMなし

        //探索開始(ボタン用)
        public void StartDiscover()
        {
            StartDiscover(() => { Debug.Log("[EDDP Requester]Found"); });
        }

        //探索開始(外部からコールされる)
        public void StartDiscover(Action OnDeviceFound)
        {
            this.OnDeviceFound = OnDeviceFound;

            //受信結果を初期化
            responseIpAddress = "";
            responsePort = 0;
            responseDeviceName = "";
            responseProtocolVersion = 0;
            foundDevices = 0;

            //jsonデータ生成
            string data = JsonUtility.ToJson(new RequestJson
            {
                servicePort = servicePort,
                deviceName = deivceName,
                version = RequestJson.protocolVersion,
            });
            byte[] dat = utf8.GetBytes(data);

            //通信を開始準備する
            TryOpen();

            //通信を開始できる状態なら
            if (udpClient != null)
            {
                udpClient.EnableBroadcast = true; //ブロードキャスト有効
                udpClient.MulticastLoopback = true; //ループバック許可
                udpClient.Send(dat, dat.Length, "255.255.255.255", discoverPort);
            }
        }

        //UDP通信の準備を行います
        void TryOpen() {
            //GameObjectが有効なときだけ開始する
            if (isActiveAndEnabled)
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                    Debug.Log("[EDDP Requester]UdpClient Open");
                }
            }
        }

        //UDP通信の停止を行います。
        void Close()
        {
            if (udpClient != null)
            {
                try
                {
                    udpClient?.Close();
                    Debug.Log("[EDDP Requester]UdpClient Closed");
                }
                finally
                {
                    udpClient = null;
                }
            }
        }

        //GameObjectがEnableになったとき開く
        private void OnEnable()
        {
            TryOpen();
        }

        //GameObjectがDisableになったとき閉じる
        private void OnDisable()
        {
            Close();
        }

        //GameObjectが破棄されるとき閉じる
        private void OnDestroy()
        {
            Close();
        }

        //アプリケーションが終了するとき閉じる
        private void OnApplicationQuit()
        {
            Close();
        }

        //アプリケーションが中断・復帰したとき(モバイルでバックグラウンドになった・エディタで別のフォーカスを当てられたとき)
        private void OnApplicationPause(bool pause)
        {
            //モバイルデバイス向けなので、デスクトップモードでは行わない
            if (!desktopMode)
            {
                if (pause)
                {
                    //アプリが閉じられたら止める
                    Close();
                }
                else
                {
                    //アプリが開かれたので開く
                    TryOpen();
                }
            }
        }

        //毎フレームループ。UDPパケットの受信処理を行う
        void Update()
        {
            if (exec) {
                exec = false;
                StartDiscover(() => { Debug.Log("[EDDP Requester]Found"); });
            }

            if (udpClient != null)
            {
                while (udpClient.Available > 0)
                {
                    IPEndPoint point = new IPEndPoint(IPAddress.Any, discoverPort);
                    var r = udpClient.Receive(ref point);
                    var res = JsonUtility.FromJson<RequestJson>(utf8.GetString(r));

                    //無視デバイス名と一致しない場合だけ処理する
                    if (res.deviceName != ignoreDeivceName) {
                        responseIpAddress = point.Address.ToString();
                        responsePort = point.Port;
                        responseProtocolVersion = res.version;

                        responseDeviceName = res.deviceName;
                        responseServicePort = res.servicePort;

                        foundDevices++;
                        OnDeviceFound?.Invoke();
                    }
                }
            }
        }
    }
}