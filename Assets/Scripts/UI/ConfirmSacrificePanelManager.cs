using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmSacrificePanelManager : MonoBehaviour
{
	private HumanCharacter character;
	public Image pic;
	public Text areYouSureText;
	public Transform prosPanel;
	public Transform consPanel;

	public void Show(HumanCharacter character)
	{
		this.character = character;
		this.pic.sprite = Resources.Load<Sprite>("Images/Pics/" + character.characterName);
		this.areYouSureText.text = "Are you sure you want to sacrifice " + character.characterName + " ?";
		for (int i = 0; i < this.prosPanel.childCount; ++i)
		{
			Transform proPanel = this.prosPanel.GetChild(i);
			if (character.prosKeepAlive.Length > i)
			{
				proPanel.GetComponentInChildren<Text>().text = character.prosKeepAlive[i];
			}
			proPanel.gameObject.SetActive(character.prosKeepAlive.Length > i);
		}
		for (int i = 0; i < this.consPanel.childCount; ++i)
		{
			Transform conPanel = this.consPanel.GetChild(i);
			if (character.consKeepAlive.Length > i)
			{
				conPanel.GetComponentInChildren<Text>().text = character.consKeepAlive[i];
			}
			conPanel.gameObject.SetActive(character.consKeepAlive.Length > i);
		}
		this.gameObject.SetActive(true);
	}

	public void ConfirmSacrifice()
	{
		if (GameController.Instance.Sacrifice(this.character))
		{
			UISoundManager.Instance.Play("sacrifice_" + this.character.characterName);
		}
		this.gameObject.SetActive(false);
	}

	public void CancelSacrifice()
	{
		UISoundManager.Instance.PlayOk();
		this.gameObject.SetActive(false);
	}
}
