using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEditor;



public class ScreenShot : MonoBehaviour
{     
    public string clientMessage;


    // TCP stuff
    Thread sendThread;
    TcpClient client;
    TcpListener listener;
    int port; //��Ʈ��ȣ 9999

    //byte[] recevBuffer;
    // Add
    Thread receiveThread;

    public string screenShotName = "1";

    public void ScreenShotClick()
    {
        port = 10490;
        RenderTexture renderTexture = GameObject.Find("ScreenShotCamera").GetComponent<Camera>().targetTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        File.WriteAllBytes($"{Application.dataPath}/{screenShotName}.png", texture.EncodeToPNG());
        string pic_base64 = Convert.ToBase64String(texture.EncodeToPNG()); //pic : Base64
        InitTCP(pic_base64);
        // Add
        ConnectTCP();
        // ReceiveClass();
    }

    // Launch TCP to send image to python
    private void InitTCP(string pic_base64)
    {
        try{
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", port);
            byte[] png = Encoding.Default.GetBytes(pic_base64);
            byte[] png_length = Encoding.Default.GetBytes(png.Length.ToString());
            client.GetStream().Write(png_length, 0, png_length.Length);
            Thread.Sleep(500);
            client.GetStream().Write(png, 0, png.Length);  
            client.Close();
        }
        catch{

        }
        //client.Close();
/*             TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 9999);
        byte[] png = Encoding.Default.GetBytes(pic_base64);
        byte[] png_length = Encoding.Default.GetBytes(png.Length.ToString());
        client.GetStream().Write(png_length, 0, png_length.Length);
        Thread.Sleep(500);
        client.GetStream().Write(png, 0, png.Length);
        //client.Close();

        client.Close(); */
    }

    public GameObject gm;

    // New
    private void ConnectTCP()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    public void ReceiveData()
    {
        try
        {
            print("Waiting");
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            print("Server is listening");
            Byte[] bytes = new Byte[4096];

            client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            int length;
            length = stream.Read(bytes, 0, bytes.Length);
            var incommingData = new byte[length];
            Array.Copy(bytes, 0, incommingData, 0, length);
            clientMessage = Encoding.ASCII.GetString(incommingData);
            print("client message received as:" + clientMessage);
            Debug.Log(clientMessage);

            client.Close();


            /* int loopNum = 0;
            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            //
                            print("client message received as:" + clientMessage);
                            //
                        }
                    }
                }
                if(loopNum++ > 10000)
                    throw new Exception("Infinite Loop");             
            } */
        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    }

    // Recieve class name from python
    /* private void ReceiveClass()
    {
        try
        {
            print("Waiting");
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            print("Server is listening");
            Byte[] bytes = new Byte[1024];

            string a;
            // int     loopNum = 0;
            int loopNum = 0;
            // int loopNum = 0;
            while (true)
            {
                using (client = listener.AcceptTcpClient())
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            //
                            print("client message received as:" + clientMessage);
                            Debug.Log(clientMessage);
                            //
                            client.Close();
                            a = clientMessage;
                        }
                        //stream.Close();
                        
                    }
                    
                    
                }
            }
            //manager_game에 전달
            gm.GetComponent<manager_game>().next_obj(a);

        }
        catch (Exception e)
        {
            print(e.ToString());
        }
    } */


}