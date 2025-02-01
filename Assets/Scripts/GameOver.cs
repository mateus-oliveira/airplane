using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameOver : MonoBehaviour {
    [SerializeField] private Text score;
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Button restartButton, selectMapButton, menuButton, saveButton;

    void Start() {
        restartButton.gameObject.SetActive(false);
        selectMapButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        score.text = "Score: " + GameController.Instance.points;

        if (GameController.Instance.points == 0) {
            this.DisplayButtons();
        }
    }

    public void OnSaveButtonClicked() {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName)) {
            this.SaveScore(playerName);
        }
        this.DisplayButtons();
    }

    private void DisplayButtons() {
        saveButton.gameObject.SetActive(false);
        playerNameInput.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        selectMapButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    void SaveScore(string playerName) {
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/highscores.csv";
        using (StreamWriter writer = new StreamWriter(path, true)) {
            writer.WriteLine(playerName + "," + GameController.Instance.points);
        }
    }
}