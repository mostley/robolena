using UnityEngine;
using System.Collections;

public class TranslationTrigger : MonoBehaviour {
	public DataController dataController;
	public Vector3 translation;

	public void Trigger() {
		dataController.Translate (translation);
	}
}
