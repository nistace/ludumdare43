using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISoundManager : SoundLibrary
{
	public static UISoundManager Instance
	{
		get
		{
			if (MusicManager.Instance == null) return new UISoundManager();
			return MusicManager.Instance.uiSounds;
		}
	}

	protected override string GetAudioFolderName()
	{
		return "Sounds/UI";
	}

	public void PlayOk()
	{
		this.Play("ok");
	}

	public void PlayKo()
	{
		this.Play("ko");
	}

	public void PlayButtonSound(bool positive)
	{
		if (positive) this.PlayOk();
		else this.PlayKo();
	}

	internal void PlayTyping()
	{
		this.Play("typing" + UnityEngine.Random.Range(1, 3));
	}
}
