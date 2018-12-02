using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{

	public void StartGame()
	{
		UISoundManager.Instance.PlayOk();
		App.Instance.StartGame();
	}
}
