using System;
using System.IO.Ports;
using UnityEngine;
using System.Collections;

public class SerialConnector : MonoBehaviour {

	public string device = "/dev/cu.usbmodem1421";
	public int baudrate = 57600;
	
	Vector3 position;
	Vector3 rotation;
	SerialPort port;

	void Start () {

		//Debug.Log (string.Join(", ", SerialPort.GetPortNames ()));

		port = new SerialPort(device, baudrate);
		port.ReadTimeout = 500;
		port.WriteTimeout = 500;
	}

	void Update() {
		/*if (port.IsOpen) {
			if (port.BytesToRead >= 11) {
				var data = port.ReadExisting ();
				if (!string.IsNullOrEmpty (data)) {
					Debug.Log ("received: '" + data + "'");
				}
			}
			port.BaseStream.Flush ();
		}*/
	}

	void OnApplicationQuit() {
		if (port.IsOpen) {
			port.Close ();
		}
	}

	void Send (string text) {
		if (port.IsOpen) {
			Debug.Log ("Sending: '" + text + "'");
			port.WriteLine (text );
		}
	}
	
	public void Connect () {
		port.Open ();
		
		if (port.IsOpen) {
			Debug.Log ("Connection is open");
		} else {
			Debug.LogError ("Connection is not open");
		}
	}
	
	public void Disconnect () {
		port.Close ();

		if (!port.IsOpen) {
			Debug.Log ("Connection is closed");
		} else {
			Debug.LogError ("Connection is not closed");
		}
	}

	public void MoveUp () {
		position += Vector3.up;
		Send ("G00 Z " + position.y + ";");
	}
	
	public void MoveDown () {
		position += Vector3.down;
		Send ("G00 Z " + position.y + ";");
	}
	
	public void MoveLeft () {
		position += Vector3.left;
		Send ("G00 X " + position.x + ";");
	}
	
	public void MoveRight () {
		position += Vector3.right;
		Send ("G00 X " + position.x + ";");
	}
	
	public void MoveForward () {
		position += Vector3.forward;
		Send ("G00 Z " + position.z + ";");
	}

	public void MoveBackward () {
		position += Vector3.back;
		Send ("G00 Z " + position.z + ";");
	}
	
	public void YawLeft () {
		rotation += Vector3.left;
		Send ("G00 A " + position.x + ";");
	}

	public void YawRight () {
		rotation += Vector3.right;
		Send ("G00 A " + position.x + ";");
	}
	
	public void RollLeft () {
		rotation += Vector3.up;
		Send ("G00 B " + position.y + ";");
	}
	
	public void RollRight () {
		rotation += Vector3.down;
		Send ("G00 B " + position.y + ";");
	}
	
	public void PitchLeft () {
		rotation += Vector3.forward;
		Send ("G00 C " + position.z + ";");
	}
	
	public void PitchRight () {
		rotation += Vector3.back;
		Send ("G00 C " + position.z + ";");
	}
}
