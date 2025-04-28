using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class LeaderboardEntry
{
    public string nickname;
    public int score;

    public LeaderboardEntry(string nickname, int score)
    {
        this.nickname = nickname;
        this.score = score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    private const string LeaderboardKey = "Leaderboard";
    public static LeaderboardManager Instance { get; private set; }

    private List<LeaderboardEntry> leaderboard;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLeaderboard();
    }

    private void LoadLeaderboard()
    {
        leaderboard = new List<LeaderboardEntry>();
        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            string json = PlayerPrefs.GetString(LeaderboardKey);
            LeaderboardEntry[] entries = JsonUtility.FromJson<LeaderboardWrapper>(json).entries;
            leaderboard = entries.ToList();
        }
    }

    private void SaveLeaderboard()
    {
        LeaderboardWrapper wrapper = new LeaderboardWrapper { entries = leaderboard.ToArray() };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    public void UpdateLeaderboard(string nickname, int score)
    {
        var existingEntry = leaderboard.Find(entry => entry.nickname == nickname);
        if (existingEntry != null)
        {
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
            }
        }
        else
        {
            leaderboard.Add(new LeaderboardEntry(nickname, score));
        }

        leaderboard = leaderboard.OrderByDescending(entry => entry.score).Take(10).ToList();
        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboard;
    }

    [System.Serializable]
    private class LeaderboardWrapper
    {
        public LeaderboardEntry[] entries;
    }
}