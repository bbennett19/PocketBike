using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffToggle : MonoBehaviour {
    public Color onColor;
    public Color offColor;
    public Text text;
    public Slider toggle;
    public string onText;
    public string offText;

    private bool _on = true;

	public void Toggle()
    {
        if(_on)
        {
            toggle.value = 0;
            _on = false;
            text.color = offColor;
            text.text = offText;
        }
        else
        {
            toggle.value = 1;
            _on = true;
            text.color = onColor;
            text.text = onText;
        }
    }
}
