using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using WeaponsGame.Game;
using WeaponsGame.Game.UI;
using WeaponsGame.UI;

namespace WeaponsGame.Scenes
{
	internal class MissionScene : Scene
	{
		private List<CharacterCard> enemies;

		private List<DamageCounter> damageCounters;

		private CharacterCard[] enemySlots;

		private ActionTimer reloadTimer;
        
        private int grenadeCount;
		private ActionTimer grenadeTimer;		

		private int medkitCount;
		private ActionTimer medkitTimer;

		private int armorCount;
		private ActionTimer armorTimer;

		private mission_hud hud;

		private mission_resultpanel result;

		private Rectangle firingBounds;

		private float lastSpawn = 0f;
		private float enemySpawnDelay = 2f;
		private int enemySlotCount = 1;

		private float nextBreakTime = 0f;
		private float breakTime = 5f;
		private bool BreakPeriod = false;

		private float lastReloadSoundTime = 0f;
		private float lastArmorSoundTime = 0f;
		private float lastMedkitSoundTime = 0f;

		private float deathBoxBaseAlpha = 0f;
		private float deathBoxAlpha = 0f;

		private int enemiesKilled = 0;

		private MissionBigtext bigText;

		private bool missionOver = false;
		private bool missionWon = false;
		private float missionEndTime = 0f;

		public override void Load()
		{
			base.Load();

			missionOver = false;
			missionWon = false;
			missionEndTime = 0f;
			enemiesKilled = 0;

			lastSpawn = Engine.ElapsedTime;

			firingBounds = new Rectangle(0, 0, Engine.BaseWindow.ClientSize.Width, Engine.BaseWindow.ClientSize.Height - 128);

			enemies = new List<CharacterCard>();

			enemies.AddRange(Engine.game.activeMission.GenerateMissionRoster());
			enemySlotCount = Engine.game.activeMission.GetSlotCount();
			enemySlots = new CharacterCard[this.enemySlotCount];

			damageCounters = new List<DamageCounter>();

			Engine.game.localPlayer.StartMission();

			grenadeCount = Engine.game.localPlayer.grenadeCount;
			grenadeTimer = new ActionTimer(Engine.game.localPlayer.grenadeThrowTime);
			grenadeTimer.OnFinish += new ActionTimerEventHandler(this.grenadeTimer_OnFinish);

			reloadTimer = new ActionTimer(Engine.game.localPlayer.currentWeapon.baseReloadTime);
			reloadTimer.OnFinish += new ActionTimerEventHandler(this.reloadTimer_OnFinish);

			medkitCount = Engine.game.localPlayer.medkitCount;
			medkitTimer = new ActionTimer(Engine.game.localPlayer.medkitUseTime);
			medkitTimer.OnFinish += new ActionTimerEventHandler(this.medkitTimer_OnFinish);

			armorCount = Engine.game.localPlayer.armorCount;
			armorTimer = new ActionTimer(Engine.game.localPlayer.armorUseTime);
			armorTimer.OnFinish += new ActionTimerEventHandler(this.armorTimer_OnFinish);

			hud = new mission_hud(this.guiManager);
			hud.Construct();

			result = new mission_resultpanel(this.guiManager);

			(guiManager["hudPanel"]["grenadeButton"] as TextureButton).Clicked += new TextureButton.ClickedEvent(this.grenadeButton_Clicked);
			(guiManager["hudPanel"]["reloadButton"] as TextureButton).Clicked += new TextureButton.ClickedEvent(this.reloadButton_Clicked);
			(guiManager["hudPanel"]["healthkitButton"] as TextureButton).Clicked += new TextureButton.ClickedEvent(this.healthkitButton_Clicked);
			(guiManager["hudPanel"]["armorButton"] as TextureButton).Clicked += new TextureButton.ClickedEvent(this.armorButton_Clicked);

			bigText = new MissionBigtext();
			bigText.Start();

			Engine.audio.PlaySound("mus/mission_start.wav");
			Engine.game.localPlayer.OnPlayerDamage += new PlayerDamageEvent(this.localPlayer_OnPlayerDamage);
			Engine.game.localPlayer.OnPlayerKilled += new PlayerDamageEvent(this.localPlayer_OnPlayerKilled);
		}

