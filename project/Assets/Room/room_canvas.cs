using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_canvas : MonoBehaviour
{

    GameManager GameManager;
    private GameObject pen_img;
    private GameObject eraser_img;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pen_img = transform.GetChild(0).Find("pen_canvas").Find("pen").gameObject;
        eraser_img = transform.GetChild(0).Find("eraser_canvas").Find("eraser").gameObject;        

        
    }

    // Update is called once per frame
    void Update()
    {
        // Inventory
        // false -> pen image active
        if (GameManager.player_object["ballpoint_pen_black"] == 1 & pen_img.activeSelf == false){
            pen_img.SetActive(true);

        }
        if (GameManager.player_object["eraser"] == 1 & eraser_img.activeSelf == false){
            eraser_img.SetActive(true);

        }
    }
}
