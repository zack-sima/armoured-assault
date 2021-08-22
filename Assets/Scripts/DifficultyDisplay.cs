using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyDisplay : MonoBehaviour {
	void Start () {
		
	}
	string translateEnglishIntoLanguage (string text) {
		if (PlayerPrefs.GetString ("language") != "chinese") {
			return text;
		} else if (text == "Boring") {
			return "无聊";
		} else if (text == "Rookie") {
			return "新手";
		} else if (text == "Casual") {
			return "简单";
		} else if (text == "Normal") {
			return "正常";
		} else if (text == "Medium") {
			return "中等";
		} else if (text == "Difficult") {
			return "较难";
		} else if (text == "Hardened") {
			return "困难";
		} else if (text == "Hardcore") {
			return "艰难";
		} else if (text == "Extreme") {
			return "巅峰";
		} else if (text == "Impossible") {
			return "不可完成";
		} else {
			return "text";
		}
	}
	void Update () {
		float r = PlayerPrefs.GetInt ("difficulty") * 50;
		float g = 250 - (PlayerPrefs.GetInt("difficulty") - 5) * 50;
		if (r < 0) {
			r = 0;
		}
		if (g < 0) {
			g = 0;
		}
		if (r > 250) {
			r = 250;
		}
		if (g > 250) {
			g = 250;
		}
		r /= 250;
		g /= 250;
		GetComponent<Text> ().color = new Color (r, g, 0);
		string difficultyText = "";
		if (PlayerPrefs.GetInt("difficulty") == 1) {
			difficultyText = translateEnglishIntoLanguage("Boring");
		} else if (PlayerPrefs.GetInt("difficulty") == 2) {
			difficultyText = translateEnglishIntoLanguage("Rookie");
		} else if (PlayerPrefs.GetInt("difficulty") == 3) {
			difficultyText = translateEnglishIntoLanguage("Casual");
		} else if (PlayerPrefs.GetInt("difficulty") == 4) {
			difficultyText = translateEnglishIntoLanguage("Normal");
		} else if (PlayerPrefs.GetInt("difficulty") == 5) {
			difficultyText = translateEnglishIntoLanguage("Medium");
		} else if (PlayerPrefs.GetInt("difficulty") == 6) {
			difficultyText = translateEnglishIntoLanguage("Difficult");
		} else if (PlayerPrefs.GetInt("difficulty") == 7) {
			difficultyText = translateEnglishIntoLanguage("Hardened");
		} else if (PlayerPrefs.GetInt("difficulty") == 8) {
			difficultyText = translateEnglishIntoLanguage("Hardcore");
		} else if (PlayerPrefs.GetInt("difficulty") == 9) {
			difficultyText = translateEnglishIntoLanguage("Extreme");
		} else if (PlayerPrefs.GetInt("difficulty") == 10) {
			difficultyText = translateEnglishIntoLanguage("Impossible");
		}
		if (PlayerPrefs.GetString ("language") != "chinese") {
			GetComponent<Text> ().text = "Difficulty: " + difficultyText;
		} else {
			GetComponent<Text> ().text = "难度：" + difficultyText;
		}
	}
}
