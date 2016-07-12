using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WeaponsGame.Game
{
	public class CharacterCard
	{
		public string Name = "UndefinedCharacter";

		public EnvironmentType environment = EnvironmentType.Forest;

		public float createTime = 0f;

		public float attackRate = 3f;

		public float lastAttackTime = 0f;

		public float nextAttackTime = 0f;

		public int shotsPerAttack = 2;

		public int shotsFired = 0;

		public bool IsAttacking = false;

		public string characterImageSet = "en_rus1_ak";

		public string fireSound = "weapon/fire6.wav";

		private string activeTextureName = "en_rus1_ak_fire";

		public Color4 tint = Color4.White;

		public bool shouldShowHit = false;

		public float lastHurtTime = 0f;

		public float hurtTime = 0.2f;

		public int baseDamage = 3;

		public float pctCritical = 0.1f;

		public float accuracy = 0.75f;

		public int BaseHealth = 200;

		public int Health;

		public bool IsSpawning = false;

		private float spawnInterp = 0f;

		public Vector2 cardPosition;

		public Vector2 cardPositionOffset;

		public bool IsDead = false;

		public bool IsFinishedDeath = false;

		public float deathAnimAccel = 0.5f;

		public Vector2 deathVelocity;

		public float deathTime = 0f;

		public float deathDelay = 1f;

		public GLSprite gunshotSprite;

		private Vector2 gunshotOffset = new Vector2(-20f, -28f);

		private Vector2[] gunshotFrames = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};

		private int gunshotSpriteFrame = 0;

		private float gunshotLastFrame = 0f;

		private float gunshotFrameRate = 0.03f;

		public float gunshotDelay = 0.3f;

		private System.Collections.Generic.List<DamageCounter> damageCounters;

		private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Drawing.Rectangle, float>> hitBoxes = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.Drawing.Rectangle, float>>
		{
			new System.Collections.Generic.KeyValuePair<System.Drawing.Rectangle, float>(new System.Drawing.Rectangle(98, 44, 50, 50), 1.75f),
			new System.Collections.Generic.KeyValuePair<System.Drawing.Rectangle, float>(new System.Drawing.Rectangle(75, 40, 115, 298), 0.8f)
		};

		public int slotIndex = 0;

		public event CharacterDeathEvent OnKilled;

		public event CharacterDeathEvent OnDeath;

		public CharacterCard()
		{
			this.damageCounters = new System.Collections.Generic.List<DamageCounter>();
			this.gunshotSprite = new GLSprite(256, 256, Renderer.GetTexture("shot"));
		}

		public virtual void Spawn(int slotIndex)
		{
			this.createTime = Engine.ElapsedTime;
			this.IsSpawning = true;
			this.Health = this.BaseHealth;
			this.cardPositionOffset = new Vector2(0f, 600f);
			this.slotIndex = slotIndex;
		}

		public virtual void Fire()
		{
			Engine.audio.PlaySound(this.fireSound);
			this.IsAttacking = true;
			int num = this.baseDamage;
			bool crit = false;
			if ((float)Engine.rand.Next(100) < this.pctCritical * 100f)
			{
				num = (int)(0f + (float)num * 1.5f);
				crit = true;
			}
			if ((float)Engine.rand.Next(100) < this.accuracy * 100f)
			{
				Engine.game.localPlayer.TakeDamage(num, crit);
			}
			else
			{
				this.damageCounters.Add(new DamageCounter
				{
					Text = "MISS",
					Position = this.cardPosition + new Vector2(128f, 128f),
					timeToLive = Engine.ElapsedTime + 1f,
					Color = System.Drawing.Color.LightGray
				});
			}
		}

		public virtual void CheckPlayerShot(int damage)
		{
			foreach (System.Collections.Generic.KeyValuePair<System.Drawing.Rectangle, float> current in this.hitBoxes)
			{
				System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle((int)this.cardPosition.X + current.Key.X, (int)this.cardPosition.Y + current.Key.Y, current.Key.Width, current.Key.Height);
				if (rectangle.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y))
				{
					this.Hit((int)((float)damage * current.Value));
					break;
				}
			}
		}

		public virtual void Hit(int damage)
		{
			if (!this.IsDead && !this.IsSpawning)
			{
				this.Health -= damage;
				this.damageCounters.Add(new DamageCounter
				{
					timeToLive = Engine.ElapsedTime + 1f,
					Text = damage.ToString(),
					Color = System.Drawing.Color.Red,
					Position = this.cardPosition + new Vector2(128f, 128f)
				});
				this.lastHurtTime = Engine.ElapsedTime;
				if (this.Health <= 0)
				{
					this.Health = 0;
					if (this.OnKilled != null)
					{
						this.OnKilled(this, damage);
					}
					this.IsDead = true;
					this.deathTime = Engine.ElapsedTime;
				}
			}
		}

		public virtual void Update()
		{
			if (!this.shouldShowHit)
			{
				this.activeTextureName = this.characterImageSet + "_fire";
			}
			else
			{
				this.activeTextureName = this.characterImageSet + "_death";
			}
			if (this.IsSpawning)
			{
				this.spawnInterp += Engine.DeltaTime;
				if (this.spawnInterp > 1f)
				{
					this.spawnInterp = 1f;
					this.IsSpawning = false;
				}
				this.cardPositionOffset = HelperFuncs.CosInterp(this.cardPositionOffset, new Vector2(0f, 0f), this.spawnInterp / 1f);
			}
			else
			{
				if (!this.IsDead)
				{
					if (!this.shouldShowHit)
					{
						if (Engine.ElapsedTime > this.nextAttackTime && this.shotsFired < this.shotsPerAttack)
						{
							this.Fire();
							this.nextAttackTime = Engine.ElapsedTime + this.attackRate / (float)(this.shotsPerAttack * 2);
							this.lastAttackTime = Engine.ElapsedTime;
							this.shotsFired++;
						}
						if (this.shotsFired >= this.shotsPerAttack)
						{
							this.shotsFired = 0;
							this.nextAttackTime = Engine.ElapsedTime + this.attackRate;
						}
					}
					if (this.IsAttacking && Engine.ElapsedTime > this.lastAttackTime + this.gunshotDelay)
					{
						this.IsAttacking = false;
						this.gunshotSpriteFrame = 0;
					}
					else if (this.IsAttacking && Engine.ElapsedTime > this.gunshotLastFrame + this.gunshotFrameRate)
					{
						this.gunshotSpriteFrame++;
						if (this.gunshotSpriteFrame > this.gunshotFrames.Length - 1)
						{
							this.gunshotSpriteFrame = this.gunshotFrames.Length - 1;
						}
						this.gunshotLastFrame = Engine.ElapsedTime;
					}
					if (this.shouldShowHit && Engine.ElapsedTime > this.lastHurtTime + this.hurtTime)
					{
						this.shouldShowHit = false;
					}
				}
				else
				{
					this.shouldShowHit = true;
					if (Engine.ElapsedTime > this.deathTime + this.deathDelay)
					{
						this.IsFinishedDeath = true;
						if (this.OnDeath != null)
						{
							this.OnDeath(this, 0);
						}
					}
					this.deathVelocity.Y = this.deathVelocity.Y + this.deathAnimAccel;
					this.deathVelocity.X = this.deathVelocity.X + this.deathAnimAccel / 4f;
					this.cardPositionOffset.Y = this.cardPositionOffset.Y + this.deathVelocity.Y;
					this.cardPositionOffset.X = this.cardPositionOffset.X + this.deathVelocity.X;
					if (Engine.ElapsedTime > this.deathTime + this.deathDelay)
					{
						this.tint.A = this.tint.A - 0.1f;
					}
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
			}
		}

		public virtual void Render()
		{
			Renderer.DrawTexturedRectangle(this.cardPosition.X + this.cardPositionOffset.X + 64f, this.cardPosition.Y + this.cardPositionOffset.Y + 16f, (0f + (float)this.Health) / (0f + (float)this.BaseHealth) * 128f, 10f, "quad", Color4.Red, null);
			Renderer.DrawString(Renderer.GetFont("Test18"), this.Health + " / " + this.BaseHealth, new Vector2(this.cardPosition.X + this.cardPositionOffset.X + 64f, this.cardPosition.Y + this.cardPositionOffset.Y + 32f), Color4.Red);
			Renderer.DrawTexturedRectangle(this.cardPosition.X + this.cardPositionOffset.X, this.cardPosition.Y + this.cardPositionOffset.Y, 256f, 341f, this.activeTextureName, this.tint, null);
			if (this.IsAttacking)
			{
				TextureUVW uVForFrame = this.gunshotSprite.GetUVForFrame((int)this.gunshotFrames[this.gunshotSpriteFrame].X, (int)this.gunshotFrames[this.gunshotSpriteFrame].Y);
				Renderer.DrawTexturedRectangle(this.cardPosition.X + this.gunshotOffset.X, this.cardPosition.Y + this.gunshotOffset.Y, 256f, 256f, "shot", Color4.White, uVForFrame);
			}
			foreach (DamageCounter current in this.damageCounters)
			{
				current.Render();
			}
		}

		public static CharacterCard LoadFromArchetype(CharacterArchetypeStruct arch)
		{
			return arch.ToCharacterCard();
		}
	}
}
