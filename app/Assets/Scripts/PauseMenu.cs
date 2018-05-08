using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void Continue()
	{
		RaceManager.ResumeRace ();
		gameObject.SetActive (false);
	}

	public void Pause()
	{
		RaceManager.PauseRace ();
		gameObject.SetActive (true);
	}
}
