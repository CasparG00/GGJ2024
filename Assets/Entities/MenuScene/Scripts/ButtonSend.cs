using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSend : MonoBehaviour
{
    public string sceneName;

    public void sceneChange()
    {
        if (sceneName == "Start")
            SceneManager.LoadScene(1);
        else
            Application.Quit();

    }
}
