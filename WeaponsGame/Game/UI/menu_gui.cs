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
		GUIManager manager;

		Panel menuPanel;
		Button missionListButton;
		Button personnelButton;
		Button rndButton;
		Button quitButton;

		Panel statsPanel;
		TextLabel statsCashLabel;
		TextLabel statsPersonnelLabel;

		Panel missionListPanel;
        TextLabel rewardListText;
		ListBox missionListBox;
		Button missionListStartButton;

        private Panel rndPanel;

        private ListBox rndListBox;

		public menu_gui(GUIManager manager)
		{
			this.manager = manager;
		}

		public void Construct()
		{
			menuPanel = new Panel("menuPanel");
			menuPanel.position = new Vector2(16f, (float)(Engine.BaseWindow.Height / 2 - 200));
			menuPanel.bounds = new System.Drawing.Rectangle(0, 0, 250, 400);
			menuPanel.panelTexture = "panel1";
			menuPanel.DrawBackground = true;
			menuPanel.ShowHotbarControl = false;
			menuPanel.Focusable = false;
			menuPanel.Moveable = false;
			menuPanel.Closable = false;
			menuPanel.Text = "Base Menu";
			menuPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			menuPanel.Alpha = 1f;
			menuPanel.titleFont = Renderer.GetFont("Test12");

			missionListButton = new Button("missionListButton");
			missionListButton.font = (Renderer.GetFont("Test18"));
			missionListButton.position = new Vector2(0f, 0f);
			missionListButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			missionListButton.align = TextAlignment.Center;
			missionListButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			missionListButton.Text = "Mission List";
			missionListButton.pressSound = "ui/button_press_sub.ogg";
			missionListButton.Clicked += new Button.ClickedEvent(missionListButton_Clicked);

			personnelButton = new Button("personnelButton");
			personnelButton.font = (Renderer.GetFont("Test18"));
			personnelButton.position = new Vector2(0f, 48f);
			personnelButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			personnelButton.align = TextAlignment.Center;
			personnelButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			personnelButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			personnelButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			personnelButton.Text = "Personnel";
			personnelButton.pressSound = "ui/button_press.ogg";
			personnelButton.Clicked += new Button.ClickedEvent(personnelButton_Clicked);

			rndButton = new Button("rndButton");
			rndButton.font = (Renderer.GetFont("Test18"));
			rndButton.position = new Vector2(0f, 96f);
			rndButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			rndButton.align = TextAlignment.Center;
			rndButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			rndButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			rndButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			rndButton.Text = "R & D";
			rndButton.pressSound = "ui/button_press.ogg";
			rndButton.Clicked += new Button.ClickedEvent(rndButton_Clicked);

			quitButton = new Button("quitButton");
			quitButton.font = (Renderer.GetFont("Test18"));
			quitButton.position = new Vector2(0f, (float)menuPanel.bounds.Height - menuPanel.edgePadding.Y - 48f);
			quitButton.bounds = new System.Drawing.Rectangle(0, 0, 230, 32);
			quitButton.align = TextAlignment.Center;
			quitButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			quitButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			quitButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			quitButton.Text = "Quit :(";
			quitButton.pressSound = "ui/button_press.ogg";

			menuPanel.AddControl(personnelButton);
			menuPanel.AddControl(missionListButton);
			menuPanel.AddControl(rndButton);
			menuPanel.AddControl(quitButton);

			statsPanel = new Panel("statsPanel");
			statsPanel.position = new Vector2(16f, 16f);
			statsPanel.bounds = new System.Drawing.Rectangle(0, 0, Engine.BaseWindow.Width - 32, 48);
			statsPanel.panelTexture = "panel1";
			statsPanel.DrawBackground = true;
			statsPanel.ShowHotbarControl = false;
			statsPanel.Focusable = false;
			statsPanel.Moveable = false;
			statsPanel.Closable = false;
			statsPanel.Text = "Stats";
			statsPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			statsPanel.Alpha = 1f;
			statsPanel.titleFont = Renderer.GetFont("Test12");

			statsCashLabel = new TextLabel("statsCashLabel");
			statsCashLabel.font = Renderer.GetFont("Test18");
			statsCashLabel.position = new Vector2((float)(statsPanel.bounds.Width - 200), -16f);
			statsCashLabel.align = TextAlignment.Left;
			statsCashLabel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			statsCashLabel.Text = "$:";

			statsPersonnelLabel = new TextLabel("statsPersonnelLabel");
			statsPersonnelLabel.font = Renderer.GetFont("Test18");
			statsPersonnelLabel.position = new Vector2((float)(statsPanel.bounds.Width - 300), -16f);
			statsPersonnelLabel.align = TextAlignment.Left;
			statsPersonnelLabel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			statsPersonnelLabel.Text = "Personnel:";

			statsPanel.AddControl(statsCashLabel);
			statsPanel.AddControl(statsPersonnelLabel);

			manager.RegisterPanel(menuPanel);
			manager.RegisterPanel(statsPanel);
		}

		private void ConstructMissionListPanel()
		{
			missionListPanel = new Panel("missionListPanel");
			missionListPanel.position = new Vector2((float)(Engine.BaseWindow.Width / 6 * 4 - 240), (float)(Engine.BaseWindow.Height / 2 - 200));
			missionListPanel.bounds = new Rectangle(0, 0, 600, 500);
			missionListPanel.panelTexture = "panel1";
			missionListPanel.DrawBackground = true;
			missionListPanel.ShowHotbarControl = true;
			missionListPanel.Focusable = true;
			missionListPanel.Moveable = true;
			missionListPanel.Closable = true;
			missionListPanel.Text = "Mission List";
			missionListPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListPanel.Alpha = 1f;
			missionListPanel.titleFont = Renderer.GetFont("Test12");

            rewardListText = new TextLabel("rewardListText");
            rewardListText.position = new Vector2(0f, 0f);
            rewardListText.bounds = new Rectangle(0, 0, 500, 400);
            rewardListText.Text = "";
            rewardListText.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
            rewardListText.font = (Renderer.GetFont("Test12"));

			missionListStartButton = new Button("missionListStartButton");
            missionListStartButton.font = (Renderer.GetFont("Test18"));
            missionListStartButton.position = new Vector2((float)(missionListPanel.bounds.Width - 224 - 16), (float)missionListPanel.bounds.Height - missionListPanel.edgePadding.Y - 215f - 32f);
			missionListStartButton.bounds = new Rectangle(0, 0, 224, 32);
			missionListStartButton.align = TextAlignment.Center;
			missionListStartButton.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListStartButton.TextUpTint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListStartButton.TextDownTint = new Color4(0f, 0f, 0f, 1f);
			missionListStartButton.Text = "Start Mission";
			missionListStartButton.pressSound = "ui/button_press.ogg";
			missionListStartButton.Clicked += new Button.ClickedEvent(missionListStartButton_Clicked);

			missionListBox = new ListBox("missionListBox");
			missionListBox.position = new Vector2(0f, (float)missionListPanel.bounds.Height - missionListPanel.edgePadding.Y - 170f - 32f);
			missionListBox.bounds = new Rectangle(0, 0, 585, 180);
			missionListBox.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
			missionListBox.Text = "Missions";
            missionListBox.OnSelectedIndexChanged += missionListBox_OnSelectedIndexChanged;

			foreach (Mission current in Engine.game.missionsAvailable)
			{
				missionListBox.Items.Add(string.Concat(new object[]
				{
					"Level ",
					current.level,
					" Mission in ",
					current.environment
				}));
			}

			missionListPanel.AddControl(missionListStartButton);
			missionListPanel.AddControl(missionListBox);
            missionListPanel.AddControl(rewardListText);

			manager.RegisterPanel(missionListPanel);
		}

        private void ConstructRndPanel()
        {
            
            this.rndPanel = new Panel("rndPanel");
            this.rndPanel.position = new Vector2((float)(Engine.BaseWindow.Width / 6 * 4 - 240), (float)(Engine.BaseWindow.Height / 2 - 160));
            this.rndPanel.bounds = new System.Drawing.Rectangle(0, 0, 500, 400);
            this.rndPanel.panelTexture = "panel1";
            this.rndPanel.DrawBackground = true;
            this.rndPanel.ShowHotbarControl = true;
            this.rndPanel.Focusable = true;
            this.rndPanel.Moveable = true;
            this.rndPanel.Closable = true;
            this.rndPanel.Text = "Research & Development";
            this.rndPanel.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
            this.rndPanel.Alpha = 1f;
            this.rndPanel.titleFont = Renderer.GetFont("Test12");

            this.rndListBox = new ListBox("rndListBox");
            this.rndListBox.position = new Vector2(0f, (float)this.rndPanel.edgePadding.Y - 50f);
            this.rndListBox.bounds = new System.Drawing.Rectangle(0, 0, 485, 350);
            this.rndListBox.tint = new Color4(0.6f, 0.8f, 0.6f, 1f);
            this.rndListBox.Text = "Blueprints";

            foreach (Blueprint bloo in Engine.game.blueprints)
            {
                this.rndListBox.Items.Add(string.Concat(new object[]
				{

                    bloo.type,
                    " ",
                    bloo.name,
					" $",
                    bloo.cost,
					
				}));
            }

            this.rndPanel.AddControl(this.rndListBox);
            this.manager.RegisterPanel(this.rndPanel);
        }

        void missionListBox_OnSelectedIndexChanged(MouseButtonEventArgs e)
        {
            Mission mission = Engine.game.missionsAvailable[missionListBox.SelectedIndex];

            rewardListText.Text = "Mission Details:\n\n";

            rewardListText.Text += "Enemies: " + mission.enemyCount + "\n\n\n";

            rewardListText.Text += "Rewards:\n";
            foreach(MissionReward r in mission.Rewards)
            {
                rewardListText.Text += "\n " + r.GetRewardText();
            }
        }

		private void missionListStartButton_Clicked(MouseButtonEventArgs e)
		{
			if (!e.IsPressed)
			{
				Mission mission = Engine.game.missionsAvailable[missionListBox.SelectedIndex];
				Engine.game.missionsAvailable.Remove(mission);
				Engine.game.activeMission = mission;
				Engine.sceneManager.SetActiveScene("MissionScene");
			}
		}

		private void rndButton_Clicked(MouseButtonEventArgs e)
		{
           // System.Diagnostics.Debug.WriteLine("asdf");
            this.ConstructRndPanel();
		}

		private void personnelButton_Clicked(MouseButtonEventArgs e)
		{
		}

		private void missionListButton_Clicked(MouseButtonEventArgs e)
		{
			ConstructMissionListPanel();
		}
	}
}
