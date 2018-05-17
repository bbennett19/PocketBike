using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelector : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] objects;
    public int selected = 0;

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }

		for(int i = 0; i < buttons.Length; i++)
        {
            int temp = i;
            buttons[i].onClick.AddListener(() => ChangeCategory(temp));
        }

        objects[selected].SetActive(true);
        buttons[selected].interactable = false;
	}

    public void ChangeCategory(int c)
    {
        objects[c].SetActive(true);
        objects[selected].SetActive(false);
        buttons[c].interactable = false;
        buttons[selected].interactable = true;
        selected = c;
    }
}
