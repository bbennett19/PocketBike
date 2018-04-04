using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float targetSpeed;
	public float maxTorque;
	public WheelJoint2D wheel;
	private JointMotor2D _wheelMotor;
	private Rigidbody2D _rgbd;
	private float _torque;
	// Use this for initialization
	void Start () {
		_wheelMotor = wheel.motor;
		_rgbd = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			wheel.useMotor = true;
			_wheelMotor.motorSpeed = targetSpeed;
			wheel.motor = _wheelMotor;
		} else {
			wheel.useMotor = false;
			//_wheelMotor.motorSpeed = 0;
		}
			

		if (Input.GetKey (KeyCode.A)) {
			_torque = maxTorque;
		} else if (Input.GetKey (KeyCode.D)) {
			_torque = -maxTorque;
		} else {
			_torque = 0f;
		}

	}

	void FixedUpdate() {
		_rgbd.AddTorque(_torque);
	}

}
