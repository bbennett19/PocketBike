using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPRequestHandler
{
    private const String HTTP_REQUEST_EMPTY = "[]";

    // Callback delegate functions
    public delegate void ReturnDataHTTPDelegate(bool networkError, bool success, string jsonString);
    public delegate void BasicHTTPDelegate(bool networkError, bool success);

    public IEnumerator GetPlayerData(string id, ReturnDataHTTPDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/player/" + id);


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
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/player/create", postData);

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

    public IEnumerator UpdatePlayerData(string id, string name, int points, float distance, BasicHTTPDelegate callback)
    {
        WWWForm postData = new WWWForm();
        postData.AddField("player_name", name);
        postData.AddField("points", points);
        postData.AddField("distance", distance.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/player/"+id, postData);

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

    public IEnumerator UpdatePlayerBestTime(string id, float time, BasicHTTPDelegate callback)
    {
        WWWForm postData = new WWWForm();
        postData.AddField("time", time.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/api/best_time/" + id, postData);

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

    public IEnumerator GetBestTimes(string id, int levelID, ReturnDataHTTPDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8080/api/best_time/" + id + "/" + levelID.ToString());

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
