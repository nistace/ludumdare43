using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : SoundLibrary
{
	public static MusicManager Instance { get; private set; }
	internal UISoundManager uiSounds;

	public new void Awake()
	{
		if (Instance == null) Instance = this;
		if (Instance != this) Destroy(this.gameObject);
		else
		{
			base.Awake();
			this.uiSounds = this.GetComponentInChildren<UISoundManager>();
			DontDestroyOnLoad(this.gameObject);
		}
	}

	protected override string GetAudioFolderName()
	{
		return "Music";
	}

	public void PlayFast()
	{
		this.Play("Human Meat Is Very Nourishing - Fast");
		this.audio.loop = false;
	}

	public void PlaySlow()
	{
		this.Play("Human Meat Is Very Nourishing - Slow");
		this.audio.loop = true;
	}
}
