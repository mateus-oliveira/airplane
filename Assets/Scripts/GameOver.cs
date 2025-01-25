using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour {
    [SerializeField] private Text score;

    void Start() {
        score.text = "Score: " + GameController.Instance.points;
    }

    public void Restart() {
        //TODO: random SceneManager.LoadScene("MorningScene");
        SceneManager.LoadScene("NightScene");
    }
}
