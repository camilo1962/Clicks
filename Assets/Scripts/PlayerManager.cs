using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Fields
    public int coinCount;
    public float multiplierDuration = 2f;
    private static PlayerManager _instance;
    public AudioSource AudioSource;
    public AudioSource sound;
    public AudioClip soundMenu;
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject soundOn;
    public GameObject soundOff;


    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();
            }

            //FindObjectOfType<PlayerManager>();
            return _instance;
        }
    }

    #endregion
    #region Functions
    private void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        coinCount = PlayerPrefs.GetInt("coinCount", 0);
    }
    public void Start()
    {
        sound.clip = soundMenu;
    }
    #endregion
    public void MusicOn()
    {
        AudioSource.Play();
        musicOff.SetActive(false);
        musicOn.SetActive(true);
    }
    public void MusicOff()
    {
        AudioSource.Stop();
        musicOn.SetActive(false);
        musicOff.SetActive(true);
    }

    public void SoundOn()
    {
        AudioSource.Play();
        soundOff.SetActive(false);
        soundOn.SetActive(true);
    }
    public void SoundOff()
    {
        AudioSource.Stop();
        soundOn.SetActive(false);
        soundOff.SetActive(true);
    }
    public void SoundButton()
    {
        sound.Play();

        //sound.enabled = false;
        //sound.enabled = true;
    }
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


}


