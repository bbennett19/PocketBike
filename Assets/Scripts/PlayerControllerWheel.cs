using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWheel : MonoBehaviour {
    public float rotationSpeed;
    private Rigidbody rb;
    private Vector3 torque = Vector3.zero;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20;
	}
	
	// Update is called once per frame
	void Update () {
        torque = Vector3.zero;

        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
        {
            torque = Vector3.back * 30f;
        }
	}

    private void FixedUpdate()
    {
        rb.AddTorque(torque);
    }
}
