using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ChangeInGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void ChangeEnd()
    {
        SceneManager.LoadScene("End");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
