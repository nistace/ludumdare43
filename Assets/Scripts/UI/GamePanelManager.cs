using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelManager : MonoBehaviour
{

	public Text carHealthText;
	public Text playerHealthText;
	public Text waveCount;
	public Text milesText;
	public Text timerText;
	public Text ShootToStartText;

	void Start()
	{
		GameController.Instance.car.OnHealthChanged += this.UpdateCarHealth;
		GameController.Instance.player.OnHealthChanged += this.UpdatePlayerHealth;
		GameController.Instance.OnMilesChanged += this.UpdateMiles;
		GameController.Instance.OnWaveCountChanged += this.UpdateWaveCount;
		GameController.Instance.OnWaveRemainingTimeChanged += this.UpdateWaveRemainingTime;
		GameController.Instance.OnGotoNextHalt += this.ShowShootToStartText;
		GameController.Instance.OnLaunchWave += this.HideShootToStartText;
		this.UpdateCarHealth(GameController.Instance.car.Health);
		this.UpdatePlayerHealth(GameController.Instance.player.Health);
		this.UpdateWaveCount(GameController.Instance.Wave);
		this.UpdateMiles(GameController.Instance.Miles);
		this.UpdateWaveRemainingTime(GameController.Instance.WaveRemainingTime);
	}

	private void ShowShootToStartText()
	{
		this.ShootToStartText.gameObject.SetActive(true);
	}

	private void HideShootToStartText()
	{
		this.ShootToStartText.gameObject.SetActive(false);
	}

	private void UpdateCarHealth(float value)
	{
		this.carHealthText.text = value.Display();
	}

	private void UpdatePlayerHealth(float value)
	{
		this.playerHealthText.text = value.Display();
	}

	private void UpdateWaveCount(int value)
	{
		this.waveCount.text = value.ToString();
	}

	private void UpdateMiles(float value)
	{
		this.milesText.text = value.Display();
	}

	private void UpdateWaveRemainingTime(float value)
	{
		this.timerText.enabled = value >= 0;
		this.timerText.text = value.Display(3);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
