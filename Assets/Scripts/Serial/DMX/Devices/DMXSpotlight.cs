using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXSpotlight : MonoBehaviour {
	public DMXController controller;
	public int baseAddress = 1;
	private int counter = -1;
	private float r;
	private float g;
	private float b;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		LightColor(r, g, b);
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
	/// Lights up the spotlight with a specific color.
	/// </summary>
	/// <param name="red">Red value.</param>
	/// <param name="green">Green value.</param>
	/// <param name="blue">Blue value.</param>
	/// <param name="light">Actually turn on the light.</param>
	public void LightColor(float red, float green, float blue, bool light = false) {
		if (light)
			counter = 0;

		// Do nothing if we have disabled the counter.
		if (counter < 0)
			return;

		// Count up.
		counter++;

		// Use a counter to delay each command.
		switch (counter) {
			case 1:
				RedDimmer(red);
				break;
			case 5:
				BlueDimmer(blue);
				break;
			case 9:
				GreenDimmer(green);
				counter = -1;
				break;
		}
	}

	/// <summary>
	/// Light the spotlight purple.
	/// </summary>
	public void LightPurple() {
		LightColor(52, 0, 63, true);
	}

	/// <summary>
	/// Turn the spotlight OFF.
	/// </summary>
	public void LightsOff() {
		LightColor(0, 0, 0, true);
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
