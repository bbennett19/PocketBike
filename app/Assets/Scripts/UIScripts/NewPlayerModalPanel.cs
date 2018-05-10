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
        submitButton.interactable = false;
        NetworkOperations.Instance.UpdatePlayerData(UpdatePlayerDataCallback);
    }

    public void UpdatePlayerDataCallback(bool success)
    {
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
