using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuScript : MonoBehaviour
{

    private bool bIsPaused; //Is the game currently paused?

    public GameObject Menu;

    // Use this for initialization
    void Start()
    {
        bIsPaused = false;
        Time.timeScale = 0;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("escape") == true & this.isActiveAndEnabled == true)
        { //Calls the pause toggle when escape is pressed and then released           
            TogglePaused();
        }
    }

    public void StartGame() //Start the game for the first time
    {
        Debug.Log("Game Starting");
        this.enabled = true;
        Time.timeScale = 1;

        GameManager.singleton.requiemForACaveMan.Stop();
        GameManager.singleton.requiemForACaveMan.Play();
    }

    public void CloseGame() //Closes the game
    {
        Debug.Log("Quit button pressed: Game closing");
        Application.Quit();
    }

    public void TogglePaused()
    { //Toggles the paused boolean
        if (bIsPaused == true)
        {
            GameManager.singleton.requiemForACaveMan.UnPause();
            bIsPaused = false;
        }
        else
        {
            GameManager.singleton.requiemForACaveMan.Pause();
            bIsPaused = true;
        }

        PauseGame(bIsPaused);
        Debug.Log("Game Paused: " + "Is Paused: " + bIsPaused);
    }

    void PauseGame(bool bPaused)
    { //The function that will disable the game while it is paused

        if (bPaused == true)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            Menu.SetActive(false);
        }
    }
}
