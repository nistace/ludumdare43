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

	public event Action<float> OnHealthChanged = delegate { };

	public void Awake()
	{
		this.anim = this.GetComponent<Animator>();
		this.Health = this.maxHealth;
		this.sqrAllowedRadiusAroundCar = Mathf.Pow(this.allowedRadiusAroundCar, 2);
	}

	public void Update()
	{
		if (this.transform.position.sqrMagnitude > this.sqrAllowedRadiusAroundCar)
			this.transform.position = this.transform.position.normalized * this.allowedRadiusAroundCar;
	}

	public void TakeZombieDamages(float damage)
	{
		this.Health -= damage;
		if (this.Health == 0)
			this.Die();
	}

	public void TakeBulletDamages(float damage)
	{
		Debug.Log("Careful!");
	}

	private void Die()
	{
		this.anim.SetTrigger("Die");
		Destroy(this.GetComponent<Collider2D>());
		if (this.GetComponent<ControlsManager>() != null)
			GameController.Instance.GameOver("You died");
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
