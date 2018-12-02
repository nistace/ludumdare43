using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : MonoBehaviour
{
	public Button continueButton;
	public Text noContinueText;

	public void UpdateContent()
	{
		this.continueButton.gameObject.SetActive(GameController.Instance.IsContinueAllowed());
		this.noContinueText.gameObject.SetActive(!this.continueButton.gameObject.activeSelf);
	}

	public void Continue()
	{
		UISoundManager.Instance.PlayOk();
		GameController.Instance.GotoNextHalt();
	}
}
