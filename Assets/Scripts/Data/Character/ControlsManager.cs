using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class ControlsManager : MonoBehaviour
{

	public float speed = 5;
	private Shooter shooter;

	public void Awake()
	{
		this.shooter = this.GetComponent<Shooter>();
	}

	public void Update()
	{
		this.UpdateLookDirection();
		this.UpdatePosition();
		this.UpdateShooting();
	}

	private void UpdateShooting()
	{
		this.shooter.shooting = Input.GetAxisRaw("Fire1") != 0;
	}

	private void UpdatePosition()
	{
		Vector3 movement = Vector2.up * Input.GetAxis("Vertical") + Vector2.right * Input.GetAxis("Horizontal");
		if (movement == Vector3.zero) return;
		if (movement.sqrMagnitude > 1) movement = movement.normalized;
		this.transform.position += movement * this.speed * Time.deltaTime;
	}

	private void UpdateLookDirection()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (mousePosition - (Vector2)this.transform.position).AngleWithVector2Up()));
	}
}
