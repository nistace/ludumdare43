using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Shooter : MonoBehaviour
{
	private new AudioSource audio;
	public float shootFrequency = .5f;
	public bool shooting;
	public float timeBeforeNextBullet;
	public Transform bulletSpawnPosition;

	public void Awake()
	{
		this.audio = this.GetComponent<AudioSource>();
	}

	public void Update()
	{
		this.timeBeforeNextBullet -= Time.deltaTime;
		if (this.shooting && this.timeBeforeNextBullet <= 0)
		{
			GameController.BulletPool.Instantiate(bulletSpawnPosition.position, this.transform.rotation, this.gameObject);
			this.audio.Play();
			this.timeBeforeNextBullet = 1 / this.shootFrequency;
		}
	}
}