		private void ReturnButton_Clicked(MouseButtonEventArgs e)
		{
			Engine.game.PostMissionFinish(missionWon);
			Engine.sceneManager.SetActiveScene("BaseMenuScene");
		}

		private void armorButton_Clicked(MouseButtonEventArgs e)
		{
			this.ArmorButton();
		}

		private void healthkitButton_Clicked(MouseButtonEventArgs e)
		{
			this.HealthkitButton();
		}

		private void reloadButton_Clicked(MouseButtonEventArgs e)
		{
			this.ReloadButton();
		}

		private void grenadeButton_Clicked(MouseButtonEventArgs e)
		{
			this.GrenadeButton();
		}

		private void armorTimer_OnFinish(ActionTimer sender)
		{
			this.armorTimer.Enabled = false;
			Engine.game.localPlayer.UseArmor();
			this.armorCount--;
		}

		private void medkitTimer_OnFinish(ActionTimer sender)
		{
			this.medkitTimer.Enabled = false;
			Engine.game.localPlayer.UseMedkit();
			this.medkitCount--;
		}

		private void reloadTimer_OnFinish(ActionTimer sender)
		{
			this.reloadTimer.Enabled = false;
			Engine.game.localPlayer.EndReload();
		}

		private void grenadeTimer_OnFinish(ActionTimer sender)
		{
			Engine.audio.PlaySound("weapon/explosion2.wav");
			this.grenadeTimer.Enabled = false;
			this.grenadeCount--;
			this.GrenadeExplosion();
		}

		public void GrenadeExplosion()
		{
			CharacterCard[] array = this.enemySlots;
			for (int i = 0; i < array.Length; i++)
			{
				CharacterCard characterCard = array[i];
				if (characterCard != null)
				{
					characterCard.Hit(Engine.game.localPlayer.baseGrenadeDamage);
				}
			}
		}

		public void ReloadButton()
		{
			if (this.reloadTimer.Started)
			{
				this.reloadTimer.BumpTime(Engine.game.localPlayer.currentWeapon.baseReloadTime / 5f);
				Engine.game.localPlayer.currentWeapon.PlayReloadSound(this.reloadTimer.currentTime / this.reloadTimer.targetTime);
				this.lastReloadSoundTime = Engine.ElapsedTime;
			}
			else
			{
				this.reloadTimer.Begin();
				Engine.game.localPlayer.StartReload();
			}
		}

		public void GrenadeButton()
		{
			if (this.grenadeCount > 0 && !this.missionOver)
			{
				if (this.grenadeTimer.Enabled)
				{
					Engine.audio.PlaySound("hit0.ogg");
				}
				if (this.grenadeTimer.Started)
				{
					this.grenadeTimer.BumpTime(Engine.game.localPlayer.grenadeThrowTime / 20f);
				}
				else
				{
					this.grenadeTimer.Begin();
				}
			}
		}

		private void ArmorButton()
		{
			if (Engine.game.localPlayer.armor < Engine.game.localPlayer.baseArmor && this.armorCount > 0 && !this.missionOver)
			{
				if (this.armorTimer.Started)
				{
					this.armorTimer.BumpTime(Engine.game.localPlayer.armorUseTime / 8f);
					Engine.game.localPlayer.PlayArmorSound(this.armorTimer.currentTime / this.armorTimer.targetTime);
					this.lastArmorSoundTime = Engine.ElapsedTime;
				}
				else
				{
					this.armorTimer.Begin();
				}
			}
		}

		private void HealthkitButton()
		{
			if (Engine.game.localPlayer.health < Engine.game.localPlayer.baseHealth && this.medkitCount > 0 && !this.missionOver)
			{
				if (this.medkitTimer.Started)
				{
					this.medkitTimer.BumpTime(Engine.game.localPlayer.medkitUseTime / 8f);
				}
				else
				{
					this.medkitTimer.Begin();
				}
			}
		}

