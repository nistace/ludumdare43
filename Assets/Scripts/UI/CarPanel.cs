﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPanel : MonoBehaviour
{
	public Text capacityText;
	public Text foodText;
	public Text fuelText;
	public Text foodLeftBehindText;
	public Text fuelLeftBehindText;
	public Text foodLootedText;
	public Text fuelLootedText;

	public void UpdateContent()
	{
		int totalStored = GameController.Instance.rations + GameController.Instance.fuel;
		this.capacityText.text = totalStored + " / " + GameController.Instance.GetStorageCapacity();
		this.capacityText.color = totalStored > GameController.Instance.GetStorageCapacity() ? GameController.UiWarningTextColor : GameController.UiDefaultTextColor;
		this.capacityText.fontStyle = totalStored > GameController.Instance.GetStorageCapacity() ? FontStyle.Bold : FontStyle.Normal;
		this.foodText.text = GameController.Instance.rations.ToString();
		this.fuelText.text = GameController.Instance.fuel.ToString();
		this.foodLeftBehindText.text = GameController.Instance.rationsLeftBehind.ToString();
		this.fuelLeftBehindText.text = GameController.Instance.fuelLeftBehind.ToString();
		this.foodLootedText.text = GameController.Instance.lootedRations.ToString();
		this.fuelLootedText.text = GameController.Instance.lootedFuel.ToString();
	}

	public void LetFood()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.LetRation());
	}

	public void LetFuel()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.LetFuel());
	}

	public void TakeFood()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.TakeRation());
	}

	public void TakeFuel()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.TakeFuel());
	}
}
