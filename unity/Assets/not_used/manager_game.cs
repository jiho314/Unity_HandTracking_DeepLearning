// using System.Collections;
// using System.Collections.Generic;
// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using CustomUtils;
// using UnityEngine.SceneManagement;

// public class manager_game : MonoBehaviour
// {
//     private string clientMessage = "";
//     private string prevclientMessage = "";
//     private int submit_count;
//     public int stage = 0;
//     public Text gamestart;
//     public Text question;
//     public Text timeout;
//     public Text result;
//     public GameObject time;
//     public GameObject cap;
//     public string[] object_list = new string[] { "sun", "cloud", "eye" };

//     press_button press_button;
//     // Start is called before the first frame update
//     void Start()
//     {
//         press_button = GameObject.Find("Player_Camera").transform.GetChild(0).transform.Find("Pen black").transform.Find("Cap").GetComponent<press_button>();

//         submit_count = press_button.submit_count;
//         time = transform.Find("DialSeconds").transform.Find("timer").gameObject;
//         question.text = object_list[stage]; //ù��° object
//         question.gameObject.SetActive(true);
//         gamestart.gameObject.SetActive(true);
//     }
    

//     // Update is called once per frame
//     void Update()
//     {
//         submit_count = press_button.submit_count;

//         clientMessage = GameObject.Find("ScreenShotCamera").GetComponent<ScreenShot>().clientMessage;
//         if(prevclientMessage != clientMessage) //수신
//         {
//             if (clientMessage == object_list[stage])
//             {
//                 result.text = clientMessage + "Correct";
//             }
//             else
//             {
//                 result.text = "answer:" + object_list[stage] + "your picture:" + "class_";
//             }
//             result.gameObject.SetActive(true);
//             cap.GetComponent<TrailRenderer>().Clear();

//             stage += 1;
//             question.text = object_list[stage];
//             time.GetComponent<timer>().resetTime();
//             prevclientMessage = String.Copy(clientMessage);
//         }

//         float t = time.GetComponent<timer>().setTime;

//         if (t < 55) //55�ʿ� ���� txt ��Ȱ��ȭ
//         {
//             gamestart.gameObject.SetActive(false);
//             timeout.gameObject.SetActive(false);
//             result.gameObject.SetActive(false);
//         }

//         if (stage == 3)
//         {
//             if (t<55)
//             {
//                 SceneManager.LoadScene("Room");
//             }
            
//         }
//     }

//     public void timeoutf()
//     {
//         timeout.gameObject.SetActive(true);
//         //���� ����
//         ScreenShot ss = new ScreenShot();
//         ss.ScreenShotClick();
//         cap.GetComponent<press_button>().click_state = 0;
//     }
// }
