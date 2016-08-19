using UnityEngine;
using System;

public class MoveCamera : MonoBehaviour {

	TextAsset Constants; // Stores the contents of Constants.txt e.g. visualisationSize:300
	string[] ConstantsArray; // Stores the lines of Constants.txt as an array, e.g. {visualisationSize:300} (as I (Ross) left it, there was only one line in the first place)
	string[] parts_of_constant; // Stores elements of each line as an array of string values, e.g. {visualisationSize, 300}
	float xMax; // Absolute value of x the camera is allowed from the centre before it is stopped (values obtained in Start())
	float yMax; // Ditto for y
	float zMax; // Ditto for x
	bool moving; // Is the camera moving?
	float speed = 0.7F; // Speed of camera (may wish to change this depending on the scale of the visualisation in Unity space)


	void Start() {
		
		// Get the values for the outer limits to where the camera can move xMax, yMax, zMax
		Constants = Resources.Load<TextAsset> ("Constants"); // Load constants text file
		ConstantsArray = Constants.text.Split ('\n'); // Split lines of constants text file into array
		foreach (string constant in ConstantsArray) {
			// This loops through the lines of Constants.txt to find the one holding the value of visualisationSize
			// Note that I (Ross) only used Constants.txt to hold this value, but this generalises for potential future developments
			// where more constants must be stored
			parts_of_constant = constant.Split (':'); // Split each line into array e.g. visualisationSize:300 -> {visualisationSize, 300}
			if (parts_of_constant [0] == "visualisationSize") {
				xMax = Int32.Parse (parts_of_constant [1]); // E.g. xMax = 300
				yMax = Int32.Parse (parts_of_constant [1]);
				zMax = Int32.Parse (parts_of_constant [1]);
			}
		}
	}


	void Update () {
		
		if (GvrViewer.Instance.Triggered) { // If the trigger is pulled
			if (moving) {
				moving = false; // If the camera was already moving, stop moving
			} else {
				moving = true; // If the camera was not already moving, start moving
			}
		}

		stopCamera (); // Detects if the camera is at the edge of the visualisation and if it is, stop it from moving
		Move (moving); // Call the Move() function, which decides what to do based on whether or not moving is true
	}


	void Move (bool moving) {

		if (moving) {
			transform.Translate (Camera.main.transform.forward * speed);
		} else {
			// Do nothing
		}
	}


	void stopCamera() { // Stop the camera from moving further out if it gets too far from the centre
		
		if (transform.position.x > xMax) {
			transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
		} else if (transform.position.x < -xMax) {
			transform.position = new Vector3(-xMax, transform.position.y, transform.position.z);
		}
		if (transform.position.y > yMax) {
			transform.position = new Vector3(transform.position.x, yMax, transform.position.z);
		} else if (transform.position.y < -yMax) {
			transform.position = new Vector3(transform.position.x, -yMax, transform.position.z);
		}
		if (transform.position.z > zMax) {
			transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
		} else if (transform.position.z < -zMax) {
			transform.position = new Vector3(transform.position.x, transform.position.y, -zMax);
		}
	}
}