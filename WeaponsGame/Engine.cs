using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using WeaponsGame.Game;

namespace WeaponsGame
{
	internal class Engine
	{
		public static Engine reference;

		private static GameWindow gw;

		public static Renderer renderer;

		public static SceneManager sceneManager;

		public static Input input;

		public static Audio audio;

		public static Game.Game game;

		public static float DeltaTime;

		public static float ElapsedTime;

		public static int randSeed = 4589135;

		public static System.Random rand;

		private static System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<DelayedFunction, float>> delayFunctions;

		private static System.Collections.Generic.List<int> removedDelayFunctions;

		public static GameWindow BaseWindow
		{
			get
			{
				return Engine.gw;
			}
		}

		public static Scene ActiveScene
		{
			get
			{
				return Engine.sceneManager.GetActiveScene();
			}
		}

		public Engine()
		{
			Engine.gw = new GameWindow(800, 600, GraphicsMode.Default, "Weapons", GameWindowFlags.FixedWindow, DisplayDevice.GetDisplay(DisplayIndex.First), 3, 1, GraphicsContextFlags.Default);
			Engine.gw.Location = new System.Drawing.Point(0, 0);
			Engine.gw.Load += new System.EventHandler<System.EventArgs>(this.gw_Load);
			Engine.gw.UpdateFrame += new System.EventHandler<FrameEventArgs>(this.gw_UpdateFrame);
			Engine.gw.RenderFrame += new System.EventHandler<FrameEventArgs>(this.gw_RenderFrame);
			Engine.gw.Closing += new System.EventHandler<System.ComponentModel.CancelEventArgs>(this.gw_Closing);
			Engine.reference = this;
		}

		private void gw_Load(object sender, System.EventArgs e)
		{
			Engine.delayFunctions = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<DelayedFunction, float>>();
			Engine.removedDelayFunctions = new System.Collections.Generic.List<int>();
			Engine.input = new Input(ref Engine.gw);
			Engine.audio = new Audio();
			Engine.input.Mouse.ButtonDown += new System.EventHandler<MouseButtonEventArgs>(this.Mouse_ButtonDown);
			Engine.input.Mouse.ButtonUp += new System.EventHandler<MouseButtonEventArgs>(this.Mouse_ButtonUp);
			Engine.renderer = new Renderer();
			Engine.renderer.PrecacheTextures();
			Engine.renderer.PrecacheFonts();
			Engine.gw.CursorVisible = true;
			Engine.gw.Cursor = MouseCursor.Empty;
			Engine.gw.VSync = VSyncMode.Off;
			Engine.rand = new System.Random();
			Engine.sceneManager = new SceneManager();
			Engine.game = new Game.Game();
			Engine.game.Startup();
		}

		private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			Engine.sceneManager.GetActiveScene().guiManager.Mouse_ButtonUp(sender, e);
		}

		private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			Engine.sceneManager.GetActiveScene().guiManager.Mouse_ButtonDown(sender, e);
		}

		private void gw_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (Engine.gw.Focused)
			{
				Engine.gw.Title = "Weapons - fps: " + 1.0 / e.Time;
				Engine.input.Update();
				Engine.audio.Update();
				Engine.sceneManager.UpdateScene();
				try
				{
					foreach (System.Collections.Generic.KeyValuePair<DelayedFunction, float> current in Engine.delayFunctions)
					{
						if (Engine.ElapsedTime > current.Value)
						{
							current.Key();
							Engine.delayFunctions.Remove(current);
						}
					}
				}
				catch
				{
				}
				Engine.removedDelayFunctions.Clear();
				Engine.DeltaTime = (float)e.Time;
				Engine.ElapsedTime += (float)e.Time;
			}
		}

		private void gw_RenderFrame(object sender, FrameEventArgs e)
		{
			if (Engine.gw.Focused)
			{
				GL.ClearColor(Color4.Black);
				GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
				Engine.sceneManager.RenderScene();
				Engine.gw.SwapBuffers();
			}
		}

		public void Run()
		{
			Engine.gw.Run(60.0);
		}

		public void Quit()
		{
			Engine.gw.Close();
		}

		private void gw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		public static void DelayFunction(DelayedFunction del, float delay)
		{
			Engine.delayFunctions.Add(new System.Collections.Generic.KeyValuePair<DelayedFunction, float>(del, Engine.ElapsedTime + delay));
		}
	}
}
