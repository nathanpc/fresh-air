using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using static SerialComms;
using System;

/// <summary>
/// Controls DMX devices using a MintyDMX controller.
/// </summary>
public class DMXController : MonoBehaviour {
	public SerialPort serial;
	public string port;
	public BaudRate baudRate = BaudRate.B115200;
	private string controlLine = "";
	private string queuedCommand = "";
	private bool armed = false;

	// Start is called before the first frame update
	void Start() {
		Debug.Log("Initializing DMX controller serial port " + port);
		serial = new SerialPort(port, (int)baudRate);
		serial.Open();
		Debug.Log("DMX controller serial connection estabilished.");
		
		Disarm();
	}

	// Update is called once per frame
	void Update() {
		try {
			// Read whatever is in the serial buffer.
			controlLine += serial.ReadExisting();

			// Check if we have a newline in the buffer.
			if (IsReadyToSend() && controlLine.Contains(">")) {
				// IT'S GO TIME! Drop everything!
				controlLine = "";

				// Send the command if there is one.
				if (queuedCommand.Length > 0)
					serial.Write(queuedCommand);

				// Disarm and wait for the next command.
				Disarm();
			} else if (controlLine.Contains("\n")) {
				// Split the lines and get the values from the last line received.
				string[] lines = controlLine.Split('\n');

				// Check if we got any errors from the controller.
				if (controlLine.Contains("ERROR")) {
					Debug.LogError(lines[0]);
				} else if (controlLine.Contains("OK")) {
					// Just ignore the OK replies.
				} else {
					Debug.Log(lines[0]);
				}

				// Put the rest of the data back into our own buffer.
				controlLine = controlLine.Substring(controlLine.IndexOf('\n') + 1);

				// Disarm and wait for the next command.
				Disarm();
			}
		} catch (TimeoutException) {
			// Just ignore the serial read timeout. It just means we didn't receive anything this time.
		}
	}

	// Application is about to quit
	void OnApplicationQuit() {
		// Close the serial port.
		if (serial.IsOpen)
			serial.Close();
	}

	/// <summary>
	/// Sets the output state of an auxiliary channel.
	/// </summary>
	/// <param name="channel">Auxiliary port channel.</param>
	/// <param name="value">Value to set it to.</param>
	public void SetAuxOutput(int channel, int value) {
		QueueCommand("ASO " + channel + " " + value);
	}

	/// <summary>
	/// Sets the value of a DMX slot given by an address on the network.
	/// </summary>
	/// <param name="address">DMX address.</param>
	/// <param name="value">Value to set it to.</param>
	public void SetDMXValueAtAddress(int address, int value) {
		QueueCommand("DSVA " + address + " " + value);
	}

	/// <summary>
	/// Queues a command to be sent to the unit.
	/// </summary>
	/// <param name="command">Command to be sent.</param>
	public void QueueCommand(string command) {
		queuedCommand = command + "\r\n";
		Arm();
	}

	/// <summary>
	/// Arm the unit to send commands to.
	/// </summary>
	private void Arm() {
		// Poke the bear.
		if (!IsReadyToSend())
			serial.Write("<");

		armed = true;
	}

	/// <summary>
	/// Disarm the unit to block commands from being sent.
	/// </summary>
	/// <param name="clearQueue">Should we clear the queued command?</param>
	private void Disarm(bool clearQueue = true) {
		armed = false;

		// Clear the queue.
		if (clearQueue)
			queuedCommand = "";
	}

	/// <summary>
	/// Is the unit ready to receive commands?
	/// </summary>
	/// <returns>True if it's ready to receive a single command.</returns>
	private bool IsReadyToSend() {
		return armed;
	}

	/// <summary>
	/// Checks if the serial port has been opened already.
	/// </summary>
	/// <returns>Is the serial port open?</returns>
	public bool IsSerialOpen() {
		return serial.IsOpen;
	}
}
