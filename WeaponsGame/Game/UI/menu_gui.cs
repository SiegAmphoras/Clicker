using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;
using WeaponsGame.UI;

namespace WeaponsGame.Game.UI
{
	internal class menu_gui
	{
		private GUIManager manager;

		private Panel menuPanel;

		private Button missionListButton;

		private Button personnelButton;

		private Button rndButton;

		private Button quitButton;

		private Panel statsPanel;

		private TextLabel statsCashLabel;

		private TextLabel statsPersonnelLabel;

		private Panel missionListPanel;

		private ListBox missionListBox;

		private Button missionListStartButton;

		public menu_gui(GUIManager manager)
		{
			this.manager = manager;
		}

		public void Construct()
		{
			this.menuPanel = new Panel("menuPanel");
			this.menuPanel.position = new Vector2(16f, (float)(Engine.BaseWindow.Height / 2 - 200));
			this.menuPanel.bounds = new System.Drawing.Rectangle(0, 0, 250, 400);
			this.menuPanel.panelTexture = "panel1";
			this.menuPanel.DrawBackground = true;
			this.menuPanel.ShowHotbarControl = false;
			this.menuPanel.Focusable = false;
			this.menuPanel.Moveable = false;
			this.menuPanel.Closable = false;
			this.menuPanel.Text = "Base Menu";
			this.menuPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.menuPanel.Alpha = 1f;
			this.menuPanel.titleFont = Renderer.GetFont("Test12");
			this.missionListButton = new Button("missionListButton");
			this.missionListButton.SetFont(Renderer.GetFont("Test18"));
			this.missionListButton.position = new Vector2(0f, 0f);
			this.missionListButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			this.missionListButton.align = TextAlignment.Center;
			this.missionListButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.missionListButton.Text = "Mission List";
			this.missionListButton.pressSound = "ui/button_press_sub.ogg";
			this.missionListButton.Clicked += new Button.ClickedEvent(this.missionListButton_Clicked);
			this.personnelButton = new Button("personnelButton");
			this.personnelButton.SetFont(Renderer.GetFont("Test18"));
			this.personnelButton.position = new Vector2(0f, 48f);
			this.personnelButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			this.personnelButton.align = TextAlignment.Center;
			this.personnelButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.personnelButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.personnelButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.personnelButton.Text = "Personnel";
			this.personnelButton.pressSound = "ui/button_press.ogg";
			this.personnelButton.Clicked += new Button.ClickedEvent(this.personnelButton_Clicked);
			this.rndButton = new Button("rndButton");
			this.rndButton.SetFont(Renderer.GetFont("Test18"));
			this.rndButton.position = new Vector2(0f, 96f);
			this.rndButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			this.rndButton.align = TextAlignment.Center;
			this.rndButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.rndButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.rndButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.rndButton.Text = "R & D";
			this.rndButton.pressSound = "ui/button_press.ogg";
			this.rndButton.Clicked += new Button.ClickedEvent(this.rndButton_Clicked);
			this.quitButton = new Button("quitButton");
			this.quitButton.SetFont(Renderer.GetFont("Test18"));
			this.quitButton.position = new Vector2(0f, (float)this.menuPanel.bounds.Height - this.menuPanel.edgePadding.Y - 48f);
			this.quitButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			this.quitButton.align = TextAlignment.Center;
			this.quitButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.quitButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.quitButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.quitButton.Text = "Quit :(";
			this.quitButton.pressSound = "ui/button_press.ogg";
			this.menuPanel.AddControl(this.personnelButton);
			this.menuPanel.AddControl(this.missionListButton);
			this.menuPanel.AddControl(this.rndButton);
			this.menuPanel.AddControl(this.quitButton);
			this.statsPanel = new Panel("statsPanel");
			this.statsPanel.position = new Vector2(16f, 16f);
			this.statsPanel.bounds = new System.Drawing.Rectangle(0, 0, Engine.BaseWindow.Width - 32, 48);
			this.statsPanel.panelTexture = "panel1";
			this.statsPanel.DrawBackground = true;
			this.statsPanel.ShowHotbarControl = false;
			this.statsPanel.Focusable = false;
			this.statsPanel.Moveable = false;
			this.statsPanel.Closable = false;
			this.statsPanel.Text = "Stats";
			this.statsPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.statsPanel.Alpha = 1f;
			this.statsPanel.titleFont = Renderer.GetFont("Test12");
			this.statsCashLabel = new TextLabel("statsCashLabel");
			this.statsCashLabel.SetFont(Renderer.GetFont("Test18"));
			this.statsCashLabel.position = new Vector2((float)(this.statsPanel.bounds.Width - 200), -16f);
			this.statsCashLabel.align = TextAlignment.Left;
			this.statsCashLabel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.statsCashLabel.Text = "$:";
			this.statsPersonnelLabel = new TextLabel("statsPersonnelLabel");
			this.statsPersonnelLabel.SetFont(Renderer.GetFont("Test18"));
			this.statsPersonnelLabel.position = new Vector2((float)(this.statsPanel.bounds.Width - 300), -16f);
			this.statsPersonnelLabel.align = TextAlignment.Left;
			this.statsPersonnelLabel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.statsPersonnelLabel.Text = "Personnel:";
			this.statsPanel.AddControl(this.statsCashLabel);
			this.statsPanel.AddControl(this.statsPersonnelLabel);
			this.manager.RegisterPanel(this.menuPanel);
			this.manager.RegisterPanel(this.statsPanel);
		}

