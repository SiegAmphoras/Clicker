using OpenTK;
using OpenTK.Input;
using System;
using System.Drawing;
using WeaponsGame.UI;

namespace WeaponsGame.Scenes
{
	internal class TestScene : Scene
	{
		private Panel panel;

		private Panel titlepanel;

		private float skyboxPitch = 0f;

		private float skyboxYaw = 0f;

		private System.Drawing.Color[] colors = new System.Drawing.Color[]
		{
			System.Drawing.Color.Green,
			System.Drawing.Color.Red,
			System.Drawing.Color.Blue
		};

		private int col = 0;

		public TestScene()
		{
			this.panel = new Panel("MainMenuPanel");
			this.panel.position = new Vector2(40f, 120f);
			this.panel.bounds.Width = 200;
			this.panel.bounds.Height = 400;
			this.panel.Moveable = false;
			this.panel.ShowHotbarControl = false;
			this.panel.Text = "Main Menu";
			this.titlepanel = new Panel("titlePanel");
			this.titlepanel.position = new Vector2(0f, 0f);
			this.titlepanel.bounds.Width = Engine.BaseWindow.ClientSize.Width;
			this.titlepanel.bounds.Height = Engine.BaseWindow.ClientSize.Height;
			this.titlepanel.DrawBackground = false;
			this.titlepanel.Moveable = false;
			this.titlepanel.ShowHotbarControl = false;
			this.titlepanel.Focusable = false;
			TextLabel textLabel = new TextLabel("titleLabel");
			textLabel.position = Vector2.Zero;
			textLabel.SetFont(Renderer.GetFont("Test64"));
			textLabel.tint = System.Drawing.Color.White;
			textLabel.Text = "SpaceStation 5";
			this.titlepanel.AddControl(textLabel);
			Button button = new Button("newGameButton");
			button.Text = "New Game";
			button.position = new Vector2(0f, 0f);
			button.bounds.Width = this.panel.bounds.Width - (int)(this.panel.ControlEdgePadding.X * 2f);
			button.Clicked += new Button.ClickedEvent(this.button1_Clicked);
			this.panel.AddControl(button);
			this.guiManager.RegisterPanel(this.panel);
			this.guiManager.RegisterPanel(this.titlepanel);
		}

		private void button1_Clicked(MouseButtonEventArgs e)
		{
			this.col++;
			this.col %= this.colors.Length;
			this.titlepanel["titleLabel"].tint = this.colors[this.col];
		}

		public override void Update()
		{
			this.skyboxYaw -= 0.125f * Engine.DeltaTime;
			if (Engine.input.IsButtonDown(MouseButton.Right) && !Engine.input.WasButtonDown(MouseButton.Right))
			{
				Panel panel = new Panel("panel");
				panel.position = new Vector2((float)Engine.input.Mouse.X, (float)Engine.input.Mouse.Y);
				panel.Text = "This is panel " + (this.guiManager.GetPanelCount() + 1);
				TextLabel textLabel = new TextLabel("text");
				textLabel.position = Vector2.Zero;
				textLabel.Text = "abcdefghijklmnopqrstuvwxyz";
				textLabel.SetFont(Renderer.GetFont("Test18"));
				panel.AddControl(textLabel);
				this.guiManager.RegisterPanel(panel);
			}
			base.Update();
		}

		public override void Render()
		{
			Engine.renderer.Prepare3D();
			Texture[] skyboxTextures = Renderer.GetSkyboxTextures("nebula1_");
			Renderer.DrawCube(new Vector3(0f, 0f, 0f), new Vector3(this.skyboxPitch, this.skyboxYaw, 0f), Vector3.One * 10f, skyboxTextures);
			Engine.renderer.End3D();
			Engine.renderer.Prepare2D();
			base.Render();
			Renderer.DrawTexturedRectangle((float)(Engine.input.Mouse.X - 2), (float)(Engine.input.Mouse.Y - 2), 20f, 27f, "cursor_pointer3D_shadow");
			Engine.renderer.End2D();
		}
	}
}
