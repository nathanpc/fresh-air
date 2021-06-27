using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpeed : WaypointBase {
	public float movementSpeed = 1;
	public float rotationSpeed = 1;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Sets the parent <see cref="PathFollower"/> speeds.
	/// </summary>
	public void SetPathFollowerSpeeds() {
		follower.movementSpeed = movementSpeed;
		follower.rotationSpeed = rotationSpeed;
	}

	/// <inheritdoc/>
	public override string WaypointType() {
		return "Speed";
	}
}
