using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundLibrary : MonoBehaviour
{
	protected new AudioSource audio;
	private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

	public void Awake()
	{
		this.audio = this.GetComponent<AudioSource>();
		foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Audio/" + this.GetAudioFolderName()))
			this.clips.Add(clip.name, clip);
	}

	protected abstract string GetAudioFolderName();

	public void Play(string clipName)
	{
		this.audio.clip = this.clips[clipName];
		this.audio.Play();
	}

}
