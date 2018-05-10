using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HTTPRequestHandler
{ 
    private const String HTTP_REQUEST_EMPTY = "[]";
    private string url = "http://ix-dev.cs.uoregon.edu:12355";
    // Callback delegate functions
    public delegate void ReturnDataHTTPDelegate(bool networkError, bool success, string jsonString);
    public delegate void BasicHTTPDelegate(bool networkError, bool success);

    public IEnumerator GetPlayerData(string id, ReturnDataHTTPDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url+"/api/player/" + id);


        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            callback(true, false, "");
        }
        else if (www.isHttpError || www.downloadHandler.text == HTTP_REQUEST_EMPTY)
        {
            callback(false, false, "");
        }
        else
        { 
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            callback(false, true, www.downloadHandler.text);
        }
    }

    public IEnumerator CreatePlayerData(string id, string name, BasicHTTPDelegate callback)
    {
        WWWForm postData = new WWWForm();
        postData.AddField("player_id", id);
        postData.AddField("player_name", name);
        UnityWebRequest www = UnityWebRequest.Post(url + "/api/player/create", postData);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            callback(true, false);
        }
        else if (www.isHttpError)
        {
            callback(false, false);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            callback(false, true);
        }
    }

    public IEnumerator UpdatePlayerData(string id, string name, int points, double distance, BasicHTTPDelegate callback)
    {
        WWWForm postData = new WWWForm();
        postData.AddField("player_name", name);
        postData.AddField("points", points);
        postData.AddField("distance", distance.ToString());
        UnityWebRequest www = UnityWebRequest.Post(url + "/api/player/" + id, postData);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            callback(true, false);
        }
        else if (www.isHttpError)
        {
            callback(false, false);
        }
        else
        {
            // Show results as text
            Debug.Log("update success");
            Debug.Log(www.downloadHandler.text);
            callback(false, true);
        }
    }

    public IEnumerator UpdatePlayerBestTime(string playerID, string levelID, float time, BasicHTTPDelegate callback)
    {
        WWWForm postData = new WWWForm();
        postData.AddField("time", time.ToString());
        UnityWebRequest www = UnityWebRequest.Post(url + "/api/best_time/" + playerID + "/" + levelID, postData);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            callback(true, false);
        }
        else if (www.isHttpError)
        {
            callback(false, false);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            callback(false, true);
        }
    }

    public IEnumerator GetBestTimes(int levelID, ReturnDataHTTPDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "/api/best_time/" + levelID.ToString());

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            callback(true, false, "");
        }
        else if (www.isHttpError)
        {
            callback(false, false, "");
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            callback(false, true, www.downloadHandler.text);
        }
    }
}
