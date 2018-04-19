using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHUDNEW : MonoBehaviour
{

    const int barSize = 9;

    private Player playerScript;

    public UnityEngine.UI.Text rockText;
    public GameObject[] healthHearts;

    // Use this for initialization
    void Start()
    {
        playerScript = GameManager.singleton.player.GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        for (int i = 0; i < barSize; i++)
        {
            if (playerScript.playerLives > i)
            {
                healthHearts[i].SetActive(true);
            }
            else
            {
                healthHearts[i].SetActive(false);
            }

            rockText.text = "x" + playerScript.ammo.ToString();
            // Debug.Log("HUD Updated");
        }



    }
}
