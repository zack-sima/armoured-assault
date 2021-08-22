using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionScript : MonoBehaviour {
	public GameObject[] maps;
	float originalScreenX = 1290;
	float scale;
	void Awake () {  
		if (PlayerPrefs.GetInt ("difficulty") == 0) {
			PlayerPrefs.SetInt ("difficulty", 3);
		}
		scale = Screen.width / originalScreenX;
		if (gameObject.GetComponent<Text> () != null) {
			gameObject.GetComponent<Text> ().fontSize = (int)(gameObject.GetComponent<Text> ().fontSize * scale);
		} else {
			GetComponent<RectTransform>().anchoredPosition = new Vector2 (GetComponent<RectTransform>().anchoredPosition.x * scale, GetComponent<RectTransform>().anchoredPosition.y * scale);
			GetComponent<RectTransform>().sizeDelta = new Vector2 (GetComponent<RectTransform>().rect.width * scale, GetComponent<RectTransform>().rect.height * scale);
		}
	}
	public void loadGame (int mapCount) {
		PlayerPrefs.SetInt ("map", mapCount);
		Time.timeScale = 1;
		Application.LoadLevel (2);
	}
	public void changeLanguage (string lang) {
		PlayerPrefs.SetString ("language", lang);
	}
	public void changeDifficulty (bool add) {
		if (add) {
			if (PlayerPrefs.GetInt("difficulty") < 10) {
				PlayerPrefs.SetInt ("difficulty", PlayerPrefs.GetInt("difficulty") + 1);
			}
		} else {
			if (PlayerPrefs.GetInt("difficulty") > 1) {
				PlayerPrefs.SetInt ("difficulty", PlayerPrefs.GetInt("difficulty") - 1);
			}
		}
	}
	public void changeScene (int sceneIndex) {
		Application.LoadLevel (sceneIndex);
	}
	public void moveMaps (bool moveRight) {
		if (maps.Length > 0) {
			if (moveRight) {
				if (maps[maps.Length - 1].transform.position.x > Screen.width / 2) {
					foreach (GameObject i in maps) {
						i.transform.Translate (new Vector3(-300, 0, 0), Space.Self);
					}
				}
			} else {
				if (maps[0].transform.position.x < Screen.width / 2) {
					foreach (GameObject i in maps) {
						i.transform.Translate (new Vector3(300, 0, 0), Space.Self);
					}
				}
			}
		}
	}
}
