using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw2 : MonoBehaviour
{
    //public Hand_Control hand;
    public int clear_trigger;
    public TrailRenderer trail;
    private hand Hand_Control;
    // Start is called before the first frame update

    void Start()
    {
        trail = transform.GetComponent<TrailRenderer>();
        Hand_Control = GameObject.Find("Custom Right Hand Model with Collider").GetComponent<hand>();
        clear_trigger = 0;
        trail.emitting = false;
    }

    // Update is called once per frame
    void LateUpdate()
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