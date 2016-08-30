using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class OrientationChangedEvent : UnityEvent<Vector3, Vector3>
{
}

[System.Serializable]
public class FloatChangedEvent : UnityEvent<float>
{
}

[System.Serializable]
public class DotsChangedEvent : UnityEvent<bool[]>
{
}

public class DataController : MonoBehaviour
{
	public Vector3 position = Vector3.zero;
	public Vector3 rotation = Vector3.zero;
	public float leftEyeBrowAngle = 0;
	public float rightEyeBrowAngle = 0;

	public float translationSpeed = 0.1f;
	public float rotationSpeed = 5f;
	public float eyeBrowRotationSpeed = 5f;

	public bool[] leftDots = new bool[25];
	public bool[] rightDots = new bool[25];

	public OrientationChangedEvent OnOrientationChange = new OrientationChangedEvent ();

	public FloatChangedEvent OnLeftEyebrowChange = new FloatChangedEvent ();
	public FloatChangedEvent OnRightEyebrowChange = new FloatChangedEvent ();
	public DotsChangedEvent OnLeftDotsChanged = new DotsChangedEvent ();
	public DotsChangedEvent OnRightDotsChanged = new DotsChangedEvent ();

	void Start() {
	}

	public void Translate(Vector3 translation) {
		position += translation * translationSpeed;
		OnOrientationChange.Invoke (position, rotation);
	}

	public void Rotate(Vector3 rot) {
		rotation += rot * rotationSpeed;
		OnOrientationChange.Invoke (position, rotation);
	}

	public void RotateLeftEyebrow(float rot) {
		leftEyeBrowAngle += rot * eyeBrowRotationSpeed;
		OnLeftEyebrowChange.Invoke (leftEyeBrowAngle);
	}

	public void RotateRightEyebrow(float rot) {
		rightEyeBrowAngle += rot * eyeBrowRotationSpeed;
		OnRightEyebrowChange.Invoke (rightEyeBrowAngle);
	}

	public void SetLeftDot(int index, bool value) {
		this.leftDots [index] = value;
		OnLeftDotsChanged.Invoke (this.leftDots);
	}

	public void SetRightDot(int index, bool value) {
		this.rightDots [index] = value;
		OnRightDotsChanged.Invoke (this.rightDots);
	}

	public void SetLeftDots(bool[] value) {
		this.leftDots = value;
		OnLeftDotsChanged.Invoke (this.leftDots);
	}

	public void SetRightDots(bool[] value) {
		this.rightDots = value;
		OnRightDotsChanged.Invoke (this.rightDots);
	}
}
