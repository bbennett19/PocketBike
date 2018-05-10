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

public class NetworkOperations : MonoBehaviour
{
    public delegate void UpdateCompleteCallback(bool success);
    public delegate void GetHighScoresCallback(bool success, List<BestTimeData> data);

    public static NetworkOperations Instance;
    private HTTPRequestHandler _httpHandler = new HTTPRequestHandler();

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Update Player Data

    // Update the player data
    // Steps to update player data:
    // 1. Make sure the player data has been created in the database
    // 2. Make sure all player data is up to date in the database
    // 3. Update all fastest times
    public void UpdatePlayerData(UpdateCompleteCallback callback, bool addPoints = false, int subtractPoints = 0)
    {
        Debug.Log("UpdatePlayer");
        // If the player data has not been created it needs to be created before doing the update
        if(!PlayerPointsAndItems.Instance.playerData.PlayerDataHasBeenCreated)
        {
            HTTPRequestHandler.BasicHTTPDelegate del = (n, s) => PlayerCreateNeedUpdateCallback(callback, addPoints, subtractPoints, n, s);
            StartCoroutine(_httpHandler.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, PlayerPointsAndItems.Instance.playerData.Name, del));
        }
        else if (PlayerPointsAndItems.Instance.playerData.PlayerDataToUpload || addPoints || subtractPoints != 0)
        {
            int points = PlayerPointsAndItems.Instance.playerData.Points;
            double dist = PlayerPointsAndItems.Instance.playerData.DistanceTraveled;

            if (addPoints)
            {
                points = PlayerPointsAndItems.Instance.playerData.GetTotalPoints();
                dist = PlayerPointsAndItems.Instance.playerData.GetTotalDistance();
            }
            if(subtractPoints != 0)
            {
                points -= subtractPoints;
            }

            HTTPRequestHandler.BasicHTTPDelegate del = (n, s) => PlayerUpdateCallback(callback, n, s);
            StartCoroutine(_httpHandler.UpdatePlayerData(SystemInfo.deviceUniqueIdentifier, PlayerPointsAndItems.Instance.playerData.Name,
                points, dist, del));
        }
        else if(PlayerPointsAndItems.Instance.playerData.RequiresBestTimeUpdate())
        {
            int levelID = PlayerPointsAndItems.Instance.playerData.GetLevelIDToUpdate();
            float time = PlayerPointsAndItems.Instance.playerData.bestTimes[levelID];
            HTTPRequestHandler.BasicHTTPDelegate del = (n, s) => PlayerUpdateTimeCallback(callback, levelID, n, s);
            StartCoroutine(_httpHandler.UpdatePlayerBestTime(SystemInfo.deviceUniqueIdentifier, levelID.ToString(), time, del));
        }
        else
        {
            callback(true);
        }
    }

    // Player has attempted to be created. Still need a player update
    private void PlayerCreateNeedUpdateCallback(UpdateCompleteCallback callback, bool updatePoints, int subtractPoints, bool networkError, bool success)
    {
        if (!networkError)
        {
            // If it was successfull that means the player data has been created in the database
            // If it was not successfull that means the player data was already in the database
            // Either way the player data has been created
            PlayerPointsAndItems.Instance.playerData.PlayerDataHasBeenCreated = true;
            UpdatePlayerData(callback, updatePoints, subtractPoints);
        }
        else
        {
            callback(false);
        }
    }

    // Player data has attempted to be updated
    private void PlayerUpdateCallback(UpdateCompleteCallback callback, bool networkError, bool success)
    {
        if (!networkError && success)
        {
            PlayerPointsAndItems.Instance.playerData.PlayerDataToUpload = false;
            UpdatePlayerData(callback);
        }
        else
        {
            callback(false);
        }
    }

    // Player best time has attempted to be updated
    private void PlayerUpdateTimeCallback(UpdateCompleteCallback callback, int levelID, bool networkError, bool success)
    {
        if (!networkError && success)
        {
            PlayerPointsAndItems.Instance.playerData.bestTimeDataToUpload[levelID] = false;
            UpdatePlayerData(callback);
        }
        else
        {
            callback(false);
        }
    }

    #endregion

    #region Get High Scores

    // Get high score list for a single level
    public void GetHighScores(GetHighScoresCallback callback, int levelID)
    {
        // First make sure all player data is up to date on the server
        UpdateCompleteCallback cb = (s) => GetLeaderboardPlayerUpdateComplete(callback, levelID, s);
        UpdatePlayerData(cb);
    }

    private void GetLeaderboardPlayerUpdateComplete(GetHighScoresCallback callback, int levelID, bool success)
    {
        if(success)
        {
            HTTPRequestHandler.ReturnDataHTTPDelegate del = (e, s, res) => GetHighScoreResultCallback(callback, e, s, res);
            StartCoroutine(_httpHandler.GetBestTimes(levelID, del));
        }
        else
        {
            callback(false, null);
        }
    }

    private void GetHighScoreResultCallback(GetHighScoresCallback callback, bool networkError, bool success, string jsonData)
    {
        if (success)
        {
            Debug.Log("{times:" + jsonData + "}");
            BestTimeDataList highScores = JsonUtility.FromJson<BestTimeDataList>("{\"times\":" + jsonData + "}");
            callback(true, highScores.times);
        }
        else
        {
            callback(false, null);
        }
    }

    #endregion







}
