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
	public ConfirmSacrificePanelManager confirmSacrificePanel;

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
			this.healthText.color = this.character.Health < this.character.maxHealth / 5 ? GameController.UiWarningTextColor : GameController.UiDefaultTextColor;
			this.healthText.fontStyle = this.character.Health < this.character.maxHealth / 2 ? FontStyle.Bold : FontStyle.Normal;
			this.hungerText.text = this.character.hungry ? "Hungry!" : "Fed";
			this.hungerText.color = this.character.hungry ? GameController.UiWarningTextColor : GameController.UiDefaultTextColor;
			this.hungerText.fontStyle = this.character.hungry ? FontStyle.Bold : FontStyle.Normal;
		}
	}

	public void GiveSmallRation()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.GiveSmallRation(this.character));
	}


	public void GiveFullRation()
	{
		UISoundManager.Instance.PlayButtonSound(GameController.Instance.GiveFullRation(this.character));
	}

	public void Sacrifice()
	{
		UISoundManager.Instance.PlayOk();
		this.confirmSacrificePanel.Show(this.character);
	}

}
