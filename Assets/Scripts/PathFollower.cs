using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a <see cref="GameObject"/> with child objects
/// that define the path that another <see cref="GameObject"/> should take.
/// </summary>
public class PathFollower : MonoBehaviour {
	public GameObject controlledCharacter;
	public float movementSpeed = 2.0f;
	private Transform tCharacter;
	private List<Transform> targets;

	// Start is called before the first frame update
	private void Start() {
		// Initialize some properties.
		targets = new List<Transform>();
		tCharacter = controlledCharacter.transform;

		// Set ourselves as the waypoint container.
		SetWaypointContainer(this.gameObject);
	}

	// Update is called once per frame
	private void Update() {
		// Check if we have reached our final destination.
		if (IsAtFinalDestination())
			return;

		// Move to the current waypoint.
		MoveTowardsWaypoint();

		// Check if we have reached our waypoint.
		if (IsAtWaypoint()) {
			if (Debug.isDebugBuild)
				Debug.Log("Arrived at: " + targets[0].gameObject.name);

			// Move to the next waypoint.
			MoveToNextWaypoint();
		}
	}

	/// <summary>
	/// Sets the current waypoints container <see cref="GameObject"/>
	/// </summary>
	/// <param name="waypointsParent">Container of waypoints.</param>
	public void SetWaypointContainer(GameObject waypointsParent) {
		// Go through GameObject childs.
		foreach (Transform transform in waypointsParent.GetComponentsInChildren<Transform>()) {
			// Ignore if we got ourself.
			if (transform.gameObject == this.gameObject)
				continue;

			// Add to the list of targets.
			targets.Add(transform);
		}
	}

	/// <summary>
	/// Moves the character towards the current waypoint.
	/// </summary>
	private void MoveTowardsWaypoint() {
		// Move our object towards the waypoint.
		float step = movementSpeed * Time.deltaTime;
		tCharacter.position = Vector3.MoveTowards(tCharacter.position, targets[0].position, step);
	}

	/// <summary>
	/// Starts moving the character to its next waypoint.
	/// </summary>
	public void MoveToNextWaypoint() {
		// Remove the first element to go to the next one.
		targets.RemoveAt(0);
	}

	/// <summary>
	/// Checks if the target character is at the current waypoint.
	/// </summary>
	/// <returns><code>True</code> if the character is at the current waypoint.</returns>
	public bool IsAtWaypoint() {
		return Vector3.Distance(tCharacter.position, targets[0].position) < 0.001f;
	}

	/// <summary>
	/// Checks if the target character is at its final destination.
	/// </summary>
	/// <returns><code>True</code> if the character is at its final destination.</returns>
	public bool IsAtFinalDestination() {
		return targets.Count == 0;
	}
}
