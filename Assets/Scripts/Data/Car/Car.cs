using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Car : MonoBehaviour, IBulletDamageable, IZombieDamageable
{
	private new AudioSource audio;
	private Animator anim;
	public float maxHealth;
	private float _health;
	public float Health
	{
		get { return this._health; }
		set { this._health = value; this.OnHealthChanged(this._health); }
	}


	public event Action<float> OnHealthChanged = delegate { };

	public void Awake()
	{
		this.anim = this.GetComponent<Animator>();
		this.audio = this.GetComponent<AudioSource>();
		this.Health = maxHealth;
	}

	public void TakeBulletDamages(float damage)
	{
		if (this.IsDestroyed()) return;
		this.Damage(damage);
		GameController.Instance.LaunchWave();
	}

	public void TakeZombieDamages(float damage)
	{
		this.Damage(damage);
	}

	public void Damage(float damage)
	{
		this.Health -= damage;
		if (this.Health <= 0)
		{
			this.StopAlarm();
			this.anim.SetTrigger("Explode");
			GameController.Instance.GameOver("Your car exploded");
		}
	}

	public void StopAlarm()
	{
		this.anim.SetBool("Alarm", false);
		this.audio.Stop();
	}

	public void StartAlarm()
	{
		this.audio.Play();
		this.anim.SetBool("Alarm", true);
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