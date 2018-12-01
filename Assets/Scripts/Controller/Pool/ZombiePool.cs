using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : Pool<Zombie>
{

	public Zombie Instantiate(Vector3 position, Quaternion rotation)
	{
		Zombie instance = this.GetNew();
		instance.transform.position = position;
		instance.transform.rotation = rotation;
		return instance;
	}
}
