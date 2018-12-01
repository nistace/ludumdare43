using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPanel : MonoBehaviour
{
	public Color colorOnOk = Color.black;
	public Color colorOnKo = new Color(.5f, 0, 0);
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
		this.capacityText.color = totalStored <= GameController.Instance.GetStorageCapacity() ? this.colorOnOk : this.colorOnKo;
		this.foodText.text = GameController.Instance.rations.ToString();
		this.fuelText.text = GameController.Instance.fuel.ToString();
		this.foodLeftBehindText.text = GameController.Instance.rationsLeftBehind.ToString();
		this.fuelLeftBehindText.text = GameController.Instance.fuelLeftBehind.ToString();
		this.foodLootedText.text = GameController.Instance.lootedRations.ToString();
		this.fuelLootedText.text = GameController.Instance.lootedFuel.ToString();
	}

	public void LetFood()
	{
		GameController.Instance.LetRation();
	}

	public void LetFuel()
	{
		GameController.Instance.LetFuel();
	}

	public void TakeFood()
	{
		GameController.Instance.TakeRation();
	}

	public void TakeFuel()
	{
		GameController.Instance.TakeFuel();
	}
}
