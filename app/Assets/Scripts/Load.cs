using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public bool PerformLoad = true;
    public bool DeleteSaveData = false;

	// Use this for initialization
	void Start ()
    {
        if(DeleteSaveData)
        {
            PlayerPointsAndItems.Instance.DeleteSaveData();
        }
        else if (PerformLoad)
        {
            PlayerPointsAndItems.Instance.Load();
        }

        SceneManager.LoadScene("main_scene");
	}
	
}
