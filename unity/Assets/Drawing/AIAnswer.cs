using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEditor;



public class AIAnswer : MonoBehaviour
{     
    // Text형 변수 생성
    // TCP stuff
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    int port = 9122;

    // Text형 변수 생성
    public Text ScriptTxt;
    
    public String clientMessage;
    
    // 추가
    public bool socketReady;

    void Start()
    {
        port = 9122;
        // 처음(socket 통신 이전) text 존재하지 않음
        // ScriptTxt.text = "...";
        // Text 컴포넌트 접근
        ScriptTxt = GetComponent<Text>();  
        // ConnectTCP() 함수 실행
        ConnectTCP();
        
    }

    // New
    public void ConnectTCP()
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
            string clientMessage = Encoding.ASCII.GetString(incommingData);
            print("client message received as:" + clientMessage);
            Debug.Log(clientMessage);
            ScriptTxt.text = clientMessage.ToString();

            // return clientMessage;
            }
        catch //(Exception e)
        {
                //print(e.ToString());
        }

                
    }

    

   /*  void Update()
    {
    
        if(socketReady)
        ReceiveData();
        // 받아온 message를 UI에 표시
        ScriptTxt.text = clientMessage.ToString();
        
    } */


    void OnApplicationQuit()
    {
        // close the thread when the application quits
        receiveThread.Abort();
    }

}
