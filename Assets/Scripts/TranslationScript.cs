using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslationScript : MonoBehaviour {
	public string myEnglishText;
	void Start () { 
		if (PlayerPrefs.GetString("language") == "chinese") {
			if (myEnglishText == "Start") {
				GetComponent<Text> ().text = "开始";
			} else if (myEnglishText == "Back") {
				GetComponent<Text> ().text = "返回";
			}
		} else {
			if (myEnglishText == "Start") {
				GetComponent<Text> ().text = "Start";
			} else if (myEnglishText == "Back") {
				GetComponent<Text> ().text = "Back";
			}
		}
	}
}
