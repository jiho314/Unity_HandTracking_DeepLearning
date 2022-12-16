using System.Collections;
using System.Collections.Generic;
using UnityEngine;

        

public class Hand_control_test : MonoBehaviour
{
 
    public UDPReceive udpReceive;
    private List<Transform> hand_list = new List<Transform>();
    // Start is called before the first frame update
    public float hand_scale = 10000;

    public Vector3 hand_bias;
    public float move_scale;
    public float z_scale;
    public float time_weight;
    public float angle_weight;
    
    public float angle_limit;
    public float angle_threshold; // classification을 위한 threshold 설정


    /// 이거는ㄴ GAMemanager로 관리

    public float hand_state;
    public float finger_state;
    
    Vector3[] right_hand_lm = new Vector3[21];

    // Vector3[] left_hand_lm = new Vector3[21];

    Vector3[] initial_finger_rot = new Vector3[21];
    // for Calculation Memory(for fingers)
    // Vector3 a0, a1, a2, a5,a6,a7,a8, a17 ;


    void Start()
    {
        //
        angle_limit = 10;
        angle_threshold = 40;
        finger_state = 0;

        //
        move_scale = 0.001f;
        z_scale = 1f;

        angle_weight =0.8f;
        time_weight = 5f;
        // move_scale = 0.001f;
        hand_bias = new Vector3(-0.8f, 0f, 0f);
                
        Transform A =  transform.Find("hands:r_hand_world").transform.Find("hands:b_r_hand");
        Debug.Log(A.position);

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
        // hand_list[0] = transform.Find("hands:r_hand_world").transform.Find("hands:b_r_hand").transform;
        
        // // Thumb
        // hand_list[1] = hand_list[0].Find("hands:b_r_thumb1").transform;
        // hand_list[2] = hand_list[1].Find("hands:b_r_thumb2").transform;
        // hand_list[3] = hand_list[2].Find("hands:b_r_thumb3").transform;
        // hand_list[4] = hand_list[3].Find("hands:b_r_thumb_ignore").transform;
        // // index
        // hand_list[5] = hand_list[0].Find("hands:b_r_index1").transform;
        // hand_list[6] = hand_list[5].Find("hands:b_r_index2").transform;
        // hand_list[7] = hand_list[6].Find("hands:b_r_index3").transform;
        // hand_list[8] = hand_list[7].Find("hands:b_r_index_ignore").transform;

        // // middle
        // hand_list[9] = hand_list[0].Find("hands:b_r_middle1").transform;
        // hand_list[10] = hand_list[9].Find("hands:b_r_middle2").transform;
        // hand_list[11] = hand_list[10].Find("hands:b_r_middle3").transform;
        // hand_list[12] = hand_list[11].Find("hands:b_r_middle_ignore").transform;
        // // ring
        // hand_list[13] = hand_list[0].Find("hands:b_r_ring1").transform;
        // hand_list[14] = hand_list[13].Find("hands:b_r_ring2").transform;
        // hand_list[15] = hand_list[14].Find("hands:b_r_ring3").transform;
        // hand_list[16] = hand_list[15].Find("hands:b_r_ring_ignore").transform;

        // // piik
        // hand_list[17] = hand_list[0].Find("hands:b_r_pinky0").transform.Find("hands:b_r_pinky1").transform;
        // hand_list[18] = hand_list[17].Find("hands:b_r_pinky2").transform;
        // hand_list[19] = hand_list[18].Find("hands:b_r_pinky3").transform;
        // hand_list[20] = hand_list[19].Find("hands:b_r_pinky_ignore").transform;
        
        // Debug.Log(hand_list[20].position);


    }

