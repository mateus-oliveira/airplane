using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour {
    [SerializeField] private Text score;

    void Start() {
        score.text = "Score: " + GameController.Instance.points;
    }
}
