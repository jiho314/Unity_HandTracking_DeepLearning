using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class your_answer : MonoBehaviour
{

    press_button press_button;
    public string client_message1;
    
    public string client_message2;
    // Start is called before the first frame update
    void Start()
    {
        press_button = GameObject.Find("Player_Camera").transform.GetChild(0).Find("Pen black").transform.Find("Cap").GetComponent<press_button>();
        
    }

    // Update is called once per frame
    void Update()
    {
        client_message1 = press_button.client_message;
        client_message2 = GameObject.Find("ScreenShotCamera").GetComponent<ScreenShot>().clientMessage;

        this.GetComponent<Text>().text =client_message1; 

    }
}
