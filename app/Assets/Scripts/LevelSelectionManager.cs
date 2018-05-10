using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour {
	public Button playButton;
	public Button unlockButton;
	public Text timeText;
	public int levelID;
	public int unlockCost;
	public TimeSpan unlockTime = new TimeSpan(0, 5, 0);
	private bool _levelUnlocked = false;

	void Awake()
	{
		UpdateLockedStatus ();
		playButton.onClick.AddListener (() => PlayClick());
		unlockButton.onClick.AddListener (() => UnlockClick());
	}
	
	// Update is called once per frame
	void Update () {
		if (_levelUnlocked) {
			UpdateLockedStatus ();
		}
	}

	private void UpdateLockedStatus()
	{
		TimeSpan elapsed = DateTime.Now-PlayerPointsAndItems.Instance.playerData.levelUnlockTime [levelID];	
		_levelUnlocked = elapsed < unlockTime;
		playButton.interactable = _levelUnlocked;
		unlockButton.interactable = !_levelUnlocked;

		if (_levelUnlocked) {
			TimeSpan timeRemaining = unlockTime - elapsed;
			timeText.text = timeRemaining.Hours.ToString ("D2") + ":" + timeRemaining.Minutes.ToString ("D2") + ":" + timeRemaining.Seconds.ToString ("D2");
		}
		else
		{
			timeText.text = "";
		}
	}

	public void UnlockClick()
	{
		if (PlayerPointsAndItems.Instance.playerData.Points >= unlockCost) {
			unlockButton.interactable = false;
			NetworkOperations.Instance.UpdatePlayerData (UpdateComplete, false, unlockCost);
		} else {
			// Display insufficient funds
			Debug.Log("No money");
		}
	}

	public void UpdateComplete(bool success)
	{
		if (success) {
			PlayerPointsAndItems.Instance.playerData.SetPlayerPointsWithEvent (PlayerPointsAndItems.Instance.playerData.Points - unlockCost);
			PlayerPointsAndItems.Instance.playerData.levelUnlockTime [levelID] = DateTime.Now;
			UpdateLockedStatus ();
		} else {
			// Display network error
			Debug.Log("Network error");
		}
	}

	public void PlayClick()
	{
		LevelSelectionData.Instance.LevelID = levelID;
		SceneManager.LoadScene("race_main");
	}

}
