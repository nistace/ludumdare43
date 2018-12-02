using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GameOverPanelManager : MonoBehaviour
{
	public Text gameOverReasonText;
	public Text milesText;
	private Animator anim;

	public void Awake()
	{
		this.anim = this.GetComponent<Animator>();
	}

	void Start()
	{
		GameController.Instance.OnGameOver += this.HandleGameOver;
	}

	private void HandleGameOver(string reason, float milesTraveled)
	{
		this.gameOverReasonText.text = reason;
		this.milesText.text = "You traveled " + milesTraveled.Display() + " miles";
		this.anim.SetTrigger("GameOver");
	}

	public void EndGame()
	{
		UISoundManager.Instance.PlayOk();
		Time.timeScale = 1;
		App.Instance.GotoMenu();
	}

}
