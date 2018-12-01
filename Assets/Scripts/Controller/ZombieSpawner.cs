using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	public float spawnRadius = 6;
	public float frequency = 1;
	public bool active;
	private float timeBeforeNextSpawn = 0;

	public void Update()
	{
		this.timeBeforeNextSpawn -= Time.deltaTime;
		if (active && this.timeBeforeNextSpawn <= 0)
		{
			GameController.ZombiePool.Instantiate(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * this.spawnRadius, Quaternion.identity);
			this.timeBeforeNextSpawn = 1 / this.frequency;
		}
	}

}