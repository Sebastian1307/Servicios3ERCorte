using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Actualiza HUD y controla paneles Start/HUD/End. Rellena ScrollView con entradas de score.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject hudPanel;
    public GameObject endPanel;

    [Header("HUD")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    [Header("Start")]
    public TMP_InputField nameInput;

    [Header("End / Leaderboard")]
    public Transform leaderboardContent; // content del ScrollView
    public GameObject scoreEntryPrefab; // prefab con dos TMP: name & score
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        ShowStart();
    }

    public void OnStartButtonPressed()
    {
        string playerName = string.IsNullOrWhiteSpace(nameInput.text) ? "Player" : nameInput.text;
        GameManager.Instance.StartGame(playerName);
    }

    public void ShowStart()
    {
        startPanel.SetActive(true);
        hudPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void ShowHUD()
    {
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
        endPanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        startPanel.SetActive(false);
        hudPanel.SetActive(false);
        endPanel.SetActive(true);
        finalScoreText.text = $"Score: {Mathf.FloorToInt(GameManager.Instance.Score)}";
    }

    public void UpdateTime(float t)
    {
        timeText.text = $"Time: {t:0.0}s";
    }

    public void UpdateScore(int s)
    {
        scoreText.text = $"Score: {s}";
    }

    public void PopulateLeaderboard(List<FirebaseManager.HighscoreEntry> entries)
    {
        // Limpiar
        foreach (Transform child in leaderboardContent) Destroy(child.gameObject);

        // Crear entradas ordenadas ya desde Firebase (desc)
        foreach (var e in entries)
        {
            GameObject go = Instantiate(scoreEntryPrefab, leaderboardContent);
            var nameT = go.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            var scoreT = go.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            nameT.text = e.name;
            scoreT.text = e.score.ToString();
        }
    }
}
