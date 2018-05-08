using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeStartup : MonoBehaviour
{

    public GameObject firstTimeStartupModalPanel;
    public Transform parent;

    // Use this for initialization
    void Start ()
    {
        if(PlayerPointsAndItems.Instance.playerData.DisplayGetName)
        {
            Instantiate(firstTimeStartupModalPanel, parent);
        }
    }
	
}
