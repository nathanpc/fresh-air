using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transforms a <see cref="GameObject"/> when the fog touches it using our
/// custom shader.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class TransformShader : MonoBehaviour, ITransformation {
	public GameObject fog;
	public float transformationAmount = 0.0f;
	private Renderer objectRenderer;
	private float insideDistance = 33.0f;

	// Start is called before the first frame update
	void Start() {
		objectRenderer = gameObject.GetComponent<Renderer>();
	}

	// Update is called once per frame
	void Update() {
		// Do nothing if we've already transformed.
		if (transformationAmount == 1.0f)
			return;

		// Check if we are inside the fog.
		if (InsideFog())
			StartTransformation();

		// Apply transformation to the shader.
		objectRenderer.material.SetFloat("_Transformation", transformationAmount);
	}

	public void StartTransformation() {
		transformationAmount = 1;
	}

	/// <summary>
	/// Check if our <see cref="GameObject"/> is inside the fog.
	/// </summary>
	/// <returns>True if we are inside the fog.</returns>
	private bool InsideFog() {
		// Check if the distance from the fog is less than its radius.
		return Vector3.Distance(transform.position, fog.transform.position) < insideDistance;
	}
}
