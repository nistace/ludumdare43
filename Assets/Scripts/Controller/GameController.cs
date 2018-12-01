using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public int rations;
	public int rationsLeftBehind;
	public int lootedRations;
	public int fuel;
	public int fuelLeftBehind;
	public int lootedFuel;

	public bool waveIsActive;

	public Car car;
	public HumanCharacter player;
	private ControlsManager playerControls;
	public HumanCharacter[] otherCharacters;

	public event Action<int> OnWaveCountChanged = delegate { };
	public event Action<float> OnMilesChanged = delegate { };
	public event Action<float> OnWaveRemainingTimeChanged = delegate { };
	public event Action<string, float> OnGameOver = delegate { };

	public event Action OnGameDataChanged = delegate { };
	public event Action OnWaveFinished = delegate { };

	public void Awake()
	{
		if (Instance == null) Instance = this;
		if (Instance != this) Destroy(this.gameObject);
		else
		{
			this.zombieSpawner = this.GetComponent<ZombieSpawner>();
			this.playerControls = this.player.GetComponent<ControlsManager>();
		}
	}

	public void Start()
	{
		this.GotoNextHalt();
	}

	public void GameOver(string reason)
	{
		this.playerControls.enabled = false;
		this.OnGameOver(reason, this.Miles);
	}

	public void Update()
	{
		if (this.waveIsActive)
		{
			this.WaveRemainingTime -= Time.deltaTime;
			if (this.WaveRemainingTime <= 0)
			{
				this.zombieSpawner.active = false;
				if (this.zombiePool.CountActive == 0)
					this.OnWaveLastZombieKilled();
			}
		}
	}

	private void OnWaveLastZombieKilled()
	{
		this.playerControls.enabled = false;
		this.player.hungry = true;
		this.otherCharacters = this.otherCharacters.ToList().Where(t => !t.IsDestroyed()).ToArray();
		float lootLowerBound = Mathf.Max(0, -1.5f * this.Wave + 16);
		float lootUpperBound = 1 + 30 / Mathf.Sqrt(this.Wave);
		this.lootedRations += (int)UnityEngine.Random.Range(lootLowerBound, lootUpperBound);
		this.lootedFuel += (int)UnityEngine.Random.Range(lootLowerBound, lootUpperBound);
		this.rations += this.lootedRations;
		this.fuel += this.lootedFuel;
		foreach (HumanCharacter c in this.otherCharacters)
		{
			c.hungry = true;
		}
		this.waveIsActive = false;
		this.OnGameDataChanged();
		this.OnWaveFinished();
	}

	public void GotoNextHalt()
	{
		this.playerControls.enabled = true;
		this.Miles += 1;
		this.Wave++;
		this.WaveRemainingTime = 30;
		this.zombieSpawner.frequency = 1;
	}

	public void LaunchWave()
	{
		if (!this.waveIsActive)
		{
			this.zombieSpawner.active = true;
			this.waveIsActive = true;
		}
	}

	public void GiveSmallRation(HumanCharacter character)
	{
		if (this.rations > 0)
		{
			this.rations--;
			character.hungry = false;
			this.OnGameDataChanged();
		}
	}

	public void GiveFullRation(HumanCharacter character)
	{
		if (this.rations > 1)
		{
			this.rations -= 2;
			character.hungry = false;
			character.Health += 20;
			this.OnGameDataChanged();
		}
	}

	public void LetRation()
	{
		if (this.rations > 0)
		{
			this.rationsLeftBehind++;
			this.rations--;
			this.OnGameDataChanged();
		}
	}

	public void TakeRation()
	{
		if (this.rationsLeftBehind > 0)
		{
			this.rationsLeftBehind--;
			this.rations++;
			this.OnGameDataChanged();
		}
	}

	public void LetFuel()
	{
		if (this.fuel > 0)
		{
			this.fuelLeftBehind++;
			this.fuel--;
			this.OnGameDataChanged();
		}
	}

	public void TakeFuel()
	{
		if (this.fuelLeftBehind > 0)
		{
			this.fuelLeftBehind--;
			this.fuel++;
			this.OnGameDataChanged();
		}
	}

	public int GetStorageCapacity()
	{
		return 70 - this.otherCharacters.Length * 10;
	}
}
