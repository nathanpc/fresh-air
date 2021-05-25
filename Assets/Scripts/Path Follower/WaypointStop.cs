using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to be attached to a waypoint <see cref="GameObject"/> in order
/// for a character to stop for some seconds at that position.
/// </summary>
public class WaypointStop : MonoBehaviour {
	public int stoppedTime = 2;
	public GameObject controlledObject;
	private bool stopEnabled = false;
	private PathFollower follower;
	private float timeLeft;
	private bool finished = false;

	// Start is called before the first frame update
	void Start() {
		follower = controlledObject.GetComponent<PathFollower>();
	}

	// Update is called once per frame
	void Update() {
		if (stopEnabled) {
			// Count down.
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
				finished = true;
		}
	}

	/// <summary>
	/// Starts the "stop" timer.
	/// </summary>
	public void StartTimer() {
		// Ignore if we've already started the timer.
		if (stopEnabled)
			return;

		timeLeft = stoppedTime;
		finished = false;
		stopEnabled = true;
	}

	/// <summary>
	/// Stops the "stop" timer.
	/// </summary>
	public void StopTimer() {
		stopEnabled = false;
		finished = false;
		timeLeft = stoppedTime;
	}

	/// <summary>
	/// Checks if the timer has finished doing its thing.
	/// </summary>
	/// <returns>Is the time up?</returns>
	public bool TimesUp() {
		return finished;
	}
}
