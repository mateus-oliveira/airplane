using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Text scoreText;
    [SerializeField] private Sprite roadNight;
    [SerializeField] private Sprite roadMorning;
    [SerializeField] private Sprite landingSiteNight;
    [SerializeField] private Sprite landingSiteMorning;


    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameController");
                    _instance = singleton.AddComponent<GameController>();
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

    public int points { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void Start() {
        this.Restart();
    }

    private void LoadScoreText()
    {
        GameObject text = GameObject.FindGameObjectWithTag("ScoreText");
        this.scoreText = text.GetComponent<Text>();
    }

    public void AddPoints(int points)
    {
        this.points += points;
        scoreText.text = "x " + this.points;
        Debug.Log("Pontos: " + this.points);
    }

    public void Restart()
    {
        points = 0;
        this.LoadScoreText();
        this.SetIsMorning();
    }

    private void SetIsMorning()
    {
        // Randomly decide if it's morning or night
        bool isMorning = Random.value > 0.5f;

        GameObject road = GameObject.FindGameObjectWithTag("Road");
        GameObject landingSite = GameObject.FindGameObjectWithTag("LandingSite");

        if (isMorning)
        {
            road.GetComponent<SpriteRenderer>().sprite = roadMorning;
            landingSite.GetComponent<SpriteRenderer>().sprite = landingSiteMorning;
        }
        else
        {
            road.GetComponent<SpriteRenderer>().sprite = roadNight;
            landingSite.GetComponent<SpriteRenderer>().sprite = landingSiteNight;
        }
    }
}