    // Update is called once per frame
    void Update()
    { 
        // Debug.Log(transform.rotation);
        // Debug.Log(transform.localRotation);
        // Debug.Log(transform.localEulerAngles);
        // Debug.Log("transform.up: ", transform.up);
        // Debug.Log( Vector3.Cross(a5-a0,a17-a0));
        try{
            // Client
            string data = udpReceive.data;
            data = data.Remove(0, 1);
            data = data.Remove(data.Length - 1, 1);
            string[] string_array = data.Split(',');

            // hand landmark 좌표 저장
            for (int i=0; i <21; i++){
                // float xScreen = xScreen.width * (float.Parse(string_array[1+i*3+0] - 0.5f *(1-c)
                right_hand_lm[i] = new Vector3(float.Parse(string_array[1+i*3+0]),float.Parse(string_array[1+i*3+1]),float.Parse(string_array[1+i*3+2])*z_scale);
            }
                        
            transform.SetPositionAndRotation(right_hand_lm[0]*move_scale + hand_bias ,Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_weight) );
    
            // Lerp 방식
            // 2, 3 번째 마디들

            // finger_angle 값: 굽을수록 0 -> -90도로 움직임
            float finger_angle;
            finger_state = 0;
            for (int i=0; i<5; i++){
                finger_angle = Vector3.SignedAngle(right_hand_lm[1+i*4+1] - right_hand_lm[1+i*4], right_hand_lm[1+i*4]-right_hand_lm[0],transform.right)*angle_weight;
                if (finger_angle < angle_limit){
                    hand_list[1+i*4].transform.localRotation = Quaternion.Lerp( hand_list[1+i*4].transform.localRotation, Quaternion.Euler(initial_finger_rot[1+i*4] + new Vector3(0,0,finger_angle)),Time.deltaTime*time_weight);               
                    
                    if (Mathf.Abs(finger_angle) > angle_threshold){
                        finger_state++;
                    }
                }
                
                for(int j=1; j<=2;j++){
                    finger_angle = Vector3.SignedAngle(right_hand_lm[1+i*4+j+1] - right_hand_lm[1+i*4+j], right_hand_lm[1+i*4+j]-right_hand_lm[1+i*4+j-1],transform.right)*angle_weight;
                    if (finger_angle < angle_limit){
                        hand_list[1+i*4+j].transform.localRotation = Quaternion.Lerp( hand_list[1+i*4+j].transform.localRotation,Quaternion.Euler(initial_finger_rot[1+i*4+j] + new Vector3(0,0,finger_angle)), Time.deltaTime*time_weight);
                        if (Mathf.Abs(finger_angle) > angle_threshold){
                            // Debug.Log(finger_angle);
                            finger_state++;
                        }
                        
                    }
                    
                }
            }
            if (finger_state > 10){
                hand_state = 1;
            }
            else{
                hand_state = 0;
            }
            Debug.Log(hand_state);


            // No Lerp 방식
            // // 2, 3 번째 마디들
            // for (int i=0; i<5; i++){
            //     hand_list[1+i*4].transform.localEulerAngles =  initial_finger_rot[1+i*4] + new Vector3(0,0,Vector3.SignedAngle(right_hand_lm[1+i*4+1] - right_hand_lm[1+i*4], right_hand_lm[1+i*4]-right_hand_lm[0],transform.right)*angle_weight);
            //     for(int j=1; j<=2;j++){
            //         hand_list[1+i*4+j].transform.localEulerAngles = initial_finger_rot[1+i*4+j] + new Vector3(0,0,Vector3.SignedAngle(right_hand_lm[1+i*4+j+1] - right_hand_lm[1+i*4+j], right_hand_lm[1+i*4+j]-right_hand_lm[1+i*4+j-1],transform.right)*angle_weight);
            //     }
            // }
            // // 첫 마디: default회전 각도가 각기다름, unity asset에서 직접 참고
            // // hand_list[1].transform.localEulerAngles =(new Vector3(0,0,Vector3.SignedAngle(a7-a6, a6-a5,transform.right)*angle_weight));
            // hand_list[5].transform.localEulerAngles = (new Vector3(-74.875f , 106.624f ,75+ Vector3.SignedAngle(right_hand_lm[6] - right_hand_lm[5], right_hand_lm[5]-right_hand_lm[0],transform.right)*angle_weight));
            // hand_list[9].transform.localEulerAngles =(new Vector3(-80.399f,18.783f,151.906f + Vector3.SignedAngle(right_hand_lm[10] - right_hand_lm[9], right_hand_lm[9]-right_hand_lm[0],transform.right)*angle_weight));
            // hand_list[13].transform.localEulerAngles =(new Vector3(-69.153f,-21.521f,-169.447f + Vector3.SignedAngle(right_hand_lm[14] - right_hand_lm[13], right_hand_lm[13]-right_hand_lm[0],transform.right)*angle_weight));
            // hand_list[17].transform.localEulerAngles =(new Vector3(-53.777f,-22.572f,22.88f + Vector3.SignedAngle(right_hand_lm[18] - right_hand_lm[17], right_hand_lm[17]-right_hand_lm[0],transform.right)*angle_weight));
            
            // transform.SetPositionAndRotation(right_hand_lm[0]*move_scale+hand_bias ,Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_weight) );
    
         

            // // finger vector 저장
            // a0 = new Vector3(float.Parse(string_array[1]),float.Parse(string_array[2]),float.Parse(string_array[3])*z_scale); 
            // // transform.position = a1 * move_scale + bias;

            
            // // hand rotation
            // a5 = new Vector3(float.Parse(string_array[1+3*5]),float.Parse(string_array[1+3*5+1]),float.Parse(string_array[1+3*5+2])*z_scale);
            // a17 = new Vector3(float.Parse(string_array[1+3*17]),float.Parse(string_array[1+3*17+1]),float.Parse(string_array[1+3*17+2])*z_scale);

            // transform.SetPositionAndRotation(right_hand_lm[0]*move_scale+bias ,Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_delta) );
    
            
            // // index finger
            // a6 = new Vector3(float.Parse(string_array[1+3*6]),float.Parse(string_array[1+3*6+1]),float.Parse(string_array[1+3*6+2])*z_scale);
            // a7 = new Vector3(float.Parse(string_array[1+3*7]),float.Parse(string_array[1+3*7+1]),float.Parse(string_array[1+3*7+2])*z_scale);
            // a8 =new Vector3(float.Parse(string_array[1+3*8]),float.Parse(string_array[1+3*8+1]),float.Parse(string_array[1+3*8+2])*z_scale);
            
            // hand_list[5].transform.localEulerAngles = (new Vector3(-74.875f,106.624f,75+ Vector3.SignedAngle(a6-a5, a5-a0,transform.right)*angle_weight));
            // hand_list[6].transform.localEulerAngles =(new Vector3(0,0,Vector3.SignedAngle(a7-a6, a6-a5,transform.right)*angle_weight));
            // hand_list[7].transform.localEulerAngles =(new Vector3(0,0, Vector3.SignedAngle(a8-a7, a7-a6,transform.right)*angle_weight));
            




            // for ( int j=0; j<5 ;  j++){

            //     a2 =  new Vector3(float.Parse(string_array[1+(j*4+1)*4+0]),float.Parse(string_array[1+(j*4+1)*4+1]),float.Parse(string_array[1+(j*4+1)*4+2]));
            //     right_hand_finger[j*4+1] = a2 - a0;


            //     for (int i=1; i <4; i++){
            //         a1 = a2;                
            //         a2 =  new Vector3(float.Parse(string_array[1+(j*4+1+i)*4+0]),float.Parse(string_array[1+(j*4+1+i)*4+1]),float.Parse(string_array[1+(j*4+1+i)*4+2]));
            //         right_hand_finger[ j*4+1 +i] = a2 - a1;

                    
            //     }
            // }
            // Debug.Log("finish handvectors");
   
    
            // // index finger
            // // default: x =-74.845 , y = 0 , z =180
            // Vector3 index_plane_normal = right_hand_finger[17]-Vector3.Project(right_hand_finger[17], right_hand_finger[5]);
            

            // Debug.Log(Vector3.SignedAngle(right_hand_finger[5],right_hand_finger[6],index_plane_normal));
            // // hand_list[5].transform.localEulerAngles = new Vector3(-90f, 0f, 180f - Vector3.SignedAngle(right_hand_finger[6],right_hand_finger[5],index_plane_normal));
            
            // // hand_list[5].transform.localEulerAngles = new Vector3(-90f,0f, (180f -  Vector3.SignedAngle(right_hand_finger[5], Vector3.ProjectOnPlane( right_hand_finger[6],index_plane_normal),index_plane_normal )));
            
            

            

            // Debug.Log(hand_list[5]);
            // Debug.Log(hand_list[5].transform.rotation);
            // Debug.Log(hand_list[5].transform.localEulerAngles);
            // // Debug.Log(Vector3.SignedAngle(right_hand_finger[5], Vector3.ProjectOnPlane( right_hand_finger[6],index_plane_normal),index_plane_normal ));
            
            // Vector3 hand_normal = Vector3.Cross(right_hand_finger[5], right_hand_finger[17]);


            // index finger
            // index: default 70 , 0-> 5, 5-> 6


            



            // float x = float.Parse(points[1 * 4 + 1]) / hand_scale;
            // float y = float.Parse(points[1 * 4 + 2]) / hand_scale;
            // float z = float.Parse(points[1 * 4 + 3]) / hand_scale;
            // hand_list[0].position = new Vector3(x,y,z);

            // for (int j=0; j <5; j++){
            //     x = float.Parse(points[(j*4 + 1) * 4 + 1]) / hand_scale;
            //     y = float.Parse(points[(j*4 + 1) * 4 + 2]) / hand_scale;
            //     z = float.Parse(points[(j*4 + 1) * 4 + 3]) / hand_scale;
            //     hand_list[j*4+1].position = new Vector3(x, y, z) - hand_list[0].position;


            //     for ( int i=2; i<5; i++)
            //     {
            //         x = float.Parse(points[(j*4 + i) * 4 + 1]) / hand_scale;
            //         y = float.Parse(points[(j*4 + i) * 4 + 2]) / hand_scale;
            //         z = float.Parse(points[(j*4 + i) * 4 + 3]) / hand_scale;
                    
            //         hand_list[j*4+i].position = new Vector3(x, y, z);
            //         // - hand_list[i-1].position;
            //     }
            //}
        }
        catch{
            Debug.Log("No Hand");
        }
    }

}

