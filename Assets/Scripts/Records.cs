using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Records : MonoBehaviour {
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject scorePrefab;

    void Start() {
        List<KeyValuePair<string, int>> highScores = LoadHighScores();
        DisplayHighScores(highScores);
    }

    List<KeyValuePair<string, int>> LoadHighScores() {
        string path = Application.persistentDataPath + "/highscores.csv";
        List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();

        if (File.Exists(path)) {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines) {
                string[] parts = line.Split(',');
                if (parts.Length == 2) {
                    string playerName = parts[0];
                    int points;
                    if (int.TryParse(parts[1], out points)) {
                        scores.Add(new KeyValuePair<string, int>(playerName, points));
                    }
                }
            }
        }

        return scores.OrderByDescending(x => x.Value).Take(10).ToList();
    }

    void DisplayHighScores(List<KeyValuePair<string, int>> highScores) {
        float yOffset = 150f;
        float yIncrement = 30f;

        foreach (var score in highScores) {
            GameObject scoreEntry = Instantiate(scorePrefab, content.transform);
            RectTransform rectTransform = scoreEntry.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yOffset);
            Text scoreText = scoreEntry.GetComponent<Text>();
            scoreText.text = score.Key + ": " + score.Value;
            yOffset -= yIncrement;
        }
    }
}