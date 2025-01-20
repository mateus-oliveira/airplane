using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Text scoreText;
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
        this.ResetPoints();
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

    public void ResetPoints()
    {
        points = 0;
        this.LoadScoreText();
    }
}
