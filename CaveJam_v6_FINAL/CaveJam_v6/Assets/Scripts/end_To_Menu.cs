using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end_To_Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void End_To_Menu()
    {
        SceneManager.LoadScene("main");
    }

    public void End_To_Game()
    {
        SceneManager.LoadScene("main");
    }
}
