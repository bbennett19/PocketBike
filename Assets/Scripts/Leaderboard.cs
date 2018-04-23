using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BestTimeDataList
{
    public List<BestTimeData> times;
}

[Serializable]
public class BestTimeData
{
    public string PLAYER_id;
    public string name;
    public double time;
}


public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardLinePrefab;

    private void Awake()
    {
        StartCoroutine(HTTPRequestHandler.Instance.GetBestTimes(0, GetHighScoreCallback));
    }

    public void GetHighScoreCallback(bool networkError, bool success, string jsonData)
    {
        if(success)
        {
            Debug.Log("{times:"+jsonData+"}");
            BestTimeDataList highScores = JsonUtility.FromJson<BestTimeDataList>("{\"times\":" + jsonData + "}");

            int place = 1;
            foreach(BestTimeData data in highScores.times)
            {
                GameObject line = Instantiate(leaderboardLinePrefab, this.transform);
                line.GetComponent<LeaderboardElement>().SetFields(data.PLAYER_id, place++, data.name, data.time);
            }
        }
    }
}
