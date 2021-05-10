using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

/// <summary>
/// Controls the movement of a <see cref="GameObject"/> using the microphone
/// hub controller that we've designed.
/// </summary>
/// <inheritdoc/>
public class MicrophoneHubMover : IMover {
	public SerialPort serial;
	public string port = "COM3";
	private float front = 0;
	private float back = 0;
	private float left = 0;
	private float right = 0;

	public MicrophoneHubMover() {
		Debug.Log("Initializing serial port " + port);
		serial = new SerialPort(port, 9600);
		serial.Open();
		Debug.Log("Serial connection estabilished.");
	}

	public void PollDevice() {
		string data = serial.ReadExisting();

		if (data != string.Empty) {
			data = data.Replace("\n", "").Replace("\r", "");
			string[] values = data.Split(',');

			front = float.Parse(values[0]) / 100;
			back = float.Parse(values[1]) / 100;
			left = float.Parse(values[2]) / 100;
			right = float.Parse(values[3]) / 100;
		}
	}

	public float MoveBackward() {
		return back;
	}

	public float MoveForward() {
		return front;
	}

	public float MoveLeft() {
		return left;
	}

	public float MoveRight() {
		return right;
	}
}
