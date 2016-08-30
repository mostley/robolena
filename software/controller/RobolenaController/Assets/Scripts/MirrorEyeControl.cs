using UnityEngine;
using System.Collections;

public class MirrorEyeControl : MonoBehaviour {
	public DataController dataController;
	public EyePanelControl targetEyePanel;
	public bool leftToRight;

	public void Trigger() {
		bool[] dots;
		if (leftToRight) {
			dots = MirrorDots (dataController.leftDots);
			dataController.SetRightDots (dots);
		} else {
			dots = MirrorDots (dataController.rightDots);
			dataController.SetLeftDots (dots);
		}

		targetEyePanel.SetDots (dots);
	}

	public bool[] MirrorDots(bool[] dots) {
		bool[] result = new bool[dots.Length];

		for (int i = 0; i < dots.Length; i++) {
			int x = i % 5;
			int y = (int)Mathf.Floor (i / 5);
			result [i] = dots[(4 - x) + y * 5];
		}

		return result;
	}
}
