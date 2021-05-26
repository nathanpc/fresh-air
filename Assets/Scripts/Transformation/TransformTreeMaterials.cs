using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transforms a tree's <see cref="GameObject"/> materials properties.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class TransformTreeMaterials : MonoBehaviour, ITransformation {
	public GameObject fog;
	public Material transformedMaterial1;
	public Material transformedMaterial2;
	private Material oldMaterial1;
	private Material oldMaterial2;
	private bool transformed = false;
	private float insideDistance = 33.0f;

	// Start is called before the first frame update
	void Start() {
		oldMaterial1 = transform.gameObject.GetComponent<MeshRenderer>().materials[0];
		oldMaterial2 = transform.gameObject.GetComponent<MeshRenderer>().materials[1];
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
		// Grab the materials.
		Material[] materials = transform.gameObject.GetComponent<MeshRenderer>().materials;
		materials[0] = transformedMaterial1;
		materials[1] = transformedMaterial2;

		// Do the actual transformation.
		transform.gameObject.GetComponent<MeshRenderer>().materials = materials;
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
