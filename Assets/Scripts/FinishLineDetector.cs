using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLineDetector : MonoBehaviour {
    public float levelLength = 750f;
    public Text text;
    private float elapsed = 0f;
    private bool finished = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!finished)
        {
            elapsed += Time.deltaTime;
        }

		if(this.transform.position.x >= levelLength)
        {
            GetComponent<PlayerControllerWheel>().enabled = false;
            finished = true;
        }

        text.text = elapsed.ToString("0.00");
	}
}
