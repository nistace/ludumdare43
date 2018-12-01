using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class NumbersExtension
{
	public static string Display(this float f, int decs = 2)
	{
		return ((float)((int)(f * Math.Pow(10, decs))) / Math.Pow(10, decs)).ToString();
	}
}
