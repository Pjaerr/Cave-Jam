using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comic : MonoBehaviour
{
    public GameObject[] images;
    private int index = 0;

    void Start()
    {
        StartCoroutine(switchImage());
    }


    IEnumerator switchImage()
    {
        yield return new WaitForSeconds(5.0f);


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
        StartCoroutine(switchImage());
    }

}
