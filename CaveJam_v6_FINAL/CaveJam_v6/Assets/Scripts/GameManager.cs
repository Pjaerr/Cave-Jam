using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton = null;

    //External References
    public GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ammoStockpilePrefab;
    [HideInInspector] public System.Random rand;
    [HideInInspector] public enum EndGameCondition { Win, Loss };

    //Attributes
    public int waveNumber = 0;
    [HideInInspector] public int numberOfEnemies = 1;
    public int initialEnemies = 1;
    [HideInInspector] public int numberOfAmmoStockpiles = 0;

    //Time keeping stuff.
    [SerializeField] private float waveDowntime = 5.0f;
    [SerializeField] private int timeLimitMinutes = 1;
    [SerializeField] private int timeLimitSeconds = 60;

    private int previousTime;


    //Objects
    private GameObject[] enemies;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] ammoSpawnPoints;

    public AudioSource requiemForACaveMan;

    public AudioSource[] gruntSounds;
    public AudioSource[] belchSounds;
    public AudioSource footTapSound;
    public AudioSource throwGruntSound;
    public AudioSource cavemanDeathSound;
    public AudioSource[] rockCollectingSounds;
    public AudioSource robotDeathSound;


    //UI
    [SerializeField] private GameObject popupUIObject;

    void Awake()
    {
        if (singleton == null)   //Check if an instance of GameManager already exists.
        {
            singleton = this;    //If not, make this that instance.
        }
        else if (singleton != this)  //If an instance already exists and it isn't this.
        {
            Destroy(gameObject);    //Destroy this.
        }
    }

    void FixedUpdate()
    {
        int currentTime = (int)Time.fixedTime;
        int timeTaken = currentTime - previousTime;
        previousTime = currentTime;

        if (timeTaken >= 1)
        {
            timeLimitSeconds--;

            if (timeLimitMinutes <= 0 && timeLimitSeconds <= 0)
            {
                endGame(GameManager.EndGameCondition.Win);
            }

            if (timeLimitSeconds <= 0)
            {
                timeLimitMinutes--;
                timeLimitSeconds = 60;
            }



            Debug.Log(timeLimitMinutes + ":" + timeLimitSeconds);
        }
    }

    void Start()
    {
        numberOfAmmoStockpiles = 0;
        rand = new System.Random();

        newWave(initialEnemies);

        previousTime = (int)Time.fixedTime;
    }

    public void newWave(int numOfEnemies)
    {
        if (waveNumber == 0)
        {
            popupUIObject.SetActive(true);
        }

        numberOfEnemies = numOfEnemies;


        if (numberOfAmmoStockpiles < 2)
        {
            //Instantiate two ammo stockpiles when the other two have run out.
            //Ensure that the two positions aren't the same.

            int randomIndexA = rand.Next(0, ammoSpawnPoints.Length);
            Instantiate(ammoStockpilePrefab, ammoSpawnPoints[randomIndexA].position, Quaternion.identity);

            if (numberOfAmmoStockpiles < 1)
            {

                int randomIndexB = rand.Next(0, ammoSpawnPoints.Length);

                while (randomIndexB == randomIndexA)
                {
                    randomIndexB = rand.Next(0, ammoSpawnPoints.Length);
                }

                Instantiate(ammoStockpilePrefab, ammoSpawnPoints[randomIndexB].position, Quaternion.identity);

            }
            numberOfAmmoStockpiles = 2;
        }

        StartCoroutine(waitBeforeWave());
    }

    IEnumerator waitBeforeWave()
    {
        yield return new WaitForSeconds(waveDowntime);

        popupUIObject.SetActive(false);

        if (numberOfEnemies > 18)
        {
            numberOfEnemies = 18;
        }

        waveNumber++;

        enemies = new GameObject[numberOfEnemies];

        for (int i = 0; i < enemies.Length; i++)
        {
            int randomIndex = rand.Next(0, enemySpawnPoints.Length);
            enemies[i] = Instantiate(enemyPrefab, enemySpawnPoints[randomIndex].position, Quaternion.Euler(0, 45, 0));
        }
    }


    public void endGame(EndGameCondition condition)
    {
        if (condition == EndGameCondition.Win)
        {
            SceneManager.LoadScene("endSceneWin");
        }
        else if (condition == EndGameCondition.Loss)
        {
            //Switch to scene for losing here.
            SceneManager.LoadScene("endSceneLoss");
        }
    }
}
