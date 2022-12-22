using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Control : MonoBehaviour
{
 

    private List<Transform> hand_list = new List<Transform>();
    

    // control vaiables
    public float hand_scale = 10000;

    public Vector3 hand_pos_bias;
    public Vector3 hand_angle_bias;
    public float move_scale;
    public float z_scale;
    public float time_weight;
    public float angle_weight;
    
    public float angle_limit;
    public float angle_threshold; // classification을 위한 threshold 설정

    //grabbing function을 위한변수
    public float grab_speed;
    public float grabbed;

    

    // 받아올 전역 변수/오브젝트
    private GameManager GameManager; 

    private player player;

    public UDPReceive udpReceive;
    

    
    /// 아래변수는 player script 로 관리
    public int hand_shape;      // output 변수
    public int object_detected;
    public float finger_state;

    
    Vector3[] right_hand_lm = new Vector3[21];

    // Vector3[] left_hand_lm = new Vector3[21];

    Vector3[] initial_finger_rot = new Vector3[21];
    // for Calculation Memory(for fingers)
    // Vector3 a0, a1, a2, a5,a6,a7,a8, a17 ;

    private Vector3 original_pos;
    private Vector3 pen_position;
    private Vector3 eraser_position;
    void Start()
    {
        hand_shape = 0;
        // 받아올 전역변수/오브젝트
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = transform.parent.GetComponent<player>();   

        udpReceive = GameObject.Find("UDP_Receive").GetComponent<UDPReceive>();
        
        pen_position = GameManager.object_manager["ballpoint_pen_black"].transform.position;
        eraser_position = GameManager.object_manager["eraser"].transform.position;
        
        //
        angle_limit = 10;
        angle_threshold = 40;
        finger_state = 0;
        //
        move_scale = 0.001f;
        z_scale = 1f;
        grab_speed = 5f;

        angle_weight =0.8f;
        time_weight = 10f;
        //
        // move_scale = 0.001f;
        hand_pos_bias = new Vector3(0.5f, -0.2f, 0.55f);
        // hand_angle_bias = new Vector3(46f,-277f,188f);
        hand_angle_bias = new Vector3(51.37f,-314.188f,155.298f);

        // 손 전체 각도 변환(초기 값)
        transform.Find("hands:r_hand_world").localEulerAngles = hand_angle_bias;
                

        // 0 
        hand_list.Add(transform.Find("hands:r_hand_world").transform.Find("hands:b_r_hand"));
        // Thumb
        hand_list.Add(hand_list[0].Find("hands:b_r_thumb1"));
        hand_list.Add(hand_list[1].Find("hands:b_r_thumb2"));
        hand_list.Add(hand_list[2].Find("hands:b_r_thumb3"));
        hand_list.Add(hand_list[3].Find("hands:b_r_thumb_ignore"));
        // index
        hand_list.Add(hand_list[0].Find("hands:b_r_index1"));
        hand_list.Add(hand_list[5].Find("hands:b_r_index2"));
        hand_list.Add(hand_list[6].Find("hands:b_r_index3"));
        hand_list.Add(hand_list[7].Find("hands:b_r_index_ignore"));

        // middle
        hand_list.Add(hand_list[0].Find("hands:b_r_middle1"));
        hand_list.Add( hand_list[9].Find("hands:b_r_middle2"));
        hand_list.Add( hand_list[10].Find("hands:b_r_middle3"));
        hand_list.Add( hand_list[11].Find("hands:b_r_middle_ignore"));
        // ring
        hand_list.Add( hand_list[0].Find("hands:b_r_ring1"));
        hand_list.Add( hand_list[13].Find("hands:b_r_ring2"));
        hand_list.Add( hand_list[14].Find("hands:b_r_ring3"));
        hand_list.Add( hand_list[15].Find("hands:b_r_ring_ignore"));

        // piik
        hand_list.Add( hand_list[0].Find("hands:b_r_pinky0").transform.Find("hands:b_r_pinky1"));
        hand_list.Add( hand_list[17].Find("hands:b_r_pinky2"));
        hand_list.Add( hand_list[18].Find("hands:b_r_pinky3"));
        hand_list.Add( hand_list[19].Find("hands:b_r_pinky_ignore"));
        


        for (int i=0 ; i<21; i++){
            initial_finger_rot[i] = hand_list[i].transform.localEulerAngles;
        }
 
    }

   void hand_control(){
        // Client
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        string[] string_array = data.Split(',');

        // hand landmark 좌표 저장
        for (int i=0; i <21; i++){
            right_hand_lm[i] = new Vector3(float.Parse(string_array[1+i*3+0]),float.Parse(string_array[1+i*3+1]),float.Parse(string_array[1+i*3+2])*z_scale);
        }

        // hand move, rotate

        transform.localPosition = new Vector3(-right_hand_lm[0].x, right_hand_lm[0].y,0)*move_scale + hand_pos_bias;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_weight);
        // transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_weight);

        // finger control
        float finger_angle; // finger_angle 값: 굽을수록 0 -> -90도로 움직임
        finger_state = 0;
        for (int i=0; i<5; i++){
            finger_angle = Vector3.SignedAngle(right_hand_lm[1+i*4+1] - right_hand_lm[1+i*4], right_hand_lm[1+i*4]-right_hand_lm[0], -transform.right)*angle_weight;
            if (finger_angle < angle_limit){
                hand_list[1+i*4].transform.localRotation = Quaternion.Lerp( hand_list[1+i*4].transform.localRotation, Quaternion.Euler(initial_finger_rot[1+i*4] + new Vector3(0,0,finger_angle)),Time.deltaTime*time_weight);               
                
                if (Mathf.Abs(finger_angle) > angle_threshold){
                    finger_state++;
                }
            }
            
            for(int j=1; j<=2;j++){
                finger_angle = Vector3.SignedAngle(right_hand_lm[1+i*4+j+1] - right_hand_lm[1+i*4+j], right_hand_lm[1+i*4+j]-right_hand_lm[1+i*4+j-1],-transform.right)*angle_weight;
                if (finger_angle < angle_limit){
                    hand_list[1+i*4+j].transform.localRotation = Quaternion.Lerp( hand_list[1+i*4+j].transform.localRotation,Quaternion.Euler(initial_finger_rot[1+i*4+j] + new Vector3(0,0,finger_angle)), Time.deltaTime*time_weight);
                    if (Mathf.Abs(finger_angle) > angle_threshold){
                        // Debug.Log(finger_angle);
                        finger_state++;
                    }
                    
                }
                
            }
        }

    }

    // grabbing Movement
    private IEnumerator grab_pen()
    {
        while(grabbed < 2){
            if(grabbed == 0){
                transform.position = Vector3.Lerp(transform.position, pen_position, Time.deltaTime * grab_speed);   
                if(transform.position == pen_position){
                    grabbed =1;
                    Debug.Log("grabbed=1");
                } 
            }
            else if (grabbed ==1){
                transform.position = Vector3.Lerp(transform.position, original_pos, Time.deltaTime * grab_speed);
                if(transform.position == original_pos){
                    grabbed =2;
                    player.hand_state = 1;
                    // sleep(1f);
                    GameManager.object_manager["ballpoint_pen_black"].gameObject.SetActive(false);
                    GameManager.player_object["ballpoint_pen_black"] = 1;
                    Debug.Log("grabbed=2");
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);
            
        }
    }

    private IEnumerator grab_eraser()
    {
        while(grabbed < 2){
            if(grabbed == 0){
                transform.position = Vector3.Lerp(transform.position, eraser_position, Time.deltaTime * grab_speed);   
                if(transform.position == eraser_position){
                    grabbed =1;
                    Debug.Log("grabbed=1");
                } 
            }
            else if (grabbed ==1){
                transform.position = Vector3.Lerp(transform.position, original_pos, Time.deltaTime * grab_speed);
                if(transform.position == original_pos){
                    grabbed =2;
                    player.hand_state = 1;
                    // sleep(1f);
                    GameManager.object_manager["eraser"].gameObject.SetActive(false);
                    GameManager.player_object["eraser"] = 1;
                    Debug.Log("grabbed=2");
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        int hand_state = player.hand_state;
        // grabbing pen
        if (hand_state == 2)        
        {
            grabbed = 0;
            original_pos = transform.position;
            StartCoroutine("grab_pen");

        }
        // grabbing eraser
        else if (hand_state == 3){
            grabbed = 0;
            original_pos = transform.position;
            StartCoroutine("grab_eraser");
            
        }
        else if (hand_state == 1)
        {
            try
            {
                hand_control();
                if (finger_state > 10)
                {
                    hand_shape = 1;     // 완전 주먹
                }
                else
                {
                    hand_shape = 0;
                }
            }
            catch
            {
                Debug.Log("no hand");
            }

        }



    }

}

