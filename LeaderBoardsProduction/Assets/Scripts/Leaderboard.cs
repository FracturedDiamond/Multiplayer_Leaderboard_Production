using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static Leaderboard instance;
    
    void Awake() { 
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(false);
        DisplayLeaderboard();     
    }

    public void DisplayLeaderboard()
    {
        Debug.Log("Display Leaderboard!");
        
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 10
        };

        Debug.Log("After Get Leaderboard Request");

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
        );
    }

    public void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        Debug.Log("Update Leaderboard UI");
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            Debug.Log("x = " + x);
            Debug.Log("Leaderboard.count = " + leaderboard.Count);
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            
            if (x >= leaderboard.Count) continue; 
            
            leaderboardEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = 
                (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            
            leaderboardEntries[x].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text =
                (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");

        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
        Debug.Log("Set Leaderboard Entry: " + newScore);
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighScore",
            FunctionParameter = new { score = newScore }
        };
        
        PlayFabClientAPI.ExecuteCloudScript(request,
         result => DisplayLeaderboard(),
         error => Debug.Log(error.ErrorMessage)
        );

        Debug.Log("End of Set Leaderboard Entry");
    }


}
