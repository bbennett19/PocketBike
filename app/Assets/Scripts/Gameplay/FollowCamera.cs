using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform followTransform;
    private Vector3 transformVec;
	// Use this for initialization
	void Start ()
    {
        transformVec = followTransform.position;
        transformVec.z = this.transform.position.z;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transformVec.x = followTransform.position.x;
        transformVec.y = followTransform.position.y;
        this.transform.position = transformVec;
	}
}
