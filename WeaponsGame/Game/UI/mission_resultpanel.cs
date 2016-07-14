using OpenTK;
using OpenTK.Graphics;
using System;
using System.Drawing;
using WeaponsGame.UI;

namespace WeaponsGame.Game.UI
{
	internal class mission_resultpanel
	{
		GUIManager manager;

		Panel panel;
		TextLabel text1;
		Button button1;

		public mission_resultpanel(GUIManager manager)
		{
			this.manager = manager;
		}

		public void Construct()
		{
			panel = new Panel("missionResultPanel");
			panel.position = new Vector2((float)(Engine.BaseWindow.Width / 2 - 200), (float)(Engine.BaseWindow.Height / 2 - 150));
			panel.bounds = new System.Drawing.Rectangle(0, 0, 400, 300);
			panel.panelTexture = "panel1";
			panel.DrawBackground = true;
			panel.ShowHotbarControl = false;
			panel.Focusable = false;
			panel.Moveable = false;
			panel.Closable = false;
			panel.Text = "";
			panel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			panel.Alpha = 1f;

			text1 = new TextLabel("resultText");
			text1.font = Renderer.GetFont("Test12");
			text1.position = new Vector2((float)(panel.bounds.Width / 2), 0f);
			text1.align = TextAlignment.Center;
			text1.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			text1.Text = "Mission Results";

            text1.Text += "\n\n\n";

            text1.Text += string.Format("Enemies Elimated: {0}/{1}\n\n\n", Engine.game.activeMission.enemiesKilled, Engine.game.activeMission.enemyCount);

            if (Engine.game.activeMission.DidPlayerWin)
            {
                text1.Text += "Rewards:\n";

                foreach (MissionReward r in Engine.game.activeMission.Rewards)
                {
                    text1.Text += "\n " + r.GetRewardText();
                }
            }

			button1 = new Button("returnButton");
            button1.font = Renderer.GetFont("Test18");
			button1.position = new Vector2((float)(panel.bounds.Width / 2 - 125), (float)panel.bounds.Height - panel.edgePadding.Y - 48f);
			button1.bounds = new System.Drawing.Rectangle(0, 0, 250, 32);
			button1.align = TextAlignment.Center;
			button1.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			button1.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			button1.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			button1.Text = "Return to Base";
			button1.pressSound = "ui/button_press.ogg";

			panel.AddControl(text1);
			panel.AddControl(button1);

			manager.RegisterPanel(panel);
		}
	}
}
