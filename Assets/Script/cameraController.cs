﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	//Public Variables
	public Transform car;
	float distance = 6.4f;
	float height = 1.4f;
	float rotationDamping = 3.0f;
	float heightDamping = 2.0f;
	float zoomRatio = 0.5f;
	float defaultFOV = 60f;

	//Private Variables
	Vector3 rotationVector;
	public Rigidbody rigidbod;
	Camera cam;
    private void Start()
    {
		
		cam = GetComponent<Camera>();
	}

    void LateUpdate(){
		if(car != null){
			float wantedAngle = rotationVector.y;
			float wantedHeight = car.position.y + height; //
			float myAngle = transform.eulerAngles.y;
			float myHeight = transform.position.y;

			myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
			myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);

			Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
			transform.position = car.position;
			transform.position -= currentRotation * Vector3.forward * distance;
			Vector3 temporary = transform.position;
			temporary.y = myHeight;
			transform.position = temporary;
			transform.LookAt(car.transform.position + Vector3.up * 2f);
		}
	}

	void FixedUpdate(){
		if(car != null){
			Vector3 localVelocity = car.InverseTransformDirection(rigidbod.velocity);
			if(localVelocity.z < -10.5f){
				Vector3 temp = rotationVector;
				temp.y = car.eulerAngles.y + 180;
				rotationVector = temp;
			}else{
				Vector3 temp = rotationVector;
				temp.y = car.eulerAngles.y;
				rotationVector = temp;
			}
			float acc = rigidbod.velocity.magnitude;
			cam.fieldOfView = defaultFOV + acc * zoomRatio * Time.deltaTime;
		}
	}
}
