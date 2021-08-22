using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplayScript : MonoBehaviour {
	public Sprite[] englishTutorialArray;
	public Sprite[] chineseTutorialArray;

	public int index = 0;
	void Start () {
		restartImages ();
	}
	public void restartImages () {
		index = 0;
		if (PlayerPrefs.GetString ("language") == "chinese") {
			GetComponent<Image> ().sprite = chineseTutorialArray [0];
		} else {
			GetComponent<Image> ().sprite = englishTutorialArray [0];
		}
	}
	public void addIndexCount () {
		index++;
		if (englishTutorialArray.Length > index) {
			if (PlayerPrefs.GetString ("language") == "chinese") {
				GetComponent<Image> ().sprite = chineseTutorialArray [index];
			} else {
				GetComponent<Image> ().sprite = englishTutorialArray [index];
			}
		} else {
			move (false);
			restartImages ();
		}
	}
	public void move (bool up) {
		if (up) {
			transform.parent.Translate (new Vector3(0, 6000, 0), Space.Self);
		} else
			transform.parent.Translate (new Vector3(0, -6000, 0), Space.Self);
	}
}
