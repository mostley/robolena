using UnityEngine;
using System.Collections;

public class HeadOrientationController : MonoBehaviour {

	public void UpdateOrientation(Vector3 position, Vector3 rotation) {
		transform.localPosition = position;
		transform.rotation = Quaternion.Euler (rotation);
	}
}
