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
}