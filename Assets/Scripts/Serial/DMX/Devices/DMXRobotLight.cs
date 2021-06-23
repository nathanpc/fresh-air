using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXRobotLight : MonoBehaviour {
	public DMXController controller;
	public int baseAddress = 4;
	private int startupCodes = 0;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		// Send the startup code a couple of times just to make sure the robot got the message.
		if (controller.IsSerialOpen() && (startupCodes < 3)) {
			MasterOn();
			startupCodes++;
		}
	}

	/// <summary>
	/// Controls the robot's X position.
	/// </summary>
	/// <param name="angle">Angle of rotation. 0 degrees start at right and rotates counter-clockwise.</param>
	public void RotateX(float angle) {
		controller.SetDMXValueAtAddress(baseAddress, NormalizeAngle(angle));
	}

	/// <summary>
	/// Controls the robot's Y position.
	/// </summary>
	/// <param name="angle">Angle of rotation from 0 to 180.</param>
	public void RotateY(float angle) {
		controller.SetDMXValueAtAddress(baseAddress + 1, NormalizeValue(angle / 1.8f));
	}

	/// <summary>
	/// Enables all of the features of the robot.
	/// </summary>
	public void MasterOn() {
		controller.SetDMXValueAtAddress(baseAddress + 2, 255);
	}

	/// <summary>
	/// Dimmers the spotlight's RED light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void RedDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 3, NormalizeValue(val));
	}

	/// <summary>
	/// Dimmers the spotlight's GREEN light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void GreenDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 4, NormalizeValue(val));
	}

	/// <summary>
	/// Dimmers the spotlight's BLUE light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void BlueDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 5, NormalizeValue(val));
	}

	/// <summary>
	/// Dimmers the spotlight's WHITE light.
	/// </summary>
	/// <param name="val">Amount of light from 0 to 100.</param>
	public void WhiteDimmer(float val) {
		controller.SetDMXValueAtAddress(baseAddress + 6, NormalizeValue(val));
	}

	/// <summary>
	/// Changes the speed of the robot's motor.
	/// </summary>
	/// <param name="val">Amount of speed from 0 to 100.</param>
	public void RobotSpeed(float val) {
		// This value should be inverted as per the manual. So 0 is faster and 255 is slower.
		controller.SetDMXValueAtAddress(baseAddress + 7, 255 - NormalizeValue(val));
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

	/// <summary>
	/// Normalizes an integer angle from 0 to 360 into a value of 0 to 165 in DMX.
	/// </summary>
	/// <param name="angle">Rotation angle. Keep in mind that 0 points to the right
	/// and the rotation is counter-clockwise</param>
	/// <returns>DMX value from 0 to 165.</returns>
	private int NormalizeAngle(float angle) {
		return (int)(angle * 165 / 360);
	}
}
