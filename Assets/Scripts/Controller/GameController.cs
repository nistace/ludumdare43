using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZombieSpawner))]
public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }
	public static BulletPool BulletPool { get { return Instance.bulletPool; } }
	public static ZombiePool ZombiePool { get { return Instance.zombiePool; } }

	public BulletPool bulletPool;
	public ZombiePool zombiePool;

	private ZombieSpawner zombieSpawner;
	private float _miles = 0;
	public float Miles
	{
		get { return this._miles; }
		set
		{
			this._miles = value;
			this.OnMilesChanged(this._miles);
		}
	}
	private int _wave = 0;
	public int Wave
	{
		get { return this._wave; }
		set
		{
			this._wave = value;
			this.OnWaveCountChanged(this._wave);
		}
	}
	private float _waveRemainingTime = 0;
	public float WaveRemainingTime
	{
		get { return this._waveRemainingTime; }
		set
		{
			this._waveRemainingTime = value;
			this.OnWaveRemainingTimeChanged(this._waveRemainingTime);
		}
	}
	public bool waveIsActive;

	public Car car;
	public HumanCharacter player;

	public event Action<int> OnWaveCountChanged = delegate { };
	public event Action<float> OnMilesChanged = delegate { };
	public event Action<float> OnWaveRemainingTimeChanged = delegate { };
	public event Action<string, float> OnGameOver = delegate { };

	public void Awake()
	{
		if (Instance == null) Instance = this;
		if (Instance != this) Destroy(this.gameObject);
		else
		{
			this.zombieSpawner = this.GetComponent<ZombieSpawner>();
		}
	}

	public void Start()
	{
		// TODO intro
		this.GotoNextHalt();
	}

	public void GameOver(string reason)
	{
		Time.timeScale = 0;
		this.OnGameOver(reason, this.Miles);
	}

	public void Update()
	{
		if (this.waveIsActive)
		{
			this.WaveRemainingTime -= Time.deltaTime;
			if (this.WaveRemainingTime <= 0)
				this.OnWaveEnded();
		}
	}

	private void OnWaveEnded()
	{
		this.zombieSpawner.active = false;
		this.waveIsActive = false;
		// TODO get the resources at the end of the wave
		this.GotoNextHalt();
	}

	public void GotoNextHalt()
	{
		this.Miles += 1;
		this.Wave++;
		this.WaveRemainingTime = 30;
		this.zombieSpawner.frequency = 1;
	}

	public void LaunchWave()
	{
		this.zombieSpawner.active = true;
		this.waveIsActive = true;
	}
}
