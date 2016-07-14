using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace WeaponsGame
{
	internal class Renderer
	{
		public static System.Collections.Generic.List<Texture> textureAtlas;

		public static System.Collections.Generic.List<Font> fontAtlas;

		private Matrix4 cameraMat;

		public static Library fontLib;

		public Renderer()
		{
			Renderer.fontLib = new Library();
			Renderer.textureAtlas = new System.Collections.Generic.List<Texture>();
			Renderer.fontAtlas = new System.Collections.Generic.List<Font>();
			this.cameraMat = Matrix4.CreatePerspectiveFieldOfView(1.3f, (float)(Engine.BaseWindow.ClientSize.Width / Engine.BaseWindow.ClientSize.Height), 0.001f, 100f);
		}

		public void PrecacheTextures()
		{
			string[] files = FileManager.GetFiles("./Content/Textures/", true);
			int[] array = new int[files.Length];
			GL.GenTextures(files.Length, array);
			for (int i = 0; i < files.Length; i++)
			{
				this.LoadTexture(files[i], array[i]);
			}
		}

		public void PrecacheFonts()
		{
			string path = "./Content/Fonts/fonts.dat";
			Font[] array = null;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Font[]));
			using (System.IO.TextReader textReader = new System.IO.StreamReader(path))
			{
				array = (Font[])xmlSerializer.Deserialize(textReader);
			}
			if (array != null)
			{
				Renderer.fontAtlas.AddRange(array);
				foreach (Font current in Renderer.fontAtlas)
				{
					current.Precache();
				}
			}
		}

		public void TestWriteFont(Font[] f)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Font[]));
			using (System.IO.TextWriter textWriter = new System.IO.StreamWriter("./Content/Fonts/Test.dat"))
			{
				xmlSerializer.Serialize(textWriter, f);
			}
		}

		public void Prepare3D()
		{
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Viewport(0, 0, Engine.BaseWindow.ClientSize.Width, Engine.BaseWindow.ClientSize.Height);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.LoadMatrix(ref this.cameraMat);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void End3D()
		{
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PopMatrix();
		}

		public void Prepare2D()
		{
			GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Disable(EnableCap.CullFace);
			GL.Viewport(0, 0, Engine.BaseWindow.ClientSize.Width, Engine.BaseWindow.ClientSize.Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.Ortho(0.0, (double)Engine.BaseWindow.ClientSize.Width, (double)Engine.BaseWindow.ClientSize.Height, 0.0, 0.0, 100.0);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void End2D()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		public static void StartClip(System.Drawing.Rectangle clipRegion)
		{
			GL.PushAttrib(AttribMask.ScissorBit);
			GL.Scissor(clipRegion.X, -(clipRegion.Y - (Engine.BaseWindow.ClientSize.Height - clipRegion.Height)), clipRegion.Width, clipRegion.Height);
			GL.Enable(EnableCap.ScissorTest);
		}

		public static void EndClip()
		{
			GL.Disable(EnableCap.ScissorTest);
			GL.PopAttrib();
		}

		public static void Clear(Color4 color)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.ClearColor(color);
		}

		public static Texture[] GetSkyboxTextures(string skyboxName)
		{
			Texture[] array = new Texture[6];
			System.Collections.Generic.List<Texture> list = Renderer.textureAtlas.FindAll((Texture i) => i.Name.Contains(skyboxName));
			if (list.Count<Texture>() == 6)
			{
				array[0] = list.Find((Texture i) => i.Name.Contains("front"));
				array[1] = list.Find((Texture i) => i.Name.Contains("back"));
				array[2] = list.Find((Texture i) => i.Name.Contains("left"));
				array[3] = list.Find((Texture i) => i.Name.Contains("right"));
				array[4] = list.Find((Texture i) => i.Name.Contains("top"));
				array[5] = list.Find((Texture i) => i.Name.Contains("bottom"));
			}
			else if (list.Count<Texture>() < 6)
			{
			}
			Texture[] result;
			if (array.Length >= 6)
			{
				result = array;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void LoadTexture(string filename, int textureID = -1)
		{
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(filename);
			if (textureID == -1)
			{
				textureID = GL.GenTexture();
			}
			string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(filename);
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 33071);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 33071);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
			System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, 8448);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
			bitmap.UnlockBits(bitmapData);
			Texture item = new Texture
			{
				Name = fileNameWithoutExtension,
				ID = textureID,
				Width = (float)bitmapData.Width,
				Height = (float)bitmapData.Height
			};
			Renderer.textureAtlas.Add(item);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public static Font GetFont(string fontname)
		{
			Font result;
			if (!Renderer.fontAtlas.Exists((Font i) => i.Name == fontname))
			{
				result = null;
			}
			else
			{
				result = Renderer.fontAtlas.Find((Font i) => i.Name == fontname);
			}
			return result;
		}

		public static Texture GetTexture(string textureName)
		{
			Texture result;
			if (Renderer.textureAtlas == null || !Renderer.textureAtlas.Exists((Texture i) => i.Name == textureName))
			{
				result = null;
			}
			else
			{
				result = Renderer.textureAtlas.Find((Texture i) => i.Name == textureName);
			}
			return result;
		}

		public static void DrawString(Font font, string line, Vector2 position, Color4 color)
		{
			Renderer.DrawString(font, line, position, 1f, color, 1f);
		}

		public static void DrawString(Font font, string line, Vector2 position, float whitespacePadMult, Color4 color)
		{
			Renderer.DrawString(font, line, position, whitespacePadMult, color, 1f);
		}

		public static void DrawString(Font font, string line, Vector2 position, float whitespacePadMult, Color4 color, float alpha)
		{
			if (font != null)
			{
				if (line != null && line.Length > 0)
				{
					Vector2 left = position;
					int num = 1;
					for (int i = 0; i < line.Length; i++)
					{
						char c = line[i];
						FontChar fontChar = font.charDictionary[c];
						float num2 = 1f;
						if (c == ' ')
						{
							left += new Vector2(fontChar.Advance.X * whitespacePadMult, fontChar.Advance.Y);
						}
						else
						{
                            Vector2 vector = new Vector2(left.X + fontChar.BitmapPos.X, left.Y - fontChar.BitmapPos.Y + (font.Size / 1.5f));
							if (c == '\n')
							{
								left = new Vector2(position.X, position.Y + (float)fontChar.BitmapHeight * 1.5f * (float)num);
								num++;
							}
							else
							{
								color.A = alpha;
								Renderer.DrawTexturedRectangle(vector.X, vector.Y, (float)fontChar.BitmapWidth, (float)fontChar.BitmapHeight, fontChar.textureID, color, null);
								left += new Vector2(fontChar.Advance.X * num2, fontChar.Advance.Y);
							}
						}
					}
				}
			}
		}

		public static void DrawTexturedRectangle(float x, float y, float width, float height, string textureName)
		{
			Renderer.DrawTexturedRectangle(x, y, width, height, textureName, System.Drawing.Color.White, null);
		}

		public static void DrawTexturedRectangle(float x, float y, float width, float height, string textureName, Color4 color, TextureUVW uvs = null)
		{
			if (uvs == null)
			{
				uvs = new TextureUVW();
			}
			GL.BindTexture(TextureTarget.Texture2D, Renderer.textureAtlas.Find((Texture i) => i.Name == textureName).ID);
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(color.R, color.G, color.B, color.A);
			GL.TexCoord2(uvs.A, uvs.B);
			GL.Vertex2(x, y);
			GL.TexCoord2(uvs.C, uvs.B);
			GL.Vertex2(x + width, y);
			GL.TexCoord2(uvs.C, uvs.D);
			GL.Vertex2(x + width, y + height);
			GL.TexCoord2(uvs.A, uvs.D);
			GL.Vertex2(x, y + height);
			GL.End();
		}

		public static void DrawTexturedRectangle(float x, float y, float width, float height, int textureID, Color4 color, TextureUVW uvs = null)
		{
			if (uvs == null)
			{
				uvs = new TextureUVW();
			}
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(color.R, color.G, color.B, color.A);
			GL.TexCoord2(uvs.A, uvs.B);
			GL.Vertex2(x, y);
			GL.TexCoord2(uvs.C, uvs.B);
			GL.Vertex2(x + width, y);
			GL.TexCoord2(uvs.C, uvs.D);
			GL.Vertex2(x + width, y + height);
			GL.TexCoord2(uvs.A, uvs.D);
			GL.Vertex2(x, y + height);
			GL.End();
		}

		public static void DrawTexturedRectangleCentered(float x, float y, float width, float height, int textureID, Color4 color, TextureUVW uvs = null)
		{
			if (uvs == null)
			{
				uvs = new TextureUVW();
			}
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(color.R, color.G, color.B, color.A);
			GL.TexCoord2(uvs.A, uvs.B);
			GL.Vertex2(x - width / 2f, y - height / 2f);
			GL.TexCoord2(uvs.C, uvs.B);
			GL.Vertex2(x + width / 2f, y - height / 2f);
			GL.TexCoord2(uvs.C, uvs.D);
			GL.Vertex2(x + width / 2f, y + height / 2f);
			GL.TexCoord2(uvs.A, uvs.D);
			GL.Vertex2(x - width / 2f, y + height / 2f);
			GL.End();
		}

		public static void DrawTexturedCircle(float x, float y, float width, float height, string textureName)
		{
			Renderer.DrawTexturedCircle(x, y, width, height, textureName, System.Drawing.Color.White, null);
		}

		public static void DrawTexturedCircle(float x, float y, float width, float height, string textureName, Color4 color, TextureUVW uvs = null)
		{
			if (uvs == null)
			{
				uvs = new TextureUVW();
			}
			GL.BindTexture(TextureTarget.Texture2D, Renderer.textureAtlas.Find((Texture i) => i.Name == textureName).ID);
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color4(color);
			GL.TexCoord2(uvs.A, uvs.B);
			GL.Vertex2(x + width / 2f, y + height / 2f);
			float num = width / 2f;
			for (int j = 0; j < 12; j++)
			{
				GL.TexCoord2(uvs.A, uvs.D);
				GL.Vertex2((double)x + (double)num * System.Math.Cos((double)j * 6.2831853071795862 / 12.0), (double)y + (double)num * System.Math.Sin((double)j * 6.2831853071795862 / 12.0));
			}
			GL.End();
		}

		public static void DrawTexturedCircle(float x, float y, float width, float height, int textureID, Color4 color, TextureUVW uvs = null)
		{
			if (uvs == null)
			{
				uvs = new TextureUVW();
			}
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(color);
			GL.TexCoord2(uvs.A, uvs.B);
			GL.Vertex2(x, y);
			GL.TexCoord2(uvs.C, uvs.B);
			GL.Vertex2(x + width, y);
			GL.TexCoord2(uvs.C, uvs.D);
			GL.Vertex2(x + width, y + height);
			GL.TexCoord2(uvs.A, uvs.D);
			GL.Vertex2(x, y + height);
			GL.End();
		}

		public static void DrawPanel(float x, float y, float width, float height, string textureName)
		{
			Renderer.DrawPanel(x, y, width, height, textureName, System.Drawing.Color.White);
		}

		public static void DrawPanel(float x, float y, float width, float height, string textureName, Color4 color)
		{
			if (Renderer.textureAtlas.Exists((Texture i) => i.Name == textureName))
			{
				Texture texture = Renderer.textureAtlas.Find((Texture i) => i.Name == textureName);
				TextureUVW uvs = new TextureUVW(0.33f, 0.33f, 0.66f, 0.66f);
				TextureUVW uvs2 = new TextureUVW(0.33f, 0f, 0.66f, 0.33f);
				TextureUVW uvs3 = new TextureUVW(0.33f, 0.66f, 0.66f, 1f);
				TextureUVW uvs4 = new TextureUVW(0f, 0.33f, 0.33f, 0.66f);
				TextureUVW uvs5 = new TextureUVW(0.66f, 0.33f, 1f, 0.66f);
				TextureUVW uvs6 = new TextureUVW(0f, 0f, 0.33f, 0.33f);
				TextureUVW uvs7 = new TextureUVW(0.66f, 0f, 1f, 0.33f);
				TextureUVW uvs8 = new TextureUVW(0f, 0.66f, 0.33f, 1f);
				TextureUVW uvs9 = new TextureUVW(0.66f, 0.66f, 1f, 1f);
				float num = texture.Width * 0.33f;
				float num2 = texture.Height * 0.33f;
				Renderer.DrawTexturedRectangle(x + num, y + num2, width - num * 2f, height - num2 * 2f, textureName, color, uvs);
				Renderer.DrawTexturedRectangle(x + num, y, width - num * 2f, num2, textureName, color, uvs2);
				Renderer.DrawTexturedRectangle(x + num, y + height - num2, width - num * 2f, num2, textureName, color, uvs3);
				Renderer.DrawTexturedRectangle(x, y + num2, num, height - num2 * 2f, textureName, color, uvs4);
				Renderer.DrawTexturedRectangle(x + width - num, y + num2, num, height - num2 * 2f, textureName, color, uvs5);
				Renderer.DrawTexturedRectangle(x, y, num, num2, textureName, color, uvs6);
				Renderer.DrawTexturedRectangle(x + width - num, y, num, num2, textureName, color, uvs7);
				Renderer.DrawTexturedRectangle(x, y + height - num2, num, num2, textureName, color, uvs8);
				Renderer.DrawTexturedRectangle(x + width - num, y + height - num2, num, num2, textureName, color, uvs9);
			}
		}

		public static void SetClippingPlane(float x, float y, float w, float h)
		{
			GL.ClipPlane(ClipPlaneName.ClipDistance0, new double[]
			{
				(double)x,
				(double)y,
				(double)w,
				(double)h
			});
		}

		public static void DrawCube(Vector3 position, Vector3 rotation, Vector3 vecScale, Texture[] textures)
		{
			GL.PushMatrix();
			GL.Translate(position);
			GL.Rotate(rotation.X, 1f, 0f, 0f);
			GL.Rotate(rotation.Y, 0f, 1f, 0f);
			GL.Rotate(rotation.Z, 0f, 0f, 1f);
			GL.Color3(1f, 1f, 1f);
			GL.BindTexture(TextureTarget.Texture2D, textures[0].ID);
			GL.Color3(1f, 1f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.End();
			GL.BindTexture(TextureTarget.Texture2D, textures[1].ID);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.BindTexture(TextureTarget.Texture2D, textures[2].ID);
			GL.Color3(1f, 1f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.BindTexture(TextureTarget.Texture2D, textures[3].ID);
			GL.Color3(1f, 1f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.End();
			GL.BindTexture(TextureTarget.Texture2D, textures[4].ID);
			GL.Color3(1f, 1f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.End();
			GL.BindTexture(TextureTarget.Texture2D, textures[5].ID);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.Y, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawTestCube(Vector3 position, Vector3 rotation, Vector3 vecScale)
		{
			GL.PushMatrix();
			GL.Translate(position);
			GL.Rotate(rotation.X, 1f, 0f, 0f);
			GL.Rotate(rotation.Y, 0f, 1f, 0f);
			GL.Rotate(rotation.Z, 0f, 0f, 1f);
			GL.Color3(1f, 0f, 0f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.End();
			GL.Color3(0f, 1f, 0f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.Color3(0f, 0f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.Color3(1f, 1f, 0f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.End();
			GL.Color3(0f, 1f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.X, vecScale.Y, vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, vecScale.Y, -vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, vecScale.Y, -vecScale.Y);
			GL.End();
			GL.Color3(1f, 0f, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(-vecScale.X, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 0);
			GL.Vertex3(vecScale.Y, -vecScale.Y, -vecScale.Y);
			GL.TexCoord2(1, 1);
			GL.Vertex3(vecScale.X, -vecScale.Y, vecScale.Y);
			GL.TexCoord2(0, 1);
			GL.Vertex3(-vecScale.X, -vecScale.Y, vecScale.Y);
			GL.End();
			GL.PopMatrix();
		}
	}
}