		private void ConstructMissionListPanel()
		{
			this.missionListPanel = new Panel("missionListPanel");
			this.missionListPanel.position = new Vector2((float)(Engine.BaseWindow.Width / 6 * 4 - 240), (float)(Engine.BaseWindow.Height / 2 - 200));
			this.missionListPanel.bounds = new System.Drawing.Rectangle(0, 0, 500, 400);
			this.missionListPanel.panelTexture = "panel1";
			this.missionListPanel.DrawBackground = true;
			this.missionListPanel.ShowHotbarControl = true;
			this.missionListPanel.Focusable = true;
			this.missionListPanel.Moveable = true;
			this.missionListPanel.Closable = true;
			this.missionListPanel.Text = "Mission List";
			this.missionListPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListPanel.Alpha = 1f;
			this.missionListPanel.titleFont = Renderer.GetFont("Test12");
			this.missionListStartButton = new Button("missionListStartButton");
			this.missionListStartButton.SetFont(Renderer.GetFont("Test18"));
			this.missionListStartButton.position = new Vector2((float)(this.missionListPanel.bounds.Width - 224 - 16), 116f);
			this.missionListStartButton.bounds = new System.Drawing.Rectangle(0, 0, 224, 32);
			this.missionListStartButton.align = TextAlignment.Center;
			this.missionListStartButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListStartButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListStartButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			this.missionListStartButton.Text = "Start Mission";
			this.missionListStartButton.pressSound = "ui/button_press.ogg";
			this.missionListStartButton.Clicked += new Button.ClickedEvent(this.missionListStartButton_Clicked);
			this.missionListBox = new ListBox("missionListBox");
			this.missionListBox.position = new Vector2(0f, (float)this.missionListPanel.bounds.Height - this.missionListPanel.edgePadding.Y - 170f - 32f);
			this.missionListBox.bounds = new System.Drawing.Rectangle(0, 0, 485, 180);
			this.missionListBox.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			this.missionListBox.Text = "Missions";
			foreach (Mission current in Engine.game.missionsAvailable)
			{
				this.missionListBox.Items.Add(string.Concat(new object[]
				{
					"Level ",
					current.level,
					" Mission in ",
					current.environment
				}));
			}
			this.missionListPanel.AddControl(this.missionListStartButton);
			this.missionListPanel.AddControl(this.missionListBox);
			this.manager.RegisterPanel(this.missionListPanel);
		}

		private void missionListStartButton_Clicked(MouseButtonEventArgs e)
		{
			if (!e.IsPressed)
			{
				Mission mission = Engine.game.missionsAvailable[this.missionListBox.SelectedIndex];
				Engine.game.missionsAvailable.Remove(mission);
				Engine.game.activeMission = mission;
				Engine.sceneManager.SetActiveScene("MissionScene");
			}
		}

		private void rndButton_Clicked(MouseButtonEventArgs e)
		{
		}

		private void personnelButton_Clicked(MouseButtonEventArgs e)
		{
		}

		private void missionListButton_Clicked(MouseButtonEventArgs e)
		{
			this.ConstructMissionListPanel();
		}
	}
}
