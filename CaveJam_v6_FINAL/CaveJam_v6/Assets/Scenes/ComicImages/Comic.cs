using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comic : MonoBehaviour
{
    public GameObject[] images;
    private int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {


            if (index > 0)
            {
                images[index].SetActive(false);
                index--;
            }

            images[index].SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {


            if (index < images.Length - 1)
            {
                images[index].SetActive(false);
                index++;
            }
            else
            {
                SceneManager.LoadScene("main");
            }

            images[index].SetActive(true);
        }
    }

}
