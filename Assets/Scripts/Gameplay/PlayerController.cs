﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform front;
    public Transform back;
	public float targetSpeed;
    public float speedAcceleration;
	public float maxRotationForce;
	public WheelJoint2D wheel;
	private JointMotor2D _wheelMotor;
	private Rigidbody2D _rgbd;
    private float _currentSpeed = 0f;

    private Vector2 _force = new Vector2();
    private Vector2 _forceLocation = new Vector2();
    private bool _addForce = false;

	// Use this for initialization
	void Start ()
    {        
		_wheelMotor = wheel.motor;
		_rgbd = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
		/*if (Input.GetKey (KeyCode.Space) || Input.touchCount > 0)
        {
			wheel.useMotor = true;
            _currentSpeed += speedAcceleration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, targetSpeed, 0f);
			_wheelMotor.motorSpeed = _currentSpeed;
			wheel.motor = _wheelMotor;
		}
        else
        {
			
		}*/

        _addForce = false;
        wheel.useMotor = false;

       
        // Accelerate/back flip
        if (Input.GetKey (KeyCode.D) || IsScreenSectionTouched(Screen.width/2, Screen.width))
        {
            _force = -this.transform.up * maxRotationForce;
            _forceLocation = this.transform.position - (transform.position-back.position);
            _addForce = true;
            wheel.useMotor = true;
            _currentSpeed += speedAcceleration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, targetSpeed, 0f);
            _wheelMotor.motorSpeed = _currentSpeed;
            wheel.motor = _wheelMotor;

        }
        // Break/front flip overrides acceleration
        if (Input.GetKey(KeyCode.A) || IsScreenSectionTouched(0, Screen.width/2))
        {
            _force = -this.transform.up * maxRotationForce;
            _forceLocation = this.transform.position - (transform.position - front.position);
            Debug.Log("FORCE: " + _force.ToString());
            Debug.Log("FORCE LOCATION: " + _forceLocation.ToString());
            _addForce = true;
            wheel.useMotor = true;
            _wheelMotor.motorSpeed = 0;
            wheel.motor = _wheelMotor;

        }
    }

    private bool IsScreenSectionTouched(int left, int right)
    {
        foreach(Touch t in Input.touches)
        {
            if(t.position.x >= left && t.position.x <= right)
            {
                return true;
            }
        }

        if (Input.GetMouseButton(0) && Input.mousePosition.x >= left && Input.mousePosition.x <= right)
            return true;
        return false;
    }

    void FixedUpdate()
    {
        if (_addForce)
        {
            Debug.DrawRay(_forceLocation, _force*5f, Color.red);
            _rgbd.AddForceAtPosition(_force, _forceLocation);
        }
	}

    public void Freeze()
    {
        _rgbd.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
