using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionData : MonoBehaviour {

	public int LevelID;
	public static LevelSelectionData Instance;

	// Use this for initialization
	void Awake ()
	{
		if(Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}
	}
}
