using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanCharacter : MonoBehaviour, IZombieDamageable, IBulletDamageable
{
	private Animator anim;
	public float allowedRadiusAroundCar = 3;
	private float sqrAllowedRadiusAroundCar;
	public string characterName;
	public float maxHealth;
	private float _health;
	public float Health
	{
		get { return this._health; }
		set { this._health = Mathf.Min(value, this.maxHealth); this.OnHealthChanged(this._health); }
	}

	public bool hungry = true;

	public string[] prosKeepAlive;
	public string[] consKeepAlive;

	public string[] thingsToSayOnBulletDamage;
	public string[] thingsToSayOnZombieDamage;
	public string[] thingsToSayOnDie;
	private float timeBeforeSayingNewStuff;

	public event Action<float> OnHealthChanged = delegate { };
	public event Action<string, float> OnSaySomething = delegate { };

	public void Awake()
	{
		this.anim = this.GetComponent<Animator>();
		this.Health = this.maxHealth;
		this.sqrAllowedRadiusAroundCar = Mathf.Pow(this.allowedRadiusAroundCar, 2);
	}

	public void Update()
	{
		this.timeBeforeSayingNewStuff -= Time.deltaTime;
		if (this.Health <= 0)
			this.Die();
		if (this.transform.position.sqrMagnitude > this.sqrAllowedRadiusAroundCar)
			this.transform.position = this.transform.position.normalized * this.allowedRadiusAroundCar;
	}

	public void TakeZombieDamages(float damage)
	{
		this.Health -= damage;
		this.SaySomething(this.thingsToSayOnZombieDamage);
	}

	public void TakeBulletDamages(float damage)
	{
		this.SaySomething(this.thingsToSayOnBulletDamage);
	}

	private void SaySomething(string[] values)
	{
		if (this.timeBeforeSayingNewStuff <= 0)
		{
			this.OnSaySomething(values[UnityEngine.Random.Range(0, values.Length)], 4);
			this.timeBeforeSayingNewStuff = 6;
		}
	}

	private void Die()
	{
		this.SaySomething(this.thingsToSayOnDie);
		this.anim.SetTrigger("Die");
		Destroy(this.GetComponent<Collider2D>());
		this.GetComponent<Shooter>().enabled = false;
		if (this.GetComponent<ControlsManager>() != null)
			GameController.Instance.GameOver("You died");
		else
			this.GetComponent<AICharacter>().enabled = false;
	}

	public Transform Transform()
	{
		return this.transform;
	}

	public GameObject GameObject()
	{
		return this.gameObject;
	}

	public bool IsDestroyed()
	{
		return this.Health <= 0;
	}
}
