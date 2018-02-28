using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float rotationSpeed;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Vector3.forward * 200f);
            //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.back * 200f);
            //transform.Rotate(Vector3.forward * rotationSpeed * -Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0)
        {
            rb.AddForce(Vector3.right * 250f);
        }
	}
}
