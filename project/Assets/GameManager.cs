using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int state;
    public int hand_state;
    public Dictionary<string, GameObject> object_manager;
    public Dictionary<string, int> player_object;
    void Start()
    {

        object_manager = new Dictionary<string, GameObject>(){

            {"eraser",  transform.Find("eraser").gameObject},
            {"ballpoint_pen_black", transform.Find("ballpoint_pen_black").gameObject}
        };
        player_object = new Dictionary<string, int>(){
            {"eraser",0},
            {"ballpoint_pen_black",0}
        };

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
