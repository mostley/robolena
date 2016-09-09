using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;

[Serializable]
public class StringChangedEvent : UnityEvent<string>
{
}

public class InputTrigger : MonoBehaviour {
	public InputField input;
	public StringChangedEvent onTrigger = new StringChangedEvent();

	public void Trigger() {
		onTrigger.Invoke (input.text);
	}
}
