using OpenTK;
using OpenTK.Graphics;
using System;
using System.Drawing;
using WeaponsGame.UI;

namespace WeaponsGame.Game.UI
{
	internal class mission_resultpanel
	{
		private GUIManager manager;

		private Panel panel;

		private TextLabel text1;

		private Button button1;

		public mission_resultpanel(GUIManager manager)
		{
			this.manager = manager;
		}

		public void Construct()
		{
			this.panel = new Panel("missionResultPanel");
			this.panel.position = new Vector2((float)(Engine.BaseWindow.Width / 2 - 200), (float)(Engine.BaseWindow.Height / 2 - 150));
			this.panel.bounds = new System.Drawing.Rectangle(0, 0, 400, 300);
			this.panel.panelTexture = "panel1";
			this.panel.DrawBackground = true;
			this.panel.ShowHotbarControl = false;
			this.panel.Focusable = false;
			this.panel.Moveable = false;
			this.panel.Closable = false;
			this.panel.Text = "";
			this.panel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.panel.Alpha = 1f;
			this.text1 = new TextLabel("resultText");
			this.text1.SetFont(Renderer.GetFont("Test18"));
			this.text1.position = new Vector2((float)(this.panel.bounds.Width / 2), 0f);
			this.text1.align = TextAlignment.Center;
			this.text1.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.text1.Text = "Mission Results";
			this.button1 = new Button("returnButton");
			this.button1.SetFont(Renderer.GetFont("Test18"));
			this.button1.position = new Vector2((float)(this.panel.bounds.Width / 2 - 125), (float)this.panel.bounds.Height - this.panel.edgePadding.Y - 48f);
			this.button1.bounds = new System.Drawing.Rectangle(0, 0, 250, 32);
			this.button1.align = TextAlignment.Center;
			this.button1.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.button1.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.button1.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.button1.Text = "Return to Base";
			this.button1.pressSound = "ui/button_press.ogg";
			this.panel.AddControl(this.text1);
			this.panel.AddControl(this.button1);
			this.manager.RegisterPanel(this.panel);
		}
	}
}
