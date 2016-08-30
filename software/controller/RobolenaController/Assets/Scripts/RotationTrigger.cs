using UnityEngine;
using System.Collections;

public class RotationTrigger : MonoBehaviour {
	public DataController dataController;
	public Vector3 rotation = Vector3.zero;

	public void Trigger() {
		dataController.Rotate (rotation);
	}
}
