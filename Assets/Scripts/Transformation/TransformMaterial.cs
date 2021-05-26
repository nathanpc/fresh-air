using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transforms the <see cref="GameObject"/> material property.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class TransformMaterial : MonoBehaviour, ITransformation {
	public GameObject fog;
	public Material transformedMaterial;
	private Material oldMaterial;
	private bool transformed = false;
	private float insideDistance = 33.0f;

	// Start is called before the first frame update
	void Start() {
		oldMaterial = transform.gameObject.GetComponent<MeshRenderer>().material;
	}

	// Update is called once per frame
	void Update() {
		// Do nothing if we've already transformed.
		if (transformed)
			return;

		// Check if we are inside the fog.
		if (InsideFog())
			StartTransformation();
	}

	public void StartTransformation() {
		// Do the actual transformation.
		transform.gameObject.GetComponent<MeshRenderer>().material = transformedMaterial;
		transformed = true;
	}

	/// <summary>
	/// Check if our <see cref="GameObject"/> is inside the fog.
	/// </summary>
	private bool InsideFog() {
		// Check if the distance from the fog is less than its radius.
		return Vector3.Distance(transform.position, fog.transform.position) < insideDistance;
	}
}
