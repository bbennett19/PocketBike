using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMover : MonoBehaviour {
    public float speed;
    private Vector2 pos;
	// Use this for initialization
	void Start () {
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        pos.x -= speed * Time.deltaTime;
        this.transform.position = pos;

        if(this.transform.position.x <= -15f)
        {
            Destroy(this.gameObject);
        }
	}
}
