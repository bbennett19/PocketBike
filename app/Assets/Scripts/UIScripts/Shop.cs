using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text pointsText;

	// Use this for initialization
	void Start ()
    {
        pointsText.text = "Points: " + PlayerPointsAndItems.Instance.playerData.Points.ToString();
	}
}
