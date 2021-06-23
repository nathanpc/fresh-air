using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXSpotlight : MonoBehaviour {
	public DMXController controller;
	public int baseAddress = 1;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Dimmers the spotlight's RED light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 1.</param>
	public void RedDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress, (int)val);
	}
}
