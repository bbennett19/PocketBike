using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardOpenClose : MonoBehaviour {

    public GameObject leaderboardObject;
	
    public void Open()
    {
        leaderboardObject.SetActive(true);
    }

    public void Close()
    {
        leaderboardObject.SetActive(false);
    }
}
