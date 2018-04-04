using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadRace()
    {
        SceneManager.LoadScene("race_main");
    }

    public void LoadTracker()
    {
        SceneManager.LoadScene("tracker");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("shop");
    }
}
