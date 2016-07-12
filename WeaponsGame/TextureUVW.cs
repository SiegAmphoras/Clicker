using OpenTK;
using System;

namespace WeaponsGame
{
	public class TextureUVW
	{
		public float A = 0f;

		public float B = 0f;

		public float C = 1f;

		public float D = 1f;

		public Vector2 TopLeft
		{
			get
			{
				return new Vector2(this.A, this.B);
			}
		}

		public Vector2 TopRight
		{
			get
			{
				return new Vector2(this.C, this.B);
			}
		}

		public Vector2 BottomLeft
		{
			get
			{
				return new Vector2(this.C, this.D);
			}
		}

		public Vector2 BottomRight
		{
			get
			{
				return new Vector2(this.A, this.D);
			}
		}

		public TextureUVW()
		{
		}

		public TextureUVW(float A, float B, float C, float D)
		{
			this.A = A;
			this.B = B;
			this.C = C;
			this.D = D;
		}
	}
}
