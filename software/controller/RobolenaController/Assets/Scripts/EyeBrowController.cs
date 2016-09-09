using UnityEngine;
using System.Collections;

public class EyeBrowController : MonoBehaviour {

	public void SetRotation(float angle)
	{
		var euler = transform.localEulerAngles;
		transform.localEulerAngles = new Vector3(euler.x, euler.y, angle);
	}
}
