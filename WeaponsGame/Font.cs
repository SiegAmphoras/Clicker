using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WeaponsGame
{
	public class Font
	{
		public string Name;

		public string FontPath;

		public float Size = 16f;

		public int CharStart = 32;

		public int CharEnd = 128;

		private Face face;

		[XmlIgnore]
		public Dictionary<char, FontChar> charDictionary;

		public Font()
		{
			this.charDictionary = new Dictionary<char, FontChar>();
		}

		public void Precache()
		{
			this.face = new Face(Renderer.fontLib, this.FontPath);
			this.face.SetPixelSizes(0u, (uint)this.Size);
			this.GenTextures();
		}

		private void GenTextures()
		{
			int num = this.CharEnd - this.CharStart;
			int[] array = new int[num];
			this.charDictionary = new Dictionary<char, FontChar>();
			GL.GenTextures(num, array);
			for (int i = 0; i < num; i++)
			{
				char c = (char)(i + this.CharStart);
				GlyphSlot glyphSlot = this.LoadGlyph(c);
				GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
				GL.BindTexture(TextureTarget.Texture2D, array[i]);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 33069);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 33069);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
				GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, 8448);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Alpha, glyphSlot.Bitmap.Width, glyphSlot.Bitmap.Rows, 0, PixelFormat.Alpha, PixelType.UnsignedByte, glyphSlot.Bitmap.Buffer);
				this.charDictionary.Add(c, new FontChar
				{
					textureID = array[i],
					BitmapWidth = glyphSlot.Bitmap.Width,
					BitmapHeight = glyphSlot.Bitmap.Rows,
					BitmapPos = new Vector2((float)glyphSlot.BitmapLeft, (float)glyphSlot.BitmapTop),
					Advance = new Vector2((float)(glyphSlot.Advance.X >> 6), (float)(glyphSlot.Advance.Y >> 6))
				});
			}
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 0);
		}

		private GlyphSlot LoadGlyph(char character)
		{
			this.face.LoadChar((uint)character, LoadFlags.Render, LoadTarget.Normal);
			return this.face.Glyph;
		}

		public Vector2 MeasureString(string text)
		{
			Vector2 result = default(Vector2);
			float num = 0f;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				FontChar fontChar = this.charDictionary[c];
				if (c == '\n')
				{
					num = result.X;
					result.X = 0f;
					result.Y += (float)fontChar.BitmapHeight * 1.5f;
				}
				else
				{
					result.X += fontChar.BitmapPos.X + fontChar.Advance.X;
					result.Y += fontChar.BitmapPos.Y + fontChar.Advance.Y - result.Y;
					if (result.X > num)
					{
						num = result.X;
					}
				}
			}
			result.X = num;
			return result;
		}
	}
}
