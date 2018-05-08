using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
	public GameObject[] levels;
	public Vector2[] bikeStartingLocation;
	public float[] bikeStartingRotation;
	public GameObject bike;

	// Use this for initialization
	void Start () 
	{
		// Set all levels inactive
		foreach (GameObject g in levels) {
			g.SetActive (false);
		}
		levels [LevelSelectionData.Instance.LevelID].SetActive (true);
		bike.transform.position = bikeStartingLocation [LevelSelectionData.Instance.LevelID];
		bike.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, bikeStartingRotation [LevelSelectionData.Instance.LevelID]));
	}
}
