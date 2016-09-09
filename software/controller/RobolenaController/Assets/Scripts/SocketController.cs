using UnityEngine;
using System.Collections;

public class SocketController : MonoBehaviour {

	MeshRenderer[] renderers = new MeshRenderer[25];

	void Start () {
		var allRenderers = this.GetComponentsInChildren<MeshRenderer> ();
		for (int i = 1; i < allRenderers.Length; i++) {
			this.renderers [i - 1] = allRenderers [i];
		}
	}

	public void SetDots(bool[] dots) {
		Debug.Log ("SetDots " + dots);
		for (int i = 0; i < dots.Length; i++) {
			if (dots [i]) {
				this.renderers [i].materials [0].color = Color.red;
			} else {
				this.renderers [i].materials [0].color = Color.white;
			}
		}
	}
}
