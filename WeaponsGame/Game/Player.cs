using System;

namespace WeaponsGame.Game
{
	public class Player
	{
		public int grenadeCount = 2;

		public float grenadeThrowTime = 8f;

		public int medkitCount = 3;

		public float medkitUseTime = 12f;

		public float medkitRestorePercentile = 0.3f;

		public int armorCount = 2;

		public float armorUseTime = 10f;

		public float armorRestorePercentile = 0.4f;

		public BaseWeapon currentWeapon;

		public int baseHealth = 100;

		public int health = 100;

		public int baseArmor = 100;

		public int armor = 100;

		public int currentAmmo;

		public int baseGrenadeDamage = 200;

		public event PlayerDamageEvent OnPlayerDamage;

		public event PlayerDamageEvent OnPlayerKilled;

		public Player()
		{
			this.currentWeapon = new BaseWeapon();
		}

		public void StartMission()
		{
			this.health = this.baseHealth;
			this.currentAmmo = this.currentWeapon.baseMagCount;
		}

		public void StartReload()
		{
			this.currentWeapon.StartReload();
		}

		public void EndReload()
		{
			this.currentAmmo = this.currentWeapon.baseMagCount;
			this.currentWeapon.FinishReload();
		}

		public void TakeDamage(int damage, bool crit)
		{
			if (this.armor > 0)
			{
				int num = (int)((float)damage / 3f * 2f);
				if (this.armor < num)
				{
					num -= this.armor;
					this.armor = 0;
					this.health -= (int)((float)num + (float)damage / 3f);
				}
				else
				{
					this.armor -= num;
					this.health -= (int)((float)damage / 3f);
				}
			}
			else
			{
				this.health -= damage;
			}
			if (this.health <= 0)
			{
				this.health = 0;
				if (this.OnPlayerKilled != null)
				{
					this.OnPlayerKilled(this, damage, crit);
				}
			}
			else if (this.OnPlayerDamage != null)
			{
				this.OnPlayerDamage(this, damage, crit);
			}
		}

		public void UseMedkit()
		{
			this.health += (int)((float)this.baseHealth * this.medkitRestorePercentile);
		}

		public void UseArmor()
		{
			this.armor += (int)((float)this.baseArmor * this.armorRestorePercentile);
		}

		public virtual void PlayArmorSound(float pctdone)
		{
			if (pctdone < 0.25f)
			{
				Engine.audio.PlaySound("Misc/cloth" + (Engine.rand.Next(12) + 1) + ".wav");
			}
			else if (pctdone > 0.25f && pctdone < 0.5f)
			{
				Engine.audio.PlaySound("Misc/cloth" + (Engine.rand.Next(12) + 1) + ".wav");
			}
			else if (pctdone > 0.5f && pctdone < 0.75f)
			{
				Engine.audio.PlaySound("Misc/cloth" + (Engine.rand.Next(12) + 1) + ".wav");
			}
			else if (pctdone > 0.75f && pctdone < 1f)
			{
				Engine.audio.PlaySound("Misc/zip" + (Engine.rand.Next(6) + 1) + ".wav");
			}
		}

		public virtual void PlayMedkitSound(float pctdone)
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
