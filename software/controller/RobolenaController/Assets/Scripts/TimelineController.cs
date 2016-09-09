using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class TimelineController : MonoBehaviour {
	public GameObject FramePrefab;
	public GameObject AddButtonPrefab;
	public Transform ContentContainer;

	public DataController dataController;

	public FrameToggleController currentFrameToggle;
	public List<FrameToggleController> toggles = new List<FrameToggleController>();

	private Button addButton;
	private bool ignoreNextChangeEvent;

	public void SetAnimationData(RobotData[] frames) {
		Debug.Log ("SetAnimationData " + frames.Length);
		this.RemoveAll ();

		for (int i = 0; i < frames.Length; i++) {
			var toggle = CreateToggleForFrame (frames [i]);
			this.Add (i, toggle);
		}

		this.addButton = this.CreateAddButton ();
		this.Add (frames.Length, this.addButton);

		this.currentFrameToggle = toggles[0];
		SetToggle(this.currentFrameToggle, true);
	}

	public FrameToggleController CreateToggleForFrame(RobotData frame)
	{
		Debug.Log ("CreateToggleForFrame " + frame);

		var result = (GameObject)Instantiate (FramePrefab, Vector3.zero, Quaternion.identity);
		var frameToggleController = result.GetComponent<FrameToggleController> ();
		SetToggle(frameToggleController, false);
		frameToggleController.FrameData = frame;
		frameToggleController.Remove = new UnityAction<RobotData>(this.OnRemoveFrame);

		var toggle = result.GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener (active => {
			this.OnToggleChanged(frameToggleController, active);
		});

		return frameToggleController;
	}

	public Button CreateAddButton()
	{
		Debug.Log ("CreateAddButton");

		var result = (GameObject)Instantiate (AddButtonPrefab, Vector3.zero, Quaternion.identity);
		var btn = result.GetComponent<Button> ();
		btn.onClick.AddListener (this.OnAddClicked);
		return btn;
	}

	public void RemoveAll()
	{
		Debug.Log ("RemoveAll");

		currentFrameToggle = null;
		ContentContainer.DetachChildren ();
		toggles.Clear ();
	}

	private void SetOffset(int index, RectTransform rectTransform) {
		rectTransform.anchorMin = new Vector2 (index * 0.12f, 0);
		rectTransform.anchorMax = new Vector2 (index * 0.12f + 0.12f, 1);
		rectTransform.offsetMin = new Vector2 (10, 10);
		rectTransform.offsetMax = new Vector2 (-10, -10);
		rectTransform.localScale = Vector3.one;
		rectTransform.localPosition = new Vector3 (rectTransform.localPosition.x, rectTransform.localPosition.y, 0);
	}

	public void Add(int index, GameObject obj)
	{
		Debug.Log ("CreateToggleForFrame " + index + " " + obj);

		obj.transform.SetParent (ContentContainer);
		var rectTransform = ((RectTransform)obj.transform);
		this.SetOffset (index, rectTransform);
	}

	public void Add(int index, FrameToggleController toggle)
	{
		this.Add (index, toggle.gameObject);

		toggles.Add (toggle);
	}

	public void Add(int index, Button btn)
	{
		this.Add (index, btn.gameObject);
	}

	public void OnRemoveFrame(RobotData frame)
	{
		dataController.RemoveFrame (frame);
	}

	private void OnToggleChanged(FrameToggleController frameToggle, bool active) {
		Debug.Log ("OnToggleChanged - active: " + active + " ignoreNextChangeEvent: " + ignoreNextChangeEvent);

		if (ignoreNextChangeEvent) { return; }

		if (active == true) {
			SetToggle(this.currentFrameToggle, false);

			this.currentFrameToggle = frameToggle;

			dataController.SetActiveFrame (frameToggle.FrameData);
		} else {
			SetToggle (frameToggle, true);
		}
	}

	private void OnAddClicked() {
		var frame = dataController.AddFrame ();
		var frameToggle = CreateToggleForFrame(frame);
		this.Add (toggles.Count, frameToggle);

		this.SetOffset (toggles.Count, ((RectTransform)this.addButton.transform));

		SetToggle(this.currentFrameToggle, false);
		this.currentFrameToggle = frameToggle;
		SetToggle(frameToggle, true);
	}

	private void SetToggle(FrameToggleController frameToggle, bool active)
	{
		ignoreNextChangeEvent = true;
		frameToggle.GetComponent<Toggle> ().isOn = active;
		ignoreNextChangeEvent = false;
	}
}
