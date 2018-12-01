
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : UnityEngine.Object, IPoolable
{
	public T[] prefabs;
	private readonly List<T> inactivePool = new List<T>();
	private readonly List<T> activePool = new List<T>();
	public float deathTimeBeforeDeactivation = 1;

	protected T GetNew()
	{
		T instance = null;
		if (this.inactivePool.Count > 0)
		{
			instance = this.inactivePool[0];
			this.inactivePool.RemoveAt(0);
		}
		else
		{
			T prefab = this.prefabs[UnityEngine.Random.Range(0, this.prefabs.Length - 1)];
			instance = Instantiate(prefab, this.transform);
		}
		this.activePool.Add(instance);
		instance.GameObject().SetActive(true);
		instance.Reinit();
		return instance;
	}

	public void Update()
	{
		foreach (T destroyedItem in this.activePool.Where(t => t.IsDestroyed() && Time.time - t.GetTimeOfDeath() > this.deathTimeBeforeDeactivation).ToList())
		{
			this.activePool.Remove(destroyedItem);
			this.inactivePool.Add(destroyedItem);
			destroyedItem.GameObject().SetActive(false);
		}
	}

}