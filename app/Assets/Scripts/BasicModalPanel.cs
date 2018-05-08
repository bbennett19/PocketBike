using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicModalPanel : MonoBehaviour
{
    public Text uiText;

    public void SetTextToDisplay(string text)
    {
        uiText.text = text;
    }
	
	public void CloseClick()
    {
        Destroy(this.gameObject);
    }
}
