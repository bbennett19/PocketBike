using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public Text nameText;
    public Text pointsText;

	// Use this for initialization
	void Start ()
    {
        nameText.text = PlayerPointsAndItems.Instance.playerData.Name;
        pointsText.text = PlayerPointsAndItems.Instance.playerData.Points.ToString();
        PlayerPointsAndItems.Instance.playerData.PlayerNameChange += UpdatePlayerName;
        PlayerPointsAndItems.Instance.playerData.PlayerPointsChange += UpdatePlayerPoints;
    }

    private void OnDestroy()
    {
        PlayerPointsAndItems.Instance.playerData.PlayerNameChange -= UpdatePlayerName;
        PlayerPointsAndItems.Instance.playerData.PlayerPointsChange -= UpdatePlayerPoints;
    }


    public void UpdatePlayerName(string name)
    {
        nameText.text = name;
    }

    public void UpdatePlayerPoints(int points)
    {
        pointsText.text = points.ToString();
    }
}
