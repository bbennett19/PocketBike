using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float rotationSpeed;
    private Rigidbody rb;
    private Vector3 lr_force = Vector3.zero;
    private Vector3 torque = Vector3.zero;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        lr_force = Vector3.zero;
        torque = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            torque = Vector3.forward * 250f;
            //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            torque = Vector3.back * 250f;
            //transform.Rotate(Vector3.forward * rotationSpeed * -Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
        {
            lr_force = Vector3.right * 300f;
        }
	}

    private void FixedUpdate()
    {
        rb.AddTorque(torque);
        rb.AddForce(lr_force);
    }
}
