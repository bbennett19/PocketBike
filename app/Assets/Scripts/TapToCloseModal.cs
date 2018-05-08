using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToCloseModal : MonoBehaviour {
    private int _tapCount = 0;
	// Use this for initialization
	void Start ()
    {
        _tapCount = Input.touchCount;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > _tapCount || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            RaceManager.StartRace();
            Destroy(this.gameObject);
        }
        else if(Input.touchCount < _tapCount)
        {
            _tapCount = Input.touchCount;
        }
	}
}
