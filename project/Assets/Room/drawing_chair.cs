using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class drawing_chair : MonoBehaviour
{
    // Start is called before the first frame update

    
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        GameObject GameManager = GameObject.Find("GameManager"); 
        if (GameManager.GetComponent<GameManager>().player_object["ballpoint_pen_black"] == 1){
            load_drawing_scene();
            DontDestroyOnLoad(GameObject.Find("GameManager"));
        }
        else{
            
            GameObject.Find("Canvas").transform.Find("warning").gameObject.SetActive(true);      
            Invoke("warning_end", 2f);  
            // 펜이 없습니다!!
            Invoke("load_drawing_scene", 2f);
        }
        
    }

    void warning_end () {

        GameObject.Find("Canvas").transform.Find("warning").gameObject.SetActive(false);  
    }

    void load_drawing_scene (){
        SceneManager.LoadScene("Drawing");
        
    }
}
