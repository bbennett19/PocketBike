using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {
    public float position;

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector2(position, 0);	
	}

}
