using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw : MonoBehaviour
{
    //public Hand_Control hand;
    public int clear_trigger;
    public TrailRenderer trail;
    private Hand_Control Hand_Control;
    // Start is called before the first frame update
    
    void Start()
    {
        Hand_Control = GameObject.Find("Custom Right Hand Model with Collider").GetComponent<Hand_Control>();
        clear_trigger = 0;
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Hand_Control.hand_shape == 1)
        {
            
            trail.emitting = true;
        }

        else if (Hand_Control.hand_shape == 0)
        {
            trail.emitting = false;
        }

        if (clear_trigger == 1)
        {
            trail.Clear();
        }
    }
}