using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject lockTimer;
    public GameObject unlockButtonStuff;
    public GameObject playText;
	public Text timeText;
    public Text costText;
	public int levelID;
	public int unlockCost;
    public int unlockHours = 12;
    public TimeSpan unlockTime;
    public GameObject basicModalPrefab;
    public Transform parent;
	private bool _levelUnlocked = false;
    private Button _thisButton;
    

	void Awake()
	{
        unlockTime = new TimeSpan(unlockHours, 0, 0);
        UpdateLockedStatus();
        costText.text = unlockCost.ToString();
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(ButtonClick);
    }
	
	// Update is called once per frame
	void Update () {
		if (_levelUnlocked) {
			UpdateLockedStatus();
		}
	}

	private void UpdateLockedStatus()
	{
		TimeSpan elapsed = DateTime.Now-PlayerPointsAndItems.Instance.playerData.levelUnlockTime [levelID];	
		_levelUnlocked = elapsed < unlockTime;
        playText.SetActive(_levelUnlocked);
        lockTimer.SetActive(_levelUnlocked);
        unlockButtonStuff.SetActive(!_levelUnlocked);

		if (_levelUnlocked)
        {
			TimeSpan timeRemaining = unlockTime - elapsed;
			timeText.text = timeRemaining.Hours.ToString ("D2") + ":" + timeRemaining.Minutes.ToString ("D2") + ":" + timeRemaining.Seconds.ToString ("D2");
		}
		else
		{
			timeText.text = "-:--";
		}
	}

	private void Unlock()
	{
		if (PlayerPointsAndItems.Instance.playerData.Points >= unlockCost) {
			_thisButton.interactable = false;
			NetworkOperations.Instance.UpdatePlayerData (UpdateComplete, false, unlockCost);
		}
        else
        {
            // Display insufficient funds
            GameObject g = Instantiate(basicModalPrefab, parent);
            g.GetComponent<BasicModalPanel>().SetTextToDisplay("Insufficient points to unlock. Do activity to earn more points.");
			Debug.Log("No money");
		}
	}

	public void UpdateComplete(bool success)
	{
        _thisButton.interactable = true;
		if (success)
        {
			PlayerPointsAndItems.Instance.playerData.SetPlayerPointsWithEvent (PlayerPointsAndItems.Instance.playerData.Points - unlockCost);
			PlayerPointsAndItems.Instance.playerData.levelUnlockTime [levelID] = DateTime.Now;
			UpdateLockedStatus();
		}
        else
        {
            // Display network error
            GameObject g = Instantiate(basicModalPrefab, parent);
            g.GetComponent<BasicModalPanel>().SetTextToDisplay("Unable to connect to the server.");
            Debug.Log("Network error");
		}
	}

    public void ButtonClick()
    {
        if(!_levelUnlocked)
        {
            Unlock();
        }
        else
        {
            Play();
        }
    }

	private void Play()
	{
		LevelSelectionData.Instance.LevelID = levelID;
		SceneManager.LoadScene("race_main");
	}

}
