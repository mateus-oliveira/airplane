using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text score;

    void Start() {
        score.text = "Score: " + Counter.Instance.Points.ToString();
    }
}
