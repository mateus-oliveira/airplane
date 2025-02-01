using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Text scoreText;
    private static string currentScene;
    
    public int points { get; private set; }

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
        currentScene = "Menu";
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
    }

    public void Restart()
    {
        points = 0;
        this.LoadScoreText();
    }

    public static void LoadScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);
        Debug.Log("Current scene: " + currentScene);
        if (scene == "Restart") {
            SceneManager.LoadScene(currentScene);
        } else {
            currentScene = scene;
            SceneManager.LoadScene(scene);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
