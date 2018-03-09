using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBike : MonoBehaviour {
    public float rotationSpeed;
    private Rigidbody rb;
    private Vector3 torque = Vector3.zero;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        torque = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            torque = Vector3.forward * 250f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            torque = Vector3.back * 250f;
        }
	}

    private void FixedUpdate()
    {
        rb.AddTorque(torque);
    }
}
