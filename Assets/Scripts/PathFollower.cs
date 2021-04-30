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
	void Start() {
		targets = new List<Transform>();
		tCharacter = controlledCharacter.transform;

		// Go through GameObject childs.
		foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>()) {
			// Ignore if we got ourself.
			if (transform.gameObject == this.gameObject)
				continue;

			// Add to the list of targets.
			targets.Add(transform);
		}
	}

	// Update is called once per frame
	void Update() {
		// Check if we have reached our final destionation.
		if (targets.Count == 0)
			return;

		// Move our object towards the waypoint.
		float step = movementSpeed * Time.deltaTime;
		tCharacter.position = Vector3.MoveTowards(tCharacter.position, targets[0].position, step);

		// Check if we have reached our waypoint.
		if (Vector3.Distance(tCharacter.position, targets[0].position) < 0.001f) {
			if (Debug.isDebugBuild)
				Debug.Log("Arrived at: " + targets[0].gameObject.name);
			
			// Remove the first element to go to the next one.
			targets.RemoveAt(0);
		}
	}
}
