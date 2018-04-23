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
        UpdateText();	
	}
	
	public void UpdateText()
    {
        nameText.text = PlayerPointsAndItems.Instance.playerData.Name;
        pointsText.text = "Points: " + PlayerPointsAndItems.Instance.playerData.Points.ToString();
    }
}
