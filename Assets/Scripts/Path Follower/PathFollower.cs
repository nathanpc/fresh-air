using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a <see cref="GameObject"/> that will be following
/// the child objects of another <see cref="GameObject"/>.
/// </summary>
public class PathFollower : MonoBehaviour {
	public GameObject waypointContainer;
	public float movementSpeed = 2.0f;
	public bool repeat = true;
	public bool ignoreY = true;
	private GameObject controlledCharacter;
	private Transform tCharacter;
	private List<Transform> targets;

	// Start is called before the first frame update
	private void Start() {
		// Initialize some properties.
		targets = new List<Transform>();
		controlledCharacter = this.gameObject;
		tCharacter = controlledCharacter.transform;

		// Populate the waypoint container.
		RestartPath();
	}

	// Update is called once per frame
	private void Update() {
		// Check if we have reached our final destination.
		if (IsAtFinalDestination()) {
			if (repeat) {
				RestartPath();
			} else {
				return;
			}
		}

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
			if (transform.gameObject == waypointsParent)
				continue;

			// Add to the list of targets.
			targets.Add(transform);
		}
	}

	/// <summary>
	/// Moves the character towards the current waypoint.
	/// </summary>
	private void MoveTowardsWaypoint() {
		// Just sit still if we are waiting at the waypoint for a timer to finish.
		if (IsAtWaypoint(true))
			return;

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
	/// (Re)starts the object path from the first waypoint.
	/// </summary>
	public void RestartPath() {
		// Set ourselves as the waypoint container.
		SetWaypointContainer(waypointContainer);
	}

	/// <summary>
	/// Checks if the target character is at the current waypoint.
	/// </summary>
	/// <param name="ignoreTimer">Should we ignore the timer?</param>
	/// <returns><code>True</code> if the character is at the current waypoint.</returns>
	public bool IsAtWaypoint(bool ignoreTimer = false) {
		bool atWaypoint = false;
		Vector3 posCharacter = tCharacter.position;
		Vector3 posTarget = targets[0].position;

		// Ignore the Y coordinate?
		if (ignoreY) {
			posCharacter.y = 0;
			posTarget.y = 0;
		}

		// Check if we have reached the waypoint already.
		atWaypoint = Vector3.Distance(posCharacter, posTarget) < 0.1f;
		if (ignoreTimer)
			return atWaypoint;

		// Stop for a while?
		if (atWaypoint) {
			WaypointStop stop = targets[0].GetComponent<WaypointStop>();
			if (stop != null) {
				stop.StartTimer();
				return stop.TimesUp();
			}
		}

		return atWaypoint;
	}

	/// <summary>
	/// Checks if the target character is at its final destination.
	/// </summary>
	/// <returns><code>True</code> if the character is at its final destination.</returns>
	public bool IsAtFinalDestination() {
		return targets.Count == 0;
	}
}