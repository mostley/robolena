using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FrameToggleController : MonoBehaviour {

	public RobotData FrameData;
	public UnityAction<RobotData> Remove;

	public void OnRemoveClicked()
	{
		Remove.Invoke (FrameData);
	}
}
