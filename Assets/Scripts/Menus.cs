using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void Salir()
    {
        Application.Quit();
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void GameStartMonaco()
    {
        SceneManager.LoadScene("GamePlayMonaco");
    }
    public void BorraRecord()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
