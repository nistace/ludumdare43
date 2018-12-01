using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VisibleArea<T> : MonoBehaviour where T : IMonoBehaviour
{
	protected int layerMask;
	private readonly HashSet<T> visibleItems = new HashSet<T>();
	private T _closest;
	public T Closest
	{
		get { return this._closest; }
		private set
		{
			this._closest = value;
			this.OnClosestItemChanged(this._closest);
		}
	}
	public event Action<T> OnClosestItemChanged = delegate { };

	public void Reinit()
	{
		this.visibleItems.Clear();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if ((this.layerMask >> collision.gameObject.layer) % 2 == 0) return;
		T component = collision.GetComponent<T>();
		if (component != null && !visibleItems.Contains(component))
		{
			this.visibleItems.Add(component);
			this.UpdateClosest();
		}
	}

	public void Update()
	{
		if (this.Closest != null && this.Closest.IsDestroyed())
			this.Closest = default(T);
		this.visibleItems.RemoveWhere(t => t.IsDestroyed());
		this.UpdateClosest();
	}

	private void UpdateClosest()
	{
		float newClosestDistance = float.MaxValue;
		T newClosest = default(T);
		if (this.visibleItems.Count > 0)
		{
			foreach (T t in this.visibleItems)
			{
				float distance = (this.transform.position - t.Transform().position).sqrMagnitude;
				if (newClosestDistance > distance)
				{
					newClosestDistance = distance;
					newClosest = t;
				}
			}
		}
		if (newClosest != null && !newClosest.Equals(this.Closest) || newClosest == null && this.Closest != null)
		{
			this.Closest = newClosest;
			this.OnClosestItemChanged(this.Closest);
		}
	}
}
