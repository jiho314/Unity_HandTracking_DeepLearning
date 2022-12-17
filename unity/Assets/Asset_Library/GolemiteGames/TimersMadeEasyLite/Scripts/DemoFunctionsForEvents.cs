using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DemoFunctionsForEvents : MonoBehaviour
{
    public GameObject finishedText;

    public Timer timer;
    public void TimerStart()
    {
        finishedText.SetActive(false);
    }
    public void TimerEnd()
    {
        finishedText.SetActive(true);
    }
}
