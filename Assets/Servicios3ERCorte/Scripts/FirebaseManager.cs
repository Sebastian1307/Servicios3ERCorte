using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using UnityEngine;

/// <summary>
/// Maneja todo lo relacionado con Firebase Firestore (guardar y leer puntajes).
/// SIN AUTH, SIN ANALYTICS.
/// </summary>
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    private FirebaseFirestore db;
    private bool initialized = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitializeFirebase();
    }

    private async void InitializeFirebase()
    {
        var dep = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dep == DependencyStatus.Available)
        {
            db = FirebaseFirestore.DefaultInstance;
            initialized = true;
            Debug.Log("🔥 Firestore inicializado correctamente.");
        }
        else
        {
            Debug.LogError("❌ Error inicializando Firebase: " + dep);
        }
    }

    [Serializable]
    public class HighscoreEntry
    {
        public string id;
        public string name;
        public int score;
        public double timePlayed;
        public int attempts;
        public int objectsCaught;
        public int objectsMissed;
        public DateTime timestamp;
    }

    public async Task SaveHighscore(string playerName, int score, double timePlayed, int attempts, int caught, int missed)
    {
        if (!initialized)
        {
            Debug.LogError("Firestore no está inicializado aún.");
            return;
        }

        try
        {
            var data = new Dictionary<string, object>
            {
                { "name", playerName },
                { "score", score },
                { "timePlayed", timePlayed },
                { "attempts", attempts },
                { "objectsCaught", caught },
                { "objectsMissed", missed },
                { "timestamp", Timestamp.GetCurrentTimestamp() }
            };

            await db.Collection("Highscores").AddAsync(data);
            Debug.Log($"✅ Highscore guardado: {playerName} - {score}");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error guardando highscore: " + ex.Message);
        }
    }

    public async Task<List<HighscoreEntry>> GetTopHighscores(int limit = 10)
    {
        var list = new List<HighscoreEntry>();

        if (!initialized)
        {
            Debug.LogError("Firestore no está inicializado aún.");
            return list;
        }

        try
        {
            var query = db.Collection("Highscores").OrderByDescending("score").Limit(limit);
            var snap = await query.GetSnapshotAsync();

            foreach (var doc in snap.Documents)
            {
                var entry = new HighscoreEntry
                {
                    id = doc.Id,
                    name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "Player",
                    score = doc.ContainsField("score") ? doc.GetValue<int>("score") : 0,
                    timePlayed = doc.ContainsField("timePlayed") ? doc.GetValue<double>("timePlayed") : 0,
                    attempts = doc.ContainsField("attempts") ? doc.GetValue<int>("attempts") : 0,
                    objectsCaught = doc.ContainsField("objectsCaught") ? doc.GetValue<int>("objectsCaught") : 0,
                    objectsMissed = doc.ContainsField("objectsMissed") ? doc.GetValue<int>("objectsMissed") : 0,
                    timestamp = doc.ContainsField("timestamp") ? doc.GetValue<Timestamp>("timestamp").ToDateTime() : DateTime.MinValue
                };
                list.Add(entry);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error obteniendo highscores: " + ex.Message);
        }

        return list;
    }
}
