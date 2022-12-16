using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour
{


    private int pen_state;
    private int eraser_state;
    public Vector3 pen_bias;
    public Vector3 pen_angle;

    private Transform pen_black;
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
    public float angle_threshold; // classification�� ���� threshold ����

    public float grab_speed;



    // �޾ƿ� ���� ����/������Ʈ
    private GameManager GameManager;

    private player player;

    public UDPReceive udpReceive;



    /// �Ʒ������� player script �� ����
    public int hand_shape;      // output ����
    public int object_detected;
    public float finger_state;


    Vector3[] right_hand_lm = new Vector3[21];

    // Vector3[] left_hand_lm = new Vector3[21];

    Vector3[] initial_finger_rot = new Vector3[21];
    // for Calculation Memory(for fingers)
    // Vector3 a0, a1, a2, a5,a6,a7,a8, a17 ;


    void Start()
    {
        // pen_bias = new Vector3(0.0354f, 0.1138f, -0.1351f);
        // pen_angle = new Vector3( -201.212f, 32.01401f, 173.986f);

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = transform.parent.GetComponent<player>();
        // 일단 펜 있는걸루
       pen_state =1; 
       eraser_state = 1;
        // pen_state = GameManager.player_object["ballpoint_pen_black"];
        // eraser_state = GameManager.player_object["eraser"];
        


        pen_black =  transform.Find("Pen black");
        pen_bias = new Vector3(0.0597f,0.1382f, -0.0814f);
        pen_angle = new Vector3( -200.729f, 22.05499f, 172.43f);
        transform.Find("Pen black").localPosition = pen_bias;
        transform.Find("Pen black").localEulerAngles = pen_angle;

        hand_shape = 0;
        // �޾ƿ� ��������/������Ʈ

        udpReceive = GameObject.Find("UDP_Receive").GetComponent<UDPReceive>();


        //
        angle_limit = 10;
        angle_threshold = 40;
        finger_state = 0;
        //
        move_scale = 0.008f;
        z_scale = 1f;
        grab_speed = 10f;

        angle_weight = 0.8f;
        time_weight = 10f;
        //
        // move_scale = 0.001f;
        hand_pos_bias = new Vector3(4f, -1.2f, 2f);
        // hand_angle_bias = new Vector3(46f,-277f,188f);
        hand_angle_bias = new Vector3(51.37f, -314.188f, 155.298f);

        // �� ��ü ���� ��ȯ(�ʱ� ��)
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
        hand_list.Add(hand_list[9].Find("hands:b_r_middle2"));
        hand_list.Add(hand_list[10].Find("hands:b_r_middle3"));
        hand_list.Add(hand_list[11].Find("hands:b_r_middle_ignore"));
        // ring
        hand_list.Add(hand_list[0].Find("hands:b_r_ring1"));
        hand_list.Add(hand_list[13].Find("hands:b_r_ring2"));
        hand_list.Add(hand_list[14].Find("hands:b_r_ring3"));
        hand_list.Add(hand_list[15].Find("hands:b_r_ring_ignore"));

        // piik
        hand_list.Add(hand_list[0].Find("hands:b_r_pinky0").transform.Find("hands:b_r_pinky1"));
        hand_list.Add(hand_list[17].Find("hands:b_r_pinky2"));
        hand_list.Add(hand_list[18].Find("hands:b_r_pinky3"));
        hand_list.Add(hand_list[19].Find("hands:b_r_pinky_ignore"));



        for (int i = 0; i < 21; i++)
        {
            initial_finger_rot[i] = hand_list[i].transform.localEulerAngles;
        }

    }

    void hand_control()
    {
        // Client
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        string[] string_array = data.Split(',');

        // hand landmark ��ǥ ����
        for (int i = 0; i < 21; i++)
        {
            right_hand_lm[i] = new Vector3(float.Parse(string_array[1 + i * 3 + 0]), float.Parse(string_array[1 + i * 3 + 1]), float.Parse(string_array[1 + i * 3 + 2]) * z_scale);
        }

        // hand move, rotate

        transform.localPosition = new Vector3(-right_hand_lm[0].x, right_hand_lm[0].y, 0) * move_scale + hand_pos_bias;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5] - right_hand_lm[0], right_hand_lm[17] - right_hand_lm[0])), Time.deltaTime * time_weight);
        // transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(-Vector3.Cross(right_hand_lm[5]-right_hand_lm[0],right_hand_lm[17]-right_hand_lm[0])), Time.deltaTime*time_weight);

        // finger control
        float finger_angle; // finger_angle ��: �������� 0 -> -90���� ������
        finger_state = 0;
        for (int i = 0; i < 5; i++)
        {
            finger_angle = Vector3.SignedAngle(right_hand_lm[1 + i * 4 + 1] - right_hand_lm[1 + i * 4], right_hand_lm[1 + i * 4] - right_hand_lm[0], -transform.right) * angle_weight;
            if (finger_angle < angle_limit)
            {
                hand_list[1 + i * 4].transform.localRotation = Quaternion.Lerp(hand_list[1 + i * 4].transform.localRotation, Quaternion.Euler(initial_finger_rot[1 + i * 4] + new Vector3(0, 0, finger_angle)), Time.deltaTime * time_weight);

                if (Mathf.Abs(finger_angle) > angle_threshold)
                {
                    finger_state++;
                }
            }

            for (int j = 1; j <= 2; j++)
            {
                finger_angle = Vector3.SignedAngle(right_hand_lm[1 + i * 4 + j + 1] - right_hand_lm[1 + i * 4 + j], right_hand_lm[1 + i * 4 + j] - right_hand_lm[1 + i * 4 + j - 1], -transform.right) * angle_weight;
                if (finger_angle < angle_limit)
                {
                    hand_list[1 + i * 4 + j].transform.localRotation = Quaternion.Lerp(hand_list[1 + i * 4 + j].transform.localRotation, Quaternion.Euler(initial_finger_rot[1 + i * 4 + j] + new Vector3(0, 0, finger_angle)), Time.deltaTime * time_weight);
                    if (Mathf.Abs(finger_angle) > angle_threshold)
                    {
                        // Debug.Log(finger_angle);
                        finger_state++;
                    }

                }

            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            hand_control();
            // pen이 있을때만
            if (pen_state == 1){
                if (finger_state > 4)
                {
                    if (pen_black.Find("Lead").gameObject.activeSelf == false){
                        pen_black.Find("Lead").gameObject.SetActive(true);
                        pen_black.Find("Body").gameObject.SetActive(true);
                        pen_black.Find("Tip").gameObject.SetActive(true);
                    }
                    
                    hand_shape = 1;     // ���� �ָ�
                }
                else
                {
                    if(pen_black.Find("Lead").gameObject.activeSelf == true){    
                        pen_black.Find("Lead").gameObject.SetActive(false);
                        pen_black.Find("Body").gameObject.SetActive(false);
                        pen_black.Find("Tip").gameObject.SetActive(false);
                    }
                    hand_shape = 0;
                }


            }
            
        }
        catch
        {
            Debug.Log("no hand");
        }

    }

}
