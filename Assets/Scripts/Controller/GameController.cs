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
	public static Color UiWarningTextColor { get { return Instance.uiWarningTextColor; } }
	public static Color UiDefaultTextColor { get { return Instance.uiDefaultTextColor; } }

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

	public Color uiDefaultTextColor = Color.black;
	public Color uiWarningTextColor = new Color(.5f, 0, 0);
	public Vector2 startPosition = Vector2.right * 2.5f;

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
	public event Action OnGotoNextHalt = delegate { };

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
		Time.timeScale = 0;
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

	private void UpdateOtherCharacters()
	{
		this.otherCharacters.ToList().Where(t => t.IsDestroyed()).ToList().ForEach(t => Destroy(t.gameObject));
		this.otherCharacters = this.otherCharacters.ToList().Where(t => !t.IsDestroyed()).ToArray();
	}

	private void OnWaveLastZombieKilled()
	{
		this.playerControls.enabled = false;
		this.player.hungry = true;
		this.UpdateOtherCharacters();
		float lootLowerBound = Mathf.Max(0, -1.5f * this.Wave + 16);
		float lootUpperBound = 1 + 30 / Mathf.Sqrt(this.Wave);
		this.lootedRations = 2 + (int)(UnityEngine.Random.Range(lootLowerBound, lootUpperBound) / 6);
		this.lootedFuel = 1 + (int)(UnityEngine.Random.Range(lootLowerBound, lootUpperBound) * 1.8);
		this.rations += this.lootedRations;
		this.fuel += this.lootedFuel;
		this.fuelLeftBehind = 0;
		this.rationsLeftBehind = 0;
		foreach (HumanCharacter c in this.otherCharacters)
		{
			c.hungry = true;
		}
		this.waveIsActive = false;
		this.OnGameDataChanged();
		this.OnWaveFinished();
	}

	internal bool IsContinueAllowed()
	{
		return this.rations + this.fuel <= this.GetStorageCapacity();
	}

	public void GotoNextHalt()
	{
		if (this.IsContinueAllowed())
		{
			if (this.player.hungry) this.player.Health -= Math.Max(5, (int)(player.Health / 2));
			foreach (HumanCharacter c in this.otherCharacters)
				if (c.hungry) c.Health -= Math.Max(5, (int)(c.Health / 2));
			this.UpdateOtherCharacters();
			this.RepositionateTeamMembers();
			this.Miles += UnityEngine.Random.Range(this.fuel * 2f, this.fuel * 2.5f);
			this.fuel = 0;
			this.Wave++;
			this.WaveRemainingTime = 30;
			this.zombieSpawner.frequency = .5f + this.Wave;
			this.OnGotoNextHalt();
			this.playerControls.enabled = true;
		}
	}

	private void RepositionateTeamMembers()
	{
		List<HumanCharacter> charactersToPlace = new List<HumanCharacter>(this.otherCharacters);
		charactersToPlace.Add(this.player);
		float angle = 360 / charactersToPlace.Count;
		for (Vector2 position = this.startPosition; charactersToPlace.Count > 0; position = position.Rotate(angle))
		{
			int randomCharacterIndex = UnityEngine.Random.Range(0, charactersToPlace.Count - 1);
			HumanCharacter c = charactersToPlace[randomCharacterIndex];
			charactersToPlace.RemoveAt(randomCharacterIndex);
			c.transform.position = position;
		}
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

	public void Sacrifice(HumanCharacter character)
	{
		if (!character.IsDestroyed())
		{
			character.Health = 0;
			this.UpdateOtherCharacters();
			this.rations += 9;
			this.OnGameDataChanged();
		}
	}
}
