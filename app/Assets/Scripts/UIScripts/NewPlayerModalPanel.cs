using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerModalPanel : MonoBehaviour
{
    public Button submitButton;
    public InputField inputText;

	// Use this for initialization
	void Start ()
    {
        submitButton.interactable = false;
	}
	
	public void Submit()
    {
        // Attempt to create player on server
        PlayerPointsAndItems.Instance.playerData.SetPlayerNameChangeWithEvent(inputText.text);
        PlayerPointsAndItems.Instance.playerData.PlayerDataToUpload = true;
        PlayerPointsAndItems.Instance.playerData.DisplayGetName = false;
        PlayerPointsAndItems.Instance.playerData.PlayerDataHasBeenCreated = false;
        StartCoroutine(HTTPRequestHandler.Instance.CreatePlayerData(SystemInfo.deviceUniqueIdentifier, inputText.text, CreatePlayerDataCallback));
    }

    public void CreatePlayerDataCallback(bool networkError, bool success)
    {
        if (success)
        {
            PlayerPointsAndItems.Instance.playerData.PlayerDataToUpload = false;
            PlayerPointsAndItems.Instance.playerData.PlayerDataHasBeenCreated = true;
            gameObject.SetActive(false);
        }
        else if (!networkError && !success)
        {
            PlayerPointsAndItems.Instance.playerData.PlayerDataHasBeenCreated = true;

            // Player must have already existed on the server, update data instead of create
            StartCoroutine(HTTPRequestHandler.Instance.UpdatePlayerData(SystemInfo.deviceUniqueIdentifier, inputText.text, 
                PlayerPointsAndItems.Instance.playerData.Points, PlayerPointsAndItems.Instance.playerData.DistanceTraveled, UpdatePlayerDataCallback));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdatePlayerDataCallback(bool networkError, bool success)
    {
        if(success)
        {
            PlayerPointsAndItems.Instance.playerData.PlayerDataToUpload = false;
        }
        Destroy(this.gameObject);
        
    }

    public void ValueChange(string value)
    {
        if(value.Length > 0)
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }
}
