using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{

	public GameObject[] keepInactive;

	void Awake()
	{
		List<GameObject> keepInactives = this.keepInactive.ToList();
		foreach (Transform child in this.GetComponentInChildren<Transform>())
			child.gameObject.SetActive(!keepInactives.Contains(child.gameObject));
	}

}
