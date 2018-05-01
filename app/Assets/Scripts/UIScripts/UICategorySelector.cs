using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICategorySelector : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] activatableObjects;
    public int defaultSelected = 0;

    private int _currentSelected;

	// Use this for initialization
	void Start ()
    {
        _currentSelected = defaultSelected;
        buttons[_currentSelected].interactable = false;

        foreach(GameObject o in activatableObjects)
        {
            o.SetActive(false);
        }
        activatableObjects[_currentSelected].SetActive(true);
	}

    public void CategoryClicked(int category)
    {
        buttons[_currentSelected].interactable = true;
        activatableObjects[_currentSelected].SetActive(false);
        _currentSelected = category;
        buttons[_currentSelected].interactable = false;
        activatableObjects[_currentSelected].SetActive(true);
    }
}
