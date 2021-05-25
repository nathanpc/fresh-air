using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a waypoint <see cref="GameObject"/> in order
/// for a character to stop and jump a couple of times at that position.
/// </summary>
public class WaypointJump : MonoBehaviour {
	public GameObject controlledObject;
	public int numberOfTimes = 1;
	public float jumpForce = 10.0f;
	public Vector3 jumpVector = new Vector3(0.0f, 2.0f, 0.0f);
	private Rigidbody body;
	private bool isGrounded = true;
	private bool jumpEnabled = false;

	// Start is called before the first frame update
	void Start() {
		if (controlledObject == null)
			MoreDebug.LogUnassignedReference(this.gameObject, nameof(controlledObject));

		body = controlledObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {
		// Check if grounded first.
		if (!isGrounded)
			return;

		if (jumpEnabled) {
			while (numberOfTimes > 0) {
				body.AddForce(jumpVector * jumpForce, ForceMode.Impulse);

				isGrounded = false;
				numberOfTimes--;
			}
		}
	}

	// Checks if the object is grounded.
	void OnCollisionStay() {
		isGrounded = true;
	}

	/// <summary>
	/// Start the jump animation.
	/// </summary>
	public void StartJump() {
		jumpEnabled = true;
	}
}
