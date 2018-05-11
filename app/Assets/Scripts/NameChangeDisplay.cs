using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameChangeDisplay : MonoBehaviour
{
    public GameObject nameChangeModalPanel;
    public Transform parent;

    public void NameChangeClick()
    {
        Instantiate(nameChangeModalPanel, parent);
    }
	
}
