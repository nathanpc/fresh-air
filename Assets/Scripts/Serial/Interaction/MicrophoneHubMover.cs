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
	public string port;
	private string controlLine = "";
	private float front = 0;
	private float back = 0;
	private float left = 0;
	private float right = 0;

	public MicrophoneHubMover(string serialPort) {
		port = serialPort;
		Debug.Log("Initializing microphone controller serial port " + port);
		serial = new SerialPort(port, 9600);
		serial.Open();
		Debug.Log("Microphone controller serial connection estabilished.");
	}

	public void PollDevice() {
		// Read whatever is in the serial buffer.
		controlLine += serial.ReadExisting();

		// Check if we have a newline in the buffer.
		if (controlLine.Contains("\n")) {
			// Split the lines and get the values from the last line received.
			string[] lines = controlLine.Split('\n');
			string[] values = lines[0].Replace("\r", "").Split(',');
			//Debug.Log(lines[0]);

			// Parse the values received.
			if (!float.TryParse(values[0], out front))
				front = 0;
			if (!float.TryParse(values[1], out back))
				back = 0;
			if (!float.TryParse(values[2], out left))
				left = 0;
			if (!float.TryParse(values[3], out right))
				right = 0;

			// Normalize parsed values.
			front /= 100;
			back /= 100;
			left /= 100;
			right /= 100;

			// Put the rest of the data back into our own buffer.
			controlLine = controlLine.Substring(controlLine.IndexOf('\n') + 1);
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

	public void OnQuit() {
		// Close the serial port.
		if (serial.IsOpen)
			serial.Close();
	}
}
