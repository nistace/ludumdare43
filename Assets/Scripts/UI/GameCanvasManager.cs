using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{

	void Awake()
	{
		foreach (Transform child in this.GetComponentInChildren<Transform>())
			child.gameObject.SetActive(true);
	}

}
