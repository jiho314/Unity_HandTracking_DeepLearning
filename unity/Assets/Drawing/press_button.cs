using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using CustomUtils;

public class press_button : MonoBehaviour
{

    public string client_message;

    public int click_state; // 1: can send, 0 : sent

    private int eraser_state;

    private TrailRenderer trail;
    public GameObject hand;
    private float handx;
    private float handy;
    private float erase_minx;
    private float erase_maxx;
    private float erase_miny;
    private float erase_maxy;
    private float end_minx;
    private float end_maxx;
    private float end_miny;
    private float end_maxy;
    private hand hand_s;
    GameManager GameManager;
    public int submit_count;

    // Start is called before the first frame update
    void Start()
    {
        submit_count = 0;
        click_state =1;
        
        trail = GameObject.Find("Player_Camera").transform.GetChild(0).transform.Find("Pen black").transform.Find("Cap").GetComponent<TrailRenderer>();
        hand = GameObject.Find("Custom Right Hand Model with Collider");
        hand_s = GameObject.Find("Custom Right Hand Model with Collider").GetComponent<hand>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        eraser_state = GameManager.player_object["eraser"];


        
        erase_minx = 1f;
        erase_maxx = 1.8f;
        erase_miny = -0.1f;
        erase_maxy = 1.6f;

        end_minx = 1f;
        end_maxx = 1.8f;
        end_maxy = -0.23f;
        end_miny = -0.85f;

    }
    // Update is called once per frame
    void Update()
    {
        client_message = GameObject.Find("ScreenShotCamera").GetComponent<ScreenShot>().clientMessage;


        Vector3 hand_pos_cam = Camera.main.WorldToViewportPoint(hand.transform.position);    
            
        handx = hand.transform.localPosition.x;
        handy = hand.transform.localPosition.y;
        
        if (handx >= erase_minx && handx <= erase_maxx && handy >= erase_miny && handy <= erase_maxy)
        {
            if (hand_s.hand_shape == 1 && eraser_state == 1 ){
                trail.Clear();
            }
        }


        else if (handx >= end_minx && handx <= end_maxx && handy >= end_miny && handy <= end_maxy)
        {
            ScreenShot ss = new ScreenShot();

            if (hand_s.hand_shape == 1 && click_state == 1)
                click_state =0;
                ss.ScreenShotClick();
                submit_count += 1;
                

        }
        else if (hand_s.hand_shape ==1 && handx < -1.5f && handx >-1.9f && handy > 0.55f && handy < 0.9f){
            
            SceneManager.LoadScene("Room");
        } 
        
        
        else {
            click_state = 1 ;
        }
    }
}