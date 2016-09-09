using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class OrientationChangedEvent : UnityEvent<Vector3, Vector3>
{
}

[Serializable]
public class FloatChangedEvent : UnityEvent<float>
{
}

[Serializable]
public class DotsChangedEvent : UnityEvent<bool[]>
{
}

[Serializable]
public class AnimationChangedEvent : UnityEvent<RobotData[]>
{
}

[Serializable ()]
public class RobotData : ISerializable
{
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public float leftEyeBrowAngle = 0;
	public float rightEyeBrowAngle = 0;

	public bool[] leftDots = new bool[25];
	public bool[] rightDots = new bool[25];

	public RobotData ()
	{
	}

	public RobotData (RobotData data)
	{
		this.position = data.position;
		this.rotation = data.rotation;
		this.leftEyeBrowAngle = data.leftEyeBrowAngle;
		this.rightEyeBrowAngle = data.rightEyeBrowAngle;
		this.leftDots = (bool[])data.leftDots.Clone();
		this.rightDots = (bool[])data.rightDots.Clone();
	}

	public RobotData (SerializationInfo info, StreamingContext ctxt)
	{
		position = (Vector3)info.GetValue("position", typeof(Vector3));
		rotation = (Vector3)info.GetValue("rotation", typeof(Vector3));
		leftEyeBrowAngle = (float)info.GetValue("leftEyeBrowAngle", typeof(float));
		rightEyeBrowAngle = (float)info.GetValue("rightEyeBrowAngle", typeof(float));
		leftDots = (bool[])info.GetValue("leftDots", typeof(bool[]));
		rightDots = (bool[])info.GetValue("rightDots", typeof(bool[]));
	}

	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("position", position);
		info.AddValue("rotation", rotation);
		info.AddValue("leftEyeBrowAngle", leftEyeBrowAngle);
		info.AddValue("rightEyeBrowAngle", rightEyeBrowAngle);
		info.AddValue("leftDots", leftDots);
		info.AddValue("rightDots", rightDots);
	}
}

[Serializable] public struct RobotDataArrayWrapper { public RobotData[] data; }

public class DataController : MonoBehaviour
{
	public List<RobotData> animationData = new List<RobotData>();
	public RobotData data;

	public float translationSpeed = 0.1f;
	public float rotationSpeed = 5f;
	public float eyeBrowRotationSpeed = 5f;

	public OrientationChangedEvent OnOrientationChange = new OrientationChangedEvent ();
	public FloatChangedEvent OnLeftEyebrowChange = new FloatChangedEvent ();
	public FloatChangedEvent OnRightEyebrowChange = new FloatChangedEvent ();
	public DotsChangedEvent OnLeftDotsChanged = new DotsChangedEvent ();
	public DotsChangedEvent OnRightDotsChanged = new DotsChangedEvent ();

	public AnimationChangedEvent OnAnimationDataChanged = new AnimationChangedEvent ();

	void Start() {
		this.data = new RobotData ();
		animationData.Add (this.data);
		this.OnAnimationDataChanged.Invoke (this.animationData.ToArray ());
	}

	public void Translate(Vector3 translation) {
		data.position += translation * translationSpeed;
		OnOrientationChange.Invoke (data.position, data.rotation);
	}

	public void Rotate(Vector3 rot) {
		data.rotation += rot * rotationSpeed;
		OnOrientationChange.Invoke (data.position, data.rotation);
	}

	public void RotateLeftEyebrow(float rot) {
		data.leftEyeBrowAngle += rot * eyeBrowRotationSpeed;
		OnLeftEyebrowChange.Invoke (data.leftEyeBrowAngle);
	}

	public void RotateRightEyebrow(float rot) {
		data.rightEyeBrowAngle += rot * eyeBrowRotationSpeed;
		OnRightEyebrowChange.Invoke (data.rightEyeBrowAngle);
	}

	public void SetLeftDot(int index, bool value) {
		data.leftDots [index] = value;
		OnLeftDotsChanged.Invoke (data.leftDots);
	}

	public void SetRightDot(int index, bool value) {
		data.rightDots [index] = value;
		OnRightDotsChanged.Invoke (data.rightDots);
	}

	public void SetLeftDots(bool[] value) {
		data.leftDots = value;
		OnLeftDotsChanged.Invoke (data.leftDots);
	}

	public void SetRightDots(bool[] value) {
		data.rightDots = value;
		OnRightDotsChanged.Invoke (data.rightDots);
	}

	public void Save(string fileName) {
		try {
			var wrapper = new RobotDataArrayWrapper{
				data = this.animationData.ToArray()
			};

			string json = JsonUtility.ToJson(wrapper);
			System.IO.File.WriteAllText (fileName, json);
			Debug.Log ("Saved '" + json + "' to '" + fileName + "'");
		} catch (Exception ex) {
			Debug.LogError(ex.Message);
		}
	}

	public void Load(string fileName) {
		try {
			var json = System.IO.File.ReadAllText (fileName);
			var wrapper = JsonUtility.FromJson<RobotDataArrayWrapper>(json);
			this.animationData = new List<RobotData>(wrapper.data);
		} catch (Exception ex) {
			Debug.LogError(ex.Message);
		}

		if (this.animationData.Count > 0) {
			this.data = this.animationData [0];
		} else {
			this.data = new RobotData ();
		}

		EmitChange ();
		this.OnAnimationDataChanged.Invoke (this.animationData.ToArray ());
	}

	public void EmitChange() {
		OnOrientationChange.Invoke (data.position, data.rotation);
		OnLeftEyebrowChange.Invoke (data.leftEyeBrowAngle);
		OnRightEyebrowChange.Invoke (data.rightEyeBrowAngle);
		OnLeftDotsChanged.Invoke (data.leftDots);
		OnRightDotsChanged.Invoke (data.rightDots);
	}

	public void Reset() {
		this.data = new RobotData ();
		this.animationData = new List<RobotData> ();
		this.animationData.Add (data);
		this.EmitChange ();
		this.OnAnimationDataChanged.Invoke (this.animationData.ToArray ());
	}

	public RobotData AddFrame()
	{
		var frame = new RobotData (this.data);
		this.animationData.Add (frame);
		this.SetActiveFrame (frame);
		return frame;
	}

	public void RemoveFrame(RobotData frame)
	{
		if (this.animationData.Count <= 1) {
			return;
		}

		var index = this.animationData.IndexOf (frame);
		this.animationData.Remove (frame);
		if (frame == this.data) {
			index = index - 1;
			if (index < 0) {
				index = 0;
			}
			if (index >= this.animationData.Count) {
				index = this.animationData.Count-1;
			}
			SetActiveFrame (this.animationData [index]);
		}
		this.OnAnimationDataChanged.Invoke (this.animationData.ToArray ());
	}

	public void SetActiveFrame(RobotData frame) {
		this.data = frame;
		this.EmitChange ();
	}
}
