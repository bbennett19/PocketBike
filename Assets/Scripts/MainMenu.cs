using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void LoadRace()
    {
        SceneManager.LoadScene("main");
    }

    public void LoadTracker()
    {
        SceneManager.LoadScene("tracker");
    }
}
