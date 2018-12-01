using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

	public float shootFrequency = .5f;
	public bool shooting;
	public float timeBeforeNextBullet;
	public Transform bulletSpawnPosition;

	public void Update()
	{
		this.timeBeforeNextBullet -= Time.deltaTime;
		if (this.shooting && this.timeBeforeNextBullet <= 0)
		{
			GameController.BulletPool.Instantiate(bulletSpawnPosition.position, this.transform.rotation, this.gameObject);
			this.timeBeforeNextBullet = 1 / this.shootFrequency;
		}
	}
}

