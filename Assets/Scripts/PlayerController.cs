using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float targetSpeed;
    public float speedAcceleration;
	public float maxTorque;
	public float accelerometerMax = 0.7f;
	public float accelerometerDeadZone = 0.1f;
	public WheelJoint2D wheel;
	private JointMotor2D _wheelMotor;
	private Rigidbody2D _rgbd;
	private float _torque;
    private float _currentSpeed = 0f;

	// Use this for initialization
	void Start ()
    {        
		_wheelMotor = wheel.motor;
		_rgbd = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey (KeyCode.Space) || Input.touchCount > 0)
        {
			wheel.useMotor = true;
            _currentSpeed += speedAcceleration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, targetSpeed, 0f);
			_wheelMotor.motorSpeed = _currentSpeed;
			wheel.motor = _wheelMotor;
		}
        else
        {
			wheel.useMotor = false;
		}

        Debug.Log(Input.acceleration);

		if (Input.GetKey (KeyCode.A) || Input.acceleration.x < accelerometerDeadZone)
        {
			Debug.Log ("back");
			_torque = maxTorque*(Input.acceleration.x*-1f);
		}
        else if (Input.GetKey (KeyCode.D) || Input.acceleration.x > accelerometerDeadZone)
        {
			Debug.Log ("Forward");
			_torque = -maxTorque*Input.acceleration.x;
		}
        else
        {
			_torque = 0f;
            _rgbd.angularVelocity = 0f;
		}

    }

    void FixedUpdate()
    {
		_rgbd.AddTorque(_torque);
        _rgbd.angularVelocity = Mathf.Clamp(_rgbd.angularVelocity, -180f, 180f);
	}

    public void Freeze()
    {
        _rgbd.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