		private void localPlayer_OnPlayerKilled(Player p, int dmg, bool crit)
		{
			this.missionOver = true;
			this.missionWon = false;

            Engine.game.activeMission.DidPlayerWin = missionWon;
            Engine.game.activeMission.enemiesKilled = enemiesKilled;

			Engine.audio.PlaySound("mus/mission_start.wav");
			this.bigText.BigtextTexture = "mission_failed";
			this.bigText.shouldFade = false;
			this.bigText.Start();
			this.grenadeTimer.Reset();
			this.reloadTimer.Reset();
			this.medkitTimer.Reset();
			this.armorTimer.Reset();
			this.missionEndTime = Engine.ElapsedTime;
			this.result.Construct();
			(this.guiManager["missionResultPanel"]["returnButton"] as Button).Clicked += new Button.ClickedEvent(this.ReturnButton_Clicked);
			this.cursorTexture = Renderer.GetTexture("cursor_pointer");
			this.cursorOffset = new Vector2(0f, 0f);
		}

		private void localPlayer_OnPlayerDamage(Player p, int Dmg, bool Crit)
		{
			DamageCounter damageCounter = new DamageCounter();
			damageCounter.Position = this.guiManager["hudPanel"]["healthLabel"].screenPosition;
			damageCounter.Text = Dmg.ToString();
			if (Crit)
			{
				damageCounter.Color = System.Drawing.Color.Red;
				DamageCounter expr_53 = damageCounter;
				expr_53.Text += " CRIT";
			}
			else
			{
				damageCounter.Color = System.Drawing.Color.Orange;
			}
			damageCounter.timeToLive = Engine.ElapsedTime + 0.5f;
			this.deathBoxAlpha = this.deathBoxBaseAlpha + 0.25f;
			this.AddDamageCounter(damageCounter);
		}

		public void AddCharacterCard(CharacterCard card, int slotIndex)
		{
			card.cardPosition = this.GetSlotPosition(slotIndex);
			card.Spawn(slotIndex);
			card.OnDeath += new CharacterDeathEvent(this.card_OnDeath);
			this.enemySlots[slotIndex] = card;
			this.enemies.Remove(card);
		}

		private void card_OnDeath(CharacterCard obj, int dmg)
		{


			this.enemies.Remove(obj);
			this.enemiesKilled++;
		}

		public Vector2 GetSlotPosition(int index)
		{
			return new Vector2((float)Engine.BaseWindow.Width / ((float)this.enemySlotCount + 1f) * ((float)index + 1f) - 128f, (float)Engine.BaseWindow.Height / 2f - 170.5f);
		}

		public override void UnLoad()
		{
			this.enemies = null;
			Engine.audio.StopSounds();
			base.UnLoad();
		}

		public void AddDamageCounter(DamageCounter newCounter)
		{
			this.damageCounters.Add(newCounter);
		}

		public void CheckShot(Vector2 shotPosition, int damage)
		{
			if (this.firingBounds.Contains((int)shotPosition.X, (int)shotPosition.Y))
			{
				CharacterCard[] array = this.enemySlots;
				for (int i = 0; i < array.Length; i++)
				{
					CharacterCard characterCard = array[i];
					if (characterCard != null)
					{
						characterCard.CheckPlayerShot(damage);
					}
				}
			}
		}

