using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    #region Fields
    //OBJECTS PREFABS
    [SerializeField]
    public List<GameObject> objectsPrefabs = new List<GameObject>();
    public List<GameObject> objectsPrefabsWith2Bombs = new List<GameObject>();
    public List<GameObject> objectsPrefabsWith5Bombs = new List<GameObject>();
    
    //OBJECTS TO INSTANTIATE
    [SerializeField]
    public List<GameObject> objectsToInstantiate = new List<GameObject>();
    public List<GameObject> objetosCon2Bombas = new List<GameObject>();
    public List<GameObject> objectosCon5Bombas = new List<GameObject>();
    // INSTANTIATED OBJECTS POSITIONS
    public Vector3[] instantiatedObjectsPositions;
    //OTHERS
    public Camera currentCamera;
    public float timeRemaining = 30f;
    public Text remainingTimeText;
    public Text scoreText;
    public Text highScoreText;
    public Text gameOverScoreText;
    public int score;
    public int scoreIncrement = 1;
    public float scoreMultiplier = 1f;
    [SerializeField]
    public float multiplierDuration = 2f;
    bool isGameOver = false;
    public GameObject gameOverPanel;
    public AudioSource gameOverSound;
    public AudioClip gameoverSound;
    public GameObject bronzeIcon;
    public GameObject goldIcon;
    public GameObject silverIcon;
 
    bool isAdShowed = false;
    #endregion
    #region Functions
    private void Awake()
    {
       
        SpawnObjects();
        StartCoroutine(StartCountdown());
        highScoreText.text = "Record : " + PlayerPrefs.GetInt("BestScore", 0).ToString();
       
        
    }
    private void Start()
    {
        
        //this.RequestIntersititial();
        isGameOver = false;
        gameOverSound.clip = gameoverSound;
        PlayerManager.Instance.musicOn.SetActive(true);
        PlayerManager.Instance.soundOn.SetActive(true);

    }
    public void Update()
    {
        if (timeRemaining<=0)
        {
            isGameOver = true;
            GameOver();
        }
        ChangeScoreText();
        ChangeTimeText();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (isGameOver == false)
            {
                if (hit.collider != null && objectsToInstantiate.Contains(hit.collider.gameObject) || objetosCon2Bombas.Contains(hit.collider.gameObject) || objectosCon5Bombas.Contains(hit.collider.gameObject)) 
                {
                    if (hit.collider.CompareTag("Objects"))
                    {
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        
                        score += (int)(scoreIncrement * scoreMultiplier);
                        PlayerManager.Instance.coinCount += (int)(score * scoreMultiplier);


                        Destroy(hit.collider.gameObject);
                    }
                    if (hit.collider.CompareTag("TimePowerUp"))
                    {
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        timeRemaining += 1;
                        Destroy(hit.collider.gameObject);
                    }
                    if (hit.collider.CompareTag("ScoreMultiplier"))
                    {
                        scoreMultiplier *= 2f;
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                        StartCoroutine(DecrementMultiplierDuration());
                    }
                    if (hit.collider.CompareTag("Bomb"))
                    {
                        timeRemaining = 0;
                        isGameOver = true;
                        if (score > PlayerPrefs.GetInt("BestScore", 0))
                        {
                            PlayerPrefs.SetInt("BestScore", score);
                            highScoreText.text = "Record : " + score.ToString();
                        }
                        GameOver();
                    }
                    if (objectsToInstantiate.Count <= 14)
                    {
                        foreach (GameObject obj in objectsToInstantiate)
                        {
                            Destroy(obj);
                        }
                        objectsToInstantiate.Clear();
                        foreach (GameObject obj in objetosCon2Bombas)
                        {
                            Destroy(obj);
                        }
                        objetosCon2Bombas.Clear();
                        foreach (GameObject obj in objectosCon5Bombas)
                        {
                            Destroy(obj);
                        }
                        objectosCon5Bombas.Clear();
                        if (score<=10)
                        {
                            SpawnObjects();
                        }
                        else if (score<=25)
                        {
                            SpawnObjectsWith2Bombs();
                        }
                        else if(score>=26)
                        {
                            SpawnObjectsWith5Bombs();
                        }
                        
                    }
                }
            }
        }
    }
    public void SpawnObjects()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objectsToInstantiate.Add(createdObject);
        }
    }
    public void SpawnObjectsWith2Bombs()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabsWith2Bombs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objetosCon2Bombas.Add(createdObject);
        }
    }
    public void SpawnObjectsWith5Bombs()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabsWith5Bombs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objectosCon5Bombas.Add(createdObject);
        }
    }
    public void ChangeScoreText()
    {
        scoreText.text = "Puntos : " + score.ToString();
        gameOverScoreText.text = "Puntos : " + score.ToString();
    }
    private void ChangeTimeText()
    {
        remainingTimeText.text = "Tiempo restante : " + timeRemaining.ToString();
    }
    IEnumerator StartCountdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
    }
    IEnumerator DecrementMultiplierDuration()
    {
        yield return new WaitForSeconds(multiplierDuration);
        scoreMultiplier /= 2f;
    }

    [System.Obsolete]
    private void GameOver()
    {
        gameOverSound.PlayOneShot(gameoverSound);
        if (isGameOver == true)
        {
            gameOverSound.Play();
            PlayerPrefs.SetInt("coinCount", PlayerManager.Instance.coinCount);
            foreach (GameObject gameobJect in objectsToInstantiate)
            {
                Destroy(gameobJect);
            }
            foreach (GameObject gameObject in objetosCon2Bombas)
            {
                Destroy(gameObject);
            }
            foreach (GameObject gameObject1 in objectosCon5Bombas)
            {
                Destroy(gameObject1);
            }
            remainingTimeText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            if (score <= 10)
            {
                bronzeIcon.SetActive(true);
            }
            else if (score <= 50){
                silverIcon.SetActive(true);
            }
            else
            {
                goldIcon.SetActive(true);
            }
            
        }
    }
   
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RequestIntersititial()
    {
#if UNITY_ANDROID
        
#elif UNITY_IPHONE
  
#else
string adID = "unexpected_platform";
#endif
        
    }
    #endregion
    public void BorraRecord()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }
    public void GameStartMonaco()
    {
        SceneManager.LoadScene("GamePlayMonaco");
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GamePlay");
    }

}
