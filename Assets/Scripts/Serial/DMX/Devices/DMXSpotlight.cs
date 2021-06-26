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
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void RedDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress, NormalizeValue(val));
	}

	/// <summary>
	/// Dimmers the spotlight's GREEN light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void GreenDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 1, NormalizeValue(val));
	}

	/// <summary>
	/// Dimmers the spotlight's BLUE light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void BlueDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 2, NormalizeValue(val));
	}

	/// <summary>
	/// Normalizes a 0 to 100 value into a nice 0 to 255 for DMX.
	/// </summary>
	/// <param name="val">Value from 0 to 100.</param>
	/// <returns>Value from 0 to 255.</returns>
	private int NormalizeValue(float val) {
		if (val >= 100) {
			return 255;
		} else if (val <= 0) {
			return 0;
		}

		return (int)(val * 2.55);
	}
}