		public override void Update()
		{
			if (!this.missionOver)
			{
				this.UpdateTimers();
				this.guiManager["hudPanel"]["grenadeLabel"].Text = this.grenadeCount.ToString();
				Engine.game.localPlayer.currentWeapon.IsReloading = this.reloadTimer.Started;
				if (this.reloadTimer.Started && Engine.ElapsedTime > this.lastReloadSoundTime + 0.75f)
				{
					Engine.game.localPlayer.currentWeapon.PlayReloadSound(this.reloadTimer.currentTime / this.reloadTimer.targetTime);
					this.lastReloadSoundTime = Engine.ElapsedTime;
				}
				if (this.armorTimer.Started && Engine.ElapsedTime > this.lastArmorSoundTime + 0.75f)
				{
					Engine.game.localPlayer.PlayArmorSound(this.armorTimer.currentTime / this.armorTimer.targetTime);
					this.lastArmorSoundTime = Engine.ElapsedTime;
				}
				if (this.medkitTimer.Started && Engine.ElapsedTime > this.lastMedkitSoundTime + 0.75f)
				{
					Engine.game.localPlayer.PlayMedkitSound(this.medkitTimer.currentTime / this.medkitTimer.targetTime);
					this.lastMedkitSoundTime = Engine.ElapsedTime;
				}
				if (Engine.ElapsedTime > this.nextBreakTime && Engine.ElapsedTime < this.nextBreakTime + this.breakTime)
				{
					this.BreakPeriod = true;
				}
				else if (Engine.ElapsedTime > this.nextBreakTime + this.breakTime)
				{
					this.nextBreakTime = Engine.ElapsedTime + 20f;
					this.BreakPeriod = false;
				}
				int i = 0;
				while (i < this.enemySlots.Length)
				{
					CharacterCard characterCard = this.enemySlots[i];
					if (characterCard == null && Engine.ElapsedTime > this.lastSpawn + this.enemySpawnDelay && !this.BreakPeriod)
					{
						if (this.enemies.Count > 0)
						{
							this.AddCharacterCard(this.enemies[Engine.rand.Next(this.enemies.Count)], i);
							this.lastSpawn = Engine.ElapsedTime;
						}
					}
					else if (characterCard != null)
					{
						characterCard.Update();
						if (characterCard.IsDead && Engine.ElapsedTime > characterCard.deathTime + characterCard.deathDelay)
						{
							this.enemySlots[i] = null;
						}
					}
					i++;
				}
				try
				{
					foreach (DamageCounter current in this.damageCounters)
					{
						current.Update();
						if (Engine.ElapsedTime > current.timeToLive)
						{
							this.damageCounters.Remove(current);
						}
					}
				}
				catch
				{
				}
				if (Engine.input.IsButtonDown(MouseButton.Left))
				{
					if (this.firingBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y))
					{
						Engine.game.localPlayer.currentWeapon.Fire();
					}
				}
				else if (Engine.input.WasButtonDown(MouseButton.Left) && !Engine.input.IsButtonDown(MouseButton.Left))
				{
					Engine.game.localPlayer.currentWeapon.UnFire();
				}
				if (Engine.input.IsKeyDown(Key.R) && !Engine.input.WasKeyDown(Key.R))
				{
					this.ReloadButton();
				}
				if (Engine.input.IsKeyDown(Key.G) && !Engine.input.WasKeyDown(Key.G))
				{
					this.GrenadeButton();
				}
				if (!this.missionOver && Engine.game.activeMission.enemyCount - this.enemiesKilled <= 0 && Engine.game.localPlayer.health > 0)
				{
					Engine.audio.PlaySound("mus/mission_start.wav");

					missionOver = true;
					missionWon = true;

                    Engine.game.activeMission.DidPlayerWin = missionWon;
                    Engine.game.activeMission.enemiesKilled = enemiesKilled;

					bigText.BigtextTexture = "mission_complete";
					bigText.shouldFade = false;
					bigText.Start();

					grenadeTimer.Reset();
					reloadTimer.Reset();
					medkitTimer.Reset();
					armorTimer.Reset();

					missionEndTime = Engine.ElapsedTime;

					result.Construct();
					(this.guiManager["missionResultPanel"]["returnButton"] as Button).Clicked += new Button.ClickedEvent(this.ReturnButton_Clicked);

					cursorTexture = Renderer.GetTexture("cursor_pointer");
					cursorOffset = new Vector2(0f, 0f);
				}
			}
			else
			{
				this.guiManager["hudPanel"].position = HelperFuncs.CosInterp(this.guiManager["hudPanel"].position, new Vector2(0f, (float)(Engine.BaseWindow.Height + 999)), Engine.ElapsedTime / (this.missionEndTime + 150f));
			}
			this.guiManager["hudPanel"]["healthLabel"].Text = Engine.game.localPlayer.health.ToString();
			this.guiManager["hudPanel"]["ammoLabel"].Text = Engine.game.localPlayer.currentAmmo.ToString();
			this.guiManager["hudPanel"]["armorLabel"].Text = Engine.game.localPlayer.armor.ToString();
			this.guiManager["hudPanel"]["healthkitLabel"].Text = this.medkitCount.ToString();
			this.guiManager["hudPanel"]["armorButtonLabel"].Text = this.armorCount.ToString();
			this.guiManager["topPanel"]["enemyCounter"].Text = "Enemies Left: " + (Engine.game.activeMission.enemyCount - this.enemiesKilled);
			this.deathBoxBaseAlpha = ((0f + (float)Engine.game.localPlayer.health) / (0f + (float)Engine.game.localPlayer.baseHealth) - 1f) * -1f;
			if (this.deathBoxAlpha > this.deathBoxBaseAlpha)
			{
				this.deathBoxAlpha -= 0.01f;
			}
			else if (this.deathBoxAlpha < this.deathBoxBaseAlpha)
			{
				this.deathBoxAlpha += 0.01f;
			}
			this.bigText.Update();
			base.Update();
		}

