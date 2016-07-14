using OpenTK;
using OpenTK.Graphics;
using System;
using System.Drawing;
using WeaponsGame.UI;

namespace WeaponsGame.Game.UI
{
	internal class mission_hud
	{
		private GUIManager manager;

		private Panel hudPanel;

		private TextureButton grenadeButton;

		private TextureButton reloadButton;

		private TextureButton healthkitButton;

		private TextureButton armorButton;

		private TextLabel grenadeLabel;

		private TextLabel healthkitLabel;

		private TextLabel armorButtonLabel;

		private TextLabel healthLabel;

		private TextLabel ammoLabel;

		private TextLabel armorLabel;

		private Panel topPanel;

		private TextLabel enemyCounter;

		public mission_hud(GUIManager manager)
		{
			this.manager = manager;
		}

		public void Construct()
		{
			this.hudPanel = new Panel("hudPanel");
			this.hudPanel.position = new Vector2(0f, (float)(Engine.BaseWindow.ClientSize.Height - 96));
			this.hudPanel.bounds = new System.Drawing.Rectangle(0, 0, Engine.BaseWindow.ClientSize.Width, 96);
			this.hudPanel.panelTexture = "quad";
			this.hudPanel.DrawBackground = true;
			this.hudPanel.ShowHotbarControl = false;
			this.hudPanel.Focusable = false;
			this.hudPanel.Moveable = false;
			this.hudPanel.Closable = false;
			this.hudPanel.Text = "";
			this.hudPanel.tint = new Color4(0f, 0f, 0f, 1f);
			this.hudPanel.Alpha = 0.5f;
			this.grenadeButton = new TextureButton("grenadeButton");
			this.grenadeButton.position = new Vector2((float)(Engine.BaseWindow.ClientSize.Width - 96), -16f);
			this.grenadeButton.bounds = new System.Drawing.Rectangle(0, 0, 64, 64);
			this.grenadeButton.buttonTexture = "GrenadeIcon";
			this.reloadButton = new TextureButton("reloadButton");
			this.reloadButton.position = new Vector2((float)(Engine.BaseWindow.ClientSize.Width - 176), -16f);
			this.reloadButton.bounds = new System.Drawing.Rectangle(0, 0, 64, 64);
			this.reloadButton.buttonTexture = "MagIcon";
			this.healthkitButton = new TextureButton("healthkitButton");
			this.healthkitButton.position = new Vector2(16f, -16f);
			this.healthkitButton.bounds = new System.Drawing.Rectangle(0, 0, 64, 64);
			this.healthkitButton.buttonTexture = "MedkitIcon";
			this.armorButton = new TextureButton("armorButton");
			this.armorButton.position = new Vector2(88f, -16f);
			this.armorButton.bounds = new System.Drawing.Rectangle(0, 0, 64, 64);
			this.armorButton.buttonTexture = "ArmorIcon";
			this.grenadeLabel = new TextLabel("grenadeLabel");
			this.grenadeLabel.font = Renderer.GetFont("Test32");
			this.grenadeLabel.position = this.grenadeButton.position + new Vector2(48f, 48f);
			this.grenadeLabel.tint = System.Drawing.Color.White;
			this.healthkitLabel = new TextLabel("healthkitLabel");
			this.healthkitLabel.font = Renderer.GetFont("Test32");
			this.healthkitLabel.position = this.healthkitButton.position + new Vector2(48f, 48f);
			this.healthkitLabel.tint = System.Drawing.Color.White;
			this.armorButtonLabel = new TextLabel("armorButtonLabel");
			this.armorButtonLabel.font = Renderer.GetFont("Test32");
			this.armorButtonLabel.position = this.armorButton.position + new Vector2(48f, 48f);
			this.armorButtonLabel.tint = System.Drawing.Color.White;
			this.healthLabel = new TextLabel("healthLabel");
			this.healthLabel.font = Renderer.GetFont("Test32");
			this.healthLabel.align = TextAlignment.Center;
			this.healthLabel.position = new Vector2((float)(Engine.BaseWindow.Width / 2), 0f);
			this.healthLabel.Text = Engine.game.localPlayer.health.ToString();
			this.healthLabel.tint = new Color4(0f, 1f, 0.25f, 1f);
			this.armorLabel = new TextLabel("armorLabel");
			this.armorLabel.font = Renderer.GetFont("Test32");
			this.armorLabel.align = TextAlignment.Center;
			this.armorLabel.position = new Vector2((float)(Engine.BaseWindow.Width / 2), 25f);
			this.armorLabel.Text = Engine.game.localPlayer.armor.ToString();
			this.armorLabel.tint = new Color4(0.9f, 0.3f, 0.3f, 1f);
			this.ammoLabel = new TextLabel("ammoLabel");
			this.ammoLabel.font = Renderer.GetFont("Test32");
			this.ammoLabel.align = TextAlignment.Right;
			this.ammoLabel.position = this.reloadButton.position - new Vector2(64f, 0f);
			this.ammoLabel.Text = Engine.game.localPlayer.currentAmmo.ToString();
			this.ammoLabel.tint = new Color4(0.9f, 0.9f, 0.9f, 1f);
			this.hudPanel.AddControl(this.grenadeButton);
			this.hudPanel.AddControl(this.reloadButton);
			this.hudPanel.AddControl(this.healthkitButton);
			this.hudPanel.AddControl(this.armorButton);
			this.hudPanel.AddControl(this.healthkitLabel);
			this.hudPanel.AddControl(this.armorButtonLabel);
			this.hudPanel.AddControl(this.grenadeLabel);
			this.hudPanel.AddControl(this.healthLabel);
			this.hudPanel.AddControl(this.ammoLabel);
			this.hudPanel.AddControl(this.armorLabel);
			this.manager.RegisterPanel(this.hudPanel);
			this.topPanel = new Panel("topPanel");
			this.topPanel.position = new Vector2(0f, 0f);
			this.topPanel.bounds = new System.Drawing.Rectangle(0, 0, Engine.BaseWindow.ClientSize.Width, 64);
			this.topPanel.panelTexture = "quad";
			this.topPanel.DrawBackground = false;
			this.topPanel.ShowHotbarControl = false;
			this.topPanel.Focusable = false;
			this.topPanel.Moveable = false;
			this.topPanel.Closable = false;
			this.topPanel.Text = "";
			this.enemyCounter = new TextLabel("enemyCounter");
			this.enemyCounter.font = Renderer.GetFont("Test18");
			this.enemyCounter.align = TextAlignment.Right;
			this.enemyCounter.position = new Vector2(200f, -16f);
			this.enemyCounter.tint = new Color4(0.9f, 0.9f, 0.9f, 1f);
			this.topPanel.AddControl(this.enemyCounter);
			this.manager.RegisterPanel(this.topPanel);
		}
	}
}
