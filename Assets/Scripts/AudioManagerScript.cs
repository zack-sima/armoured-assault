using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound {
	public string name;
	public AudioClip clip;
	public AudioSource source;
}
public class AudioManagerScript : MonoBehaviour {
	public Sound[] sounds;
	void Awake () {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
		}
	}
	
	void Update () {
		
	}
	public void Play (string name, float volume) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		GetComponent<AudioSource>().PlayOneShot(s.clip, volume);
	}
}