		private void UpdateTimers()
		{
			this.grenadeTimer.Update();
			this.reloadTimer.Update();
			this.medkitTimer.Update();
			this.armorTimer.Update();
			if (Engine.ElapsedTime > this.grenadeTimer.endTime + 2f)
			{
				this.grenadeTimer.Enabled = true;
			}
			if (Engine.ElapsedTime > this.reloadTimer.endTime + 1f)
			{
				this.reloadTimer.Enabled = true;
			}
			if (Engine.ElapsedTime > this.medkitTimer.endTime + 4f)
			{
				this.medkitTimer.Enabled = true;
			}
			if (Engine.ElapsedTime > this.armorTimer.endTime + 4f)
			{
				this.armorTimer.Enabled = true;
			}
		}

		public override void Render()
		{
			Engine.renderer.Prepare2D();
            Renderer.DrawTexturedRectangle(0f, 0f, Engine.gw.ClientSize.Width, Engine.gw.ClientSize.Height, "bg1");
			CharacterCard[] array = this.enemySlots;
			for (int i = 0; i < array.Length; i++)
			{
				CharacterCard characterCard = array[i];
				if (characterCard != null)
				{
					characterCard.Render();
				}
			}
			Renderer.DrawTexturedRectangle(0f, 0f, (float)Engine.BaseWindow.ClientSize.Width, (float)Engine.BaseWindow.ClientSize.Height, "deathBox", new Color4(1f, 1f, 1f, this.deathBoxAlpha), null);
			base.Render();
			if (!this.missionOver)
			{
				Renderer.DrawString(Renderer.GetFont("Test18"), Engine.game.localPlayer.currentAmmo.ToString(), new Vector2((float)Engine.input.Mouse.X + 16f, (float)Engine.input.Mouse.Y + 16f), System.Drawing.Color.White);
			}
			Renderer.DrawTexturedRectangle(this.guiManager["hudPanel"]["grenadeButton"].screenPosition.X, this.guiManager["hudPanel"]["grenadeButton"].screenPosition.Y, this.grenadeTimer.currentTime / this.grenadeTimer.targetTime * 64f, 64f, "quad");
			Renderer.DrawTexturedRectangle(this.guiManager["hudPanel"]["reloadButton"].screenPosition.X, this.guiManager["hudPanel"]["reloadButton"].screenPosition.Y, this.reloadTimer.currentTime / this.reloadTimer.targetTime * 64f, 64f, "quad");
			Renderer.DrawTexturedRectangle(this.guiManager["hudPanel"]["armorButton"].screenPosition.X, this.guiManager["hudPanel"]["armorButton"].screenPosition.Y, this.armorTimer.currentTime / this.armorTimer.targetTime * 64f, 64f, "quad");
			Renderer.DrawTexturedRectangle(this.guiManager["hudPanel"]["healthkitButton"].screenPosition.X, this.guiManager["hudPanel"]["healthkitButton"].screenPosition.Y, this.medkitTimer.currentTime / this.medkitTimer.targetTime * 64f, 64f, "quad");
			foreach (DamageCounter current in this.damageCounters)
			{
				current.Render();
			}
			this.bigText.Render();
			Engine.renderer.End2D();
		}
	}
}
