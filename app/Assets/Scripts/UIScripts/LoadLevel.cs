using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
	public void LoadLevelClick(int levelID)
    {
		LevelSelectionData.Instance.LevelID = levelID;
		SceneManager.LoadScene("race_main");
    }
}
