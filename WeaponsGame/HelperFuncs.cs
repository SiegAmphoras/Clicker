using OpenTK;
using System;

namespace WeaponsGame
{
	internal class HelperFuncs
	{
		public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float t)
		{
			Vector2 vec = (value3 - value1) / 2f;
			Vector2 vec2 = (value4 - value2) / 2f;
			float num = t * t;
			float num2 = num * t;
			float scale = 2f * num2 - 3f * num + 1f;
			float scale2 = -2f * num2 + 3f * num;
			float scale3 = num2 - 2f * num + t;
			float scale4 = num2 - num;
			Vector2 left = value2 * scale + vec * scale3;
			return left + (value3 * scale2 + vec2 * scale4);
		}

		public static Vector2 CosInterp(Vector2 a, Vector2 b, float t)
		{
			return new Vector2((float)System.Math.Floor((double)HelperFuncs.CosineInterpolate(a.X, b.X, t)), (float)System.Math.Floor((double)HelperFuncs.CosineInterpolate(a.Y, b.Y, t)));
		}

		public static float CosineInterpolate(float y1, float y2, float mu)
		{
			float num = (float)((1.0 - System.Math.Cos((double)mu * 3.1415926535897931)) / 2.0);
			return y1 * (1f - num) + y2 * num;
		}
	}
}
