using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Pool<Bullet>
{
	public Bullet Instantiate(Vector3 position, Quaternion rotation, GameObject ignoreCollisionWith)
	{
		Bullet instance = this.GetNew();
		instance.transform.position = position;
		instance.transform.rotation = rotation;
		instance.ignoreCollision = ignoreCollisionWith;
		return instance;
	}
}
