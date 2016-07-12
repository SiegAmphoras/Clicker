using OpenTK;
using System;
using WeaponsGame.Scenes;

namespace WeaponsGame.Game
{
	public class BaseWeapon
	{
		public float fireRate = 0.16f;

		public float lastFireTime = 0f;

		public bool SemiFire = false;

		public float accuracy = 0.5f;

		public int baseMagCount = 13;

		public float baseReloadTime = 5f;

		public int baseDamage = 10;

		public float baseCritChance = 0.12f;

		public float baseCritMult = 1.3f;

		public bool IsReloading;

		public virtual void Fire()
		{
			if (!this.IsReloading)
			{
				if (Engine.ElapsedTime > this.lastFireTime + this.fireRate && Engine.game.localPlayer.currentAmmo > 0)
				{
					this.lastFireTime = Engine.ElapsedTime;
					Vector2 shotPosition = new Vector2((float)Engine.input.Mouse.X, (float)Engine.input.Mouse.Y);
					float num = (float)Engine.rand.NextDouble() * 2f - 1f;
					num *= this.accuracy;
					float num2 = (float)Engine.rand.NextDouble() * 2f - 1f;
					num2 *= this.accuracy;
					shotPosition.X += num;
					shotPosition.Y += num2;
					int num3 = this.baseDamage;
					if ((float)Engine.rand.Next(100) < this.baseCritChance * 100f)
					{
						num3 = (int)((float)num3 * this.baseCritMult);
					}
					(Engine.ActiveScene as MissionScene).CheckShot(shotPosition, num3);
					Engine.audio.PlaySound("weapon/fire4.wav");
					Engine.game.localPlayer.currentAmmo--;
				}
			}
		}

		public virtual void UnFire()
		{
			this.lastFireTime = 0f;
		}

		public virtual void StartReload()
		{
			this.IsReloading = true;
		}

		public virtual void FinishReload()
		{
			this.IsReloading = false;
		}

		public virtual void PlayReloadSound(float pctdone)
		{
			if (pctdone < 0.25f)
			{
				Engine.audio.PlaySound("weapon/reload/foley" + (Engine.rand.Next(6) + 1) + ".wav");
			}
			else if (pctdone > 0.25f && pctdone < 0.5f)
			{
				Engine.audio.PlaySound("weapon/reload/mag_in" + (Engine.rand.Next(1) + 1) + ".wav");
			}
			else if (pctdone > 0.5f && pctdone < 0.75f)
			{
				Engine.audio.PlaySound("weapon/reload/slide_pull" + (Engine.rand.Next(1) + 1) + ".wav");
			}
			else if (pctdone > 0.75f && pctdone < 1f)
			{
				Engine.audio.PlaySound("weapon/reload/slide_release" + (Engine.rand.Next(5) + 1) + ".wav");
			}
		}
	}
}
