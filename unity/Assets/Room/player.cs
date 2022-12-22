using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //GameManager
    public GameManager GameManager;

    
    // 출력변수
    public int hand_state; // output
    public Vector3 pen_position;
    public Vector3 eraser_position;


    // movement variables
    public float object_distance_threshold;
    public float Speed = 10.0f;
    public Transform hand_trans;
    private Vector3 hand_pos;

    public float move_speed;
    public Vector2 rotation_threshold;
    public float rotation_speed;
    // Start is called before the first frame update
    void Start()
    {  
        hand_state = 1;
        
        //movment setting
        move_speed = 1.5f;
        rotation_speed = 50f;
        rotation_threshold = new Vector2 (0.3f,0.15f);
        hand_trans = transform.GetChild(0);

        // GameManager...
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        object_distance_threshold = 2f;

        // object position
        pen_position = GameManager.object_manager["ballpoint_pen_black"].transform.position;
        eraser_position = GameManager.object_manager["eraser"].transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        // Player(Camera) movement
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
 
        Vector3 dirXY = (transform.right * dirX) + new Vector3(transform.forward.x, 0,transform.forward.z) * dirY;
 
        transform.position += dirXY * move_speed* Time.deltaTime;

        // Camera rotation( due to hand position)
        hand_pos = Camera.main.WorldToViewportPoint(hand_trans.position);    
        if (hand_pos.x < (rotation_threshold.x)){
            transform.Rotate(-Vector3.up, rotation_speed*Time.deltaTime);
            // transform.lo
        }
        else if (hand_pos.x > (1 - rotation_threshold.x)){
            transform.Rotate(Vector3.up,rotation_speed*Time.deltaTime);
        }

        if (hand_pos.y< (rotation_threshold.y )){
            transform.Rotate(Vector3.right,rotation_speed*Time.deltaTime);
        }
        else if(hand_pos.y > (0.9f-rotation_threshold.y)){
            transform.Rotate(-Vector3.right, rotation_speed*Time.deltaTime);
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,0);
    }

    // 물체 잡으러 갈건지
    void FixedUpdate(){

        int hand_shape = transform.GetChild(0).GetComponent<Hand_Control>().hand_shape;

        if (hand_shape == 1 ){
            try{
                
                if(Vector3.Distance(transform.position, pen_position) < object_distance_threshold){
                    hand_state = 2;
                }
                else if (Vector3.Distance(transform.position, eraser_position) < object_distance_threshold){
                    hand_state = 3;
                }
            }
            catch{
                Debug.Log("No object");
            }
            
        }
    }

}
