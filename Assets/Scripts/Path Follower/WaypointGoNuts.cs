using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGoNuts : WaypointBase {
	public DMXSpotlight spotlight;
	public DMXRobotLight robotLight;
	private int counter = -1;
	private float lightLow = 10;
	private float lightHigh = 99;
	private float angleLow = 0;
	private float angleXHigh = 360;
	private float angleYHigh = 180;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		// Do nothing if we have disabled the counter.
		if (counter < 0)
			return;

		// Count up.
		counter++;

		// Use a counter to delay each command.
		switch (counter) {
			case 1:
				spotlight.RedDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 5:
				spotlight.BlueDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 9:
				spotlight.GreenDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 13:
				robotLight.RedDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 17:
				robotLight.GreenDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 21:
				robotLight.BlueDimmer(Random.Range(lightLow, lightHigh));
				break;
			case 25:
				robotLight.RotateX(Random.Range(angleLow, angleXHigh));
				break;
			case 29:
				robotLight.RotateY(Random.Range(angleLow, angleYHigh));
				break;
		}

		// Reset the counter.
		if (counter > 33)
			counter = 0;
	}

	/// <summary>
	/// Let's just go nuts and have some fun!
	/// </summary>
	public void StartTheShow() {
		robotLight.MasterOn();
		counter = 0;
	}
}
