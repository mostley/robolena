using UnityEngine;
using System.Collections;

public class FloatTrigger : MonoBehaviour {
	public float rotation = 0f;

	public FloatChangedEvent onTrigger = new FloatChangedEvent();

	public void Trigger() {
		onTrigger.Invoke (rotation);
	}
}
