using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private static Score _instance;

    public static Score Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Score>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("Score");
                    _instance = singleton.AddComponent<Score>();
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

    public int points { get; private set; }
    
    void Start() {
        this.LoadScoreText();
    }

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

    private void LoadScoreText() {
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
