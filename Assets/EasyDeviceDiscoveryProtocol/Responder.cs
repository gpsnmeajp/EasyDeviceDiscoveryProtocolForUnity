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
    public class Responder : MonoBehaviour
    {
        public string deivceName = "mydevice_server";
        UdpClient udpClient;
        UTF8Encoding utf8 = new UTF8Encoding(false);

        public Text text1;
        public Text text2;
        public Toggle toggle;

        void Start()
        {
            udpClient = new UdpClient(39500);
        }

        private void OnApplicationQuit()
        {
            udpClient.Close();
        }

        void Update()
        {
            if (udpClient != null && toggle.isOn)
            {
                while (udpClient.Available > 0)
                {
                    IPEndPoint point = new IPEndPoint(IPAddress.Any, 39500); //待受ポート兼応答先(変化後)
                    var r = udpClient.Receive(ref point);

                    text1.text = utf8.GetString(r);
                    text2.text = point.ToString();

                    string data = deivceName;
                    byte[] dat = utf8.GetBytes(data);
                    udpClient.Send(dat, dat.Length, point);
                }
            }

        }
    }
}