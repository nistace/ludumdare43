using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class VectorsExtension
{
	public static float AngleWithVector2Up(this Vector2 v)
	{
		// sorry for anyone reading this, there's probably a better way but...
		float angle = Vector2.Angle(Vector2.up, v);
		if (v.x > 0) angle = 360 - angle;
		return angle;
	}

	public static Vector2 Rotate(this Vector2 v, float degAngle)
	{
		float radAngle = degAngle * Mathf.Deg2Rad;
		float sin = Mathf.Sin(radAngle);
		float cos = Mathf.Cos(radAngle);
		return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
	}
}