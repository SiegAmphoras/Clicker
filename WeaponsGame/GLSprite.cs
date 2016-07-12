using OpenTK;
using System;

namespace WeaponsGame
{
	public class GLSprite
	{
		public int Width;

		public int Height;

		public int SrcWidth;

		public int SrcHeight;

		public int glTextureID;

		public GLSprite(int width, int height, Texture tex)
		{
			if (tex != null)
			{
				this.Width = width;
				this.Height = height;
				this.SrcWidth = (int)tex.Width;
				this.SrcHeight = (int)tex.Height;
			}
		}

		public TextureUVW GetUVForFrame(int frameX, int frameY)
		{
			Vector2[] array = new Vector2[4];
			float num = 1f * (float)this.Width / (float)this.SrcWidth;
			float num2 = 1f * (float)this.Height / (float)this.SrcHeight;
			array[0] = new Vector2(num * (float)frameX, num2 * (float)frameY);
			array[1] = new Vector2(num * (float)frameX + num, num2 * (float)frameY);
			array[2] = new Vector2(num * (float)frameX + num, num2 * (float)frameY + num2);
			array[3] = new Vector2(num * (float)frameX, num2 * (float)frameY + num2);
			return new TextureUVW(array[0].X, array[0].Y, array[2].X, array[2].Y);
		}
	}
}
