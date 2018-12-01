using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackRange<T> : MonoBehaviour where T : IMonoBehaviour
{

	private readonly HashSet<T> withinRange = new HashSet<T>();
	public event Action<T, bool> OnItemWithinRangeChanged = delegate { };


	public void Reinit()
	{
		this.withinRange.Clear();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		T component = collision.GetComponent<T>();
		if (component != null && !withinRange.Contains(component))
		{
			this.withinRange.Add(component);
			this.OnItemWithinRangeChanged(component, true);
		}
	}

	public void Update()
	{
		foreach (T t in this.withinRange)
		{
			if (t.IsDestroyed())
			{
				this.OnItemWithinRangeChanged(t, false);
			}
		}
		this.withinRange.RemoveWhere(t => t.IsDestroyed());
	}

	public bool IsWithinRange(T item)
	{
		return this.withinRange.Contains(item);
	}
}
