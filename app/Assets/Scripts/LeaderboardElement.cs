using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour
{
    public Text placeText;
    public Text nameText;
    public Text timeText;

    public Color highlightColor;

	public void SetFields(string id, int place, string name, double time, string label)
    {
        placeText.text = place.ToString() + ".";
        nameText.text = name;
        timeText.text = time.ToString("0.0000") + " " + label;

        if(id == SystemInfo.deviceUniqueIdentifier)
        {
            GetComponent<Image>().color = highlightColor;
        }
    }
}
