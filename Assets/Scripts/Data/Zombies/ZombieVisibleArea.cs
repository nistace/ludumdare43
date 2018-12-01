using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVisibleArea : VisibleArea<IZombieDamageable>
{

	public void Awake()
	{
		this.layerMask = 1 << LayerMask.NameToLayer("TeamMember");
	}
}
