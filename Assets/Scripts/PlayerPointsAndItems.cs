using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerData
{
    public bool[] itemsPurchased = new bool[24];
    public int Points { get; set; }
    public double DistanceTraveled { get; set; }
    
    public void SetupData()
    {
        if(itemsPurchased == null)
        {
            itemsPurchased = new bool[24];
        }
    }
}


public class PlayerPointsAndItems : MonoBehaviour
{
    public bool deleteSave = false;
    public static PlayerPointsAndItems Instance;
    public PlayerData data = new PlayerData();

    private HTTPRequestHandler _httpHander = new HTTPRequestHandler();
    

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

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        Debug.Log("save");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save_data.dat", FileMode.OpenOrCreate);

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/save_data.dat"))
        {
            Debug.Log("load");
            FileStream file = File.Open(Application.persistentDataPath + "/save_data.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            data = bf.Deserialize(file) as PlayerData;
            data.SetupData();
            file.Close();
        }

        //StartCoroutine(_httpHander.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, GeneratePlayerName(), CreatePlayerDataCallback));
        StartCoroutine(_httpHander.GetPlayerData(SystemInfo.deviceUniqueIdentifier, GetPlayerDataCallback));
    }

    private string GeneratePlayerName()
    {
        int num = (int)(UnityEngine.Random.value * 10000);
        return "Guest_" + num.ToString();
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
            StartCoroutine(_httpHander.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, GeneratePlayerName(), CreatePlayerDataCallback));
        }
        else
        {
            Debug.Log(jsonString);
        }
    }
    public void CreatePlayerDataCallback(bool networkError, bool success)
    {
        if (networkError)
        {
            Debug.Log("Network error");
        }
        else if (!success)
        {
            Debug.Log("Player could not be created");
        }
    }


}

