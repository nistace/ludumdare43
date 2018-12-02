
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Zombie))]
public class ZombieCustomEditor : Editor
{
	public void OnSceneGUI()
	{
		Zombie zombie = (Zombie)target;

		if (zombie.target != null)
		{
			Handles.color = zombie.targetWithinRange ? Color.red : Color.yellow;
			Handles.DrawLine(zombie.transform.position, zombie.target.Transform().position);
		}

	}
}