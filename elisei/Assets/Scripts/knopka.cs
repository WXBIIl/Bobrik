using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class knopka : MonoBehaviour
{
    public void Transtion()
    {
        SceneManager.LoadScene("Round");
        PlayerPrefs.DeleteKey("TotalApples");

    }
    public void Vihod()
    {
        Application.Quit();
    }
}
