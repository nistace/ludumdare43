using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisibleArea : VisibleArea<IBulletDamageable>
{

	public void Awake()
	{
		this.layerMask = 1 << LayerMask.NameToLayer("Zombie");
	}
}
