using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class door_exit : MonoBehaviour
{

    void OnTriggerEnter(Collider other){
        SceneManager.LoadScene("Title");
        
    }
}
