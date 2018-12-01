using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{

	public float lifetime = 5;
	public float speed = 20;
	public float damage = 1;
	public GameObject ignoreCollision;
	public float createTime;
	public float deathTime = -1;

	public void Reinit()
	{
		this.createTime = Time.time;
		this.deathTime = -1;
	}

	public void Update()
	{
		if (this.IsDestroyed()) return;
		if (Time.time - this.createTime > this.lifetime) this.deathTime = Time.time;
		else this.transform.Translate(Vector3.up * speed * Time.deltaTime);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.isTrigger) return;
		if (collision.gameObject == this.ignoreCollision) return;
		IBulletDamageable colliderEffect = collision.GetComponent<IBulletDamageable>();
		if (colliderEffect != null)
		{
			colliderEffect.TakeBulletDamages(damage);
		}
		this.deathTime = Time.time;
	}

	public bool IsDestroyed()
	{
		return this.deathTime >= 0;
	}

	public float GetTimeOfDeath()
	{
		return this.deathTime;
	}

	public Transform Transform()
	{
		return this.transform;
	}

	public GameObject GameObject()
	{
		return this.gameObject;
	}

}
