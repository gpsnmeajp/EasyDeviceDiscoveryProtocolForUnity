using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyDeviceDiscoveryProtocolClient
{
    public class Requester : MonoBehaviour
    {
        public string deivceName = "mydevice_client";
        public Text text1;
        public Text text2;

        public Action<string> OnDeviceFound = null;
        public Action<string> OnError = null;

        UdpClient udpClient = new UdpClient();
        UTF8Encoding utf8 = new UTF8Encoding(false);

        public void TestStartDiscover()
        {
            StartDiscover((s) => {
                Debug.Log("OnDeviceFound: " + s);
            }, (s) => {
                Debug.Log("OnError: " + s);
            });
        }

        IPEndPoint point;
        public void StartDiscover(Action<string> OnDeviceFound, Action<string> OnError)
        {
            this.OnDeviceFound = OnDeviceFound;
            this.OnError = OnError;

            point = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 39500);//送信先兼待受ポート

            string data = deivceName;
            byte[] dat = utf8.GetBytes(data);

            udpClient.EnableBroadcast = true;
            text2.text = "";
            udpClient.Send(dat, dat.Length, point);
        }

        private void OnApplicationQuit()
        {
            udpClient.Close();
        }

        void Start()
        {

        }

        void Update()
        {
            if (udpClient != null)
            {
                while (udpClient.Available > 0)
                {
                    var r = udpClient.Receive(ref point);

                    text1.text = utf8.GetString(r);
                    text2.text += point.ToString() + "\n";
                }
            }
        }
    }
}