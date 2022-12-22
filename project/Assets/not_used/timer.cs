// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System;

// public class timer : MonoBehaviour
// {
//     public float setTime = 60.0f;
//     [SerializeField] Text countdownText;
//     public GameObject canvas;

//     void Start()
//     {
//         countdownText.text = setTime.ToString();
//         canvas = transform.parent.transform.parent.gameObject;
//     }

//     private void Update()
//     {
//         if (setTime > 0)
//             setTime -= Time.deltaTime;
//         else if (setTime <= 0)
//         {
//             canvas.GetComponent<manager_game>().timeoutf();
//             Time.timeScale = 0.0f;
//             setTime = 60.0f;
//         }

//         countdownText.text = Mathf.Round(setTime).ToString();
//     }

//     public void resetTime()
//     {
//         setTime = 60.0f;
//     }
// }