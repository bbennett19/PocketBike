using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerPointsAndItems : MonoBehaviour
{
    public bool deleteSave = false;
    public static PlayerPointsAndItems Instance;
    public PlayerData playerData = new PlayerData();
    private bool _newPlayer = false;

	// Use this for initialization
	void Awake ()
    {
		if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            if(File.Exists(Application.persistentDataPath + "/save_data.dat") && deleteSave)
            {
                File.Delete(Application.persistentDataPath + "/save_data.dat");
            }
            Load();
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
	}

    public bool IsNewPlayer()
    {
        return _newPlayer;
    }

    // Make sure to save the data before quitting
    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            Save();
        }
        else
        {
            // CHeck for GPS updates
        }
    }

    public void Save()
    {
        Debug.Log("save");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save_data.dat", FileMode.OpenOrCreate);

        bf.Serialize(file, playerData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/save_data.dat"))
        {
            Debug.Log("load");
            FileStream file = File.Open(Application.persistentDataPath + "/save_data.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            playerData = bf.Deserialize(file) as PlayerData;
            playerData.SetupData();
            file.Close();

            CheckForDataToUpload();
        }
        else
        {
            // If no save data it is a new player
            _newPlayer = true;
        }

        if(SceneManager.GetActiveScene().name == "LoadingScene")
        {
            SceneManager.LoadScene("main_scene");
        }
    }

    private string GeneratePlayerName()
    {
        int num = (int)(UnityEngine.Random.value * 10000);
        return "Guest_" + num.ToString();
    }

    public void CheckForDataToUpload()
    {
        if (!playerData.PlayerDataHasBeenCreated)
        {
            StartCoroutine(HTTPRequestHandler.Instance.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, playerData.Name, CreatePlayerDataCallback));
        }
        else if(playerData.PlayerDataToUpload)
        {
            StartCoroutine(HTTPRequestHandler.Instance.UpdatePlayerData(SystemInfo.deviceUniqueIdentifier, playerData.Name, playerData.Points, playerData.DistanceTraveled, UpdatePlayerDataCallback));
        }
    }

    public void GetPlayerDataCallback(bool networkError, bool success, string jsonString)
    {
        if(networkError)
        {
            Debug.Log("Network error");
        }
        else if(!success)
        {
            // Create player data
            StartCoroutine(HTTPRequestHandler.Instance.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, GeneratePlayerName(), CreatePlayerDataCallback));
        }
        else
        {
            Debug.Log(jsonString);
        }
    }

    public void CreatePlayerDataCallback(bool networkError, bool success)
    {
        if(success)
        {
            playerData.PlayerDataHasBeenCreated = true;

            if(playerData.PlayerDataToUpload)
            {
                StartCoroutine(HTTPRequestHandler.Instance.UpdatePlayerData(SystemInfo.deviceUniqueIdentifier, playerData.Name, playerData.Points, playerData.DistanceTraveled, UpdatePlayerDataCallback));
            }
        }
    }

    public void UpdatePlayerDataCallback(bool networkError, bool success)
    {
        if(success)
        {
            playerData.PlayerDataToUpload = false;
        }
    }


}

