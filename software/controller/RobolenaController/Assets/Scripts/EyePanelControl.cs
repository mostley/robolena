using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;


[System.Serializable]
public class DotChangedEvent : UnityEvent<int, bool> { }

public class EyePanelControl : MonoBehaviour {
	Toggle[] toggles;

	public DotChangedEvent OnDotChanged = new DotChangedEvent();

	void Start () {
		this.toggles = this.GetComponentsInChildren<Toggle> ();
		for (int i = 0; i < this.toggles.Length; i++) {
			this.toggles [i].isOn = false;
			this.toggles[i].onValueChanged.AddListener (new EyeDotControl (i, OnDotChanged).OnToggle);
		}
	}

	public void SetDots (bool[] dots)
	{
		for (int i = 0; i < this.toggles.Length; i++) {
			this.toggles [i].isOn = dots[i];
		}
	}
}

public class EyeDotControl {
	public int index;
	public DotChangedEvent dotChangedEvent;

	public EyeDotControl(int index, DotChangedEvent dotChangedEvent) {
		this.index = index;
		this.dotChangedEvent = dotChangedEvent;
	}

	public void OnToggle(bool value) {
		this.dotChangedEvent.Invoke(this.index, value);
	}
}