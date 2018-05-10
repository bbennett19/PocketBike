using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardLinePrefab;
    private int _currentLevelID;

    private void ChangeLeaderboard(int levelID)
    {
        if(_currentLevelID != levelID)
        {
            _currentLevelID = levelID;
            NetworkOperations.Instance.GetHighScores(GetHighScoreCallback, levelID);
        }
    }

    public void GetHighScoreCallback(bool success, List<BestTimeData> data)
    {
        if(success)
        {
            int place = 1;
            foreach(BestTimeData item in data)
            {
                GameObject line = Instantiate(leaderboardLinePrefab, this.transform);
                line.GetComponent<LeaderboardElement>().SetFields(item.PLAYER_id, place++, item.name, item.time);
            }
        }
        else
        {
            // Display network error
        }
    }
}
