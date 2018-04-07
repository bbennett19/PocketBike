using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICategorySelector : MonoBehaviour
{
    public Button[] buttons;
    public int defaultSelected = 0;

    private int _currentSelected;

	// Use this for initialization
	void Start ()
    {
        _currentSelected = defaultSelected;
        buttons[_currentSelected].interactable = false;
	}

    public void CategoryClicked(int category)
    {
        buttons[_currentSelected].interactable = true;
        _currentSelected = category;
        buttons[_currentSelected].interactable = false;
    }
}
