using UnityEngine;
using System.Collections;

public class EyeBrowController : MonoBehaviour {

	public void SetRotation(float angle)
	{
		var euler = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(euler.x, euler.y, angle);
	}
}
