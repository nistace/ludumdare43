using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class WaveResultPanelManager : MonoBehaviour
{

	public PersonPanel[] personPanels;
	public PersonPanel playerPanel;
	public CarPanel carPanel;
	public Text waveTitle;
	private Animator anim;

	public void Awake()
	{
		this.anim = this.GetComponent<Animator>();
		GameController.Instance.OnGameDataChanged += this.UpdateContent;
		GameController.Instance.OnWaveFinished += this.Show;
	}

	private void Show()
	{
		this.anim.SetBool("Visible", true);
	}

	public void Hide()
	{
		this.anim.SetBool("Visible", false);
	}

	private void UpdateContent()
	{
		this.waveTitle.text = "Wave " + GameController.Instance.Wave + " cleared !";
		this.playerPanel.UpdateCharacter(-1, GameController.Instance.player);
		this.carPanel.UpdateContent();
		int i = 0;
		while (i < GameController.Instance.otherCharacters.Length)
		{
			this.personPanels[i].UpdateCharacter(i, GameController.Instance.otherCharacters[i]);
			i++;
		}
		while (i < this.personPanels.Length)
		{
			this.personPanels[i].UpdateCharacter(-1, null);
			i++;
		}
	}
}
