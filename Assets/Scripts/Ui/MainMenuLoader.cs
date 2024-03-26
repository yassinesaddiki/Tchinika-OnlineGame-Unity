using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Scenes/Home");
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Scenes/Auth/LoginS");
    }
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("Scenes/Auth/RegisterS");
    }
}
