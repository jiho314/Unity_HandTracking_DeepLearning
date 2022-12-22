using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start(){
        DontDestroyOnLoad(GameObject.Find("GameManager").gameObject);

    }
    public void Play()
    {
        SceneManager.LoadScene("Room");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }
}
