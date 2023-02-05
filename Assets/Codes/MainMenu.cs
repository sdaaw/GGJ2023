using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void QuitButton()
    {
        Application.Quit();
    }


    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Play2()
    {
        SceneManager.LoadScene("OrtoScene");
    }
}
