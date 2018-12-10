using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
	private bool _mustRotate = false; // Indicate if the robot must translate (false) or rotate (true)
	private bool _rotating = false; // Indicate if the robot is already rotating or not
	private int targetRotationY = 0; // Indicate the target rotation Y value

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_mustRotate) {
			// Move forward
			transform.Translate (Vector3.forward * Time.deltaTime);
		} else {
			if (!_rotating) {
				// Start of rotation: generate rotation angle
				int rotation = Random.Range (90, 181);
				// Calculate target rotation Y value
				targetRotationY = (int) transform.rotation.eulerAngles.y + rotation;
				// Angles must be in the [-180:180] interval
				if (targetRotationY > 180)
					targetRotationY = targetRotationY - 360;
				Debug.Log ("Target rotation Y: " + targetRotationY);
				_rotating = true;
			}
			if (Mathf.DeltaAngle (transform.rotation.eulerAngles.y, targetRotationY) > 3f) {
				// Target rotation Y not reached yet: continue rotation
				transform.Rotate (Vector3.up, 20f * Time.deltaTime);
			} else {
				// Target rotation Y reached: stop rotation
				Debug.Log ("Ending rotation");
				_mustRotate = false;
				_rotating = false;
			}
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		// New collision and no rotation pending: start a new rotation
		if ((collision.gameObject.tag == "Wall") && !_mustRotate) {
			Debug.Log ("Collision with wall: starting rotation");
			_mustRotate = true;
		}
	}
}
