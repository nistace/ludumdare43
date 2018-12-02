using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Zombie : MonoBehaviour, IBulletDamageable, IPoolable
{
	public IZombieDamageable target;
	public bool moving = true;
	public float speed = 1;
	public ZombieVisibleArea sight;
	public ZombieAttackRange attackRange;
	public float damages;
	public float attackFrequency = 2f;
	private float timeBeforeNextAttack = 0;
	public bool targetWithinRange = false;
	private float deathTime = -1;
	private new Collider2D collider;
	private Animator anim;

	public void Awake()
	{
		this.collider = this.GetComponent<Collider2D>();
		this.anim = this.GetComponent<Animator>();
		this.sight.OnClosestItemChanged += this.SetTarget;
		this.attackRange.OnItemWithinRangeChanged += this.SetTargetWithinRange;
		this.Reinit();
	}

	public void Reinit()
	{
		this.deathTime = -1;
		this.sight.Reinit();
		this.attackRange.Reinit();
		this.moving = true;
		this.collider.enabled = true;
		this.anim.SetTrigger("Restart");
	}

	public void Update()
	{
		if (this.IsDestroyed())
		{
			this.target = null;
			return;
		}
		if (this.target == null || this.target.IsDestroyed())
			this.target = this.sight.Closest;
		if (this.target == null || this.target.IsDestroyed())
			this.target = GameController.Instance.car;
		if (this.moving)
		{
			this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (((Vector2)this.target.Transform().position) - (Vector2)this.transform.position).AngleWithVector2Up()));
			this.transform.position += (this.target.Transform().position - this.transform.position).normalized * this.speed * Time.deltaTime;
		}
		this.timeBeforeNextAttack -= Time.deltaTime;
		if (this.timeBeforeNextAttack <= 0 && this.target != null && this.targetWithinRange)
		{
			this.target.TakeZombieDamages(this.damages);
			this.timeBeforeNextAttack = 1 / this.attackFrequency;
		}
	}

	private void SetTargetWithinRange(IZombieDamageable target, bool withinRange)
	{
		if (target == this.target)
			this.targetWithinRange = withinRange;
	}

	private void SetTarget(IZombieDamageable newTarget)
	{
		this.target = newTarget ?? GameController.Instance.car;
		this.targetWithinRange = this.attackRange.IsWithinRange(this.target);
	}

	public void TakeBulletDamages(float damage)
	{
		this.Die();
	}

	private void Die()
	{
		this.moving = false;
		this.deathTime = Time.time;
		this.collider.enabled = false;
		this.anim.SetTrigger("Die");
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
		return this.deathTime >= 0;
	}

	public float GetTimeOfDeath()
	{
		return this.deathTime;
	}

}