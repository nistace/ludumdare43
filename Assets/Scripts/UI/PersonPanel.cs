using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonPanel : MonoBehaviour
{
	public int personIndex;
	private HumanCharacter character;

	public Image Pic;
	public Text nameText;
	public Text healthText;
	public Text hungerText;

	public void UpdateCharacter(int i, HumanCharacter character)
	{
		this.personIndex = i;
		this.character = character;
		this.UpdateCharacter();
	}

	public void UpdateCharacter()
	{
		this.gameObject.SetActive(this.character != null);
		if (this.character != null)
		{
			this.Pic.sprite = Resources.Load<Sprite>("Images/Pics/" + this.character.characterName);
			this.nameText.text = this.character.characterName;
			this.healthText.text = this.character.Health.Display() + "/" + this.character.maxHealth;
			this.hungerText.text = this.character.hungry ? "Hungry" : "Fed";
		}
	}

	public void GiveSmallRation()
	{
		GameController.Instance.GiveSmallRation(this.character);
	}


	public void GiveFullRation()
	{
		GameController.Instance.GiveSmallRation(this.character);
	}

}
