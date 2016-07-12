using OpenTK;
using OpenTK.Graphics;
using System;

namespace WeaponsGame.Game
{
	internal class DamageCounter
	{
		public string Text;

		public Color4 Color;

		public float alpha = 1f;

		public float timeToLive;

		public Vector2 Position;

		private Vector2 velocity;

		private float accel = 8f;

		private float gravity = 0.5f;

		private Font font;

		public DamageCounter()
		{
			this.Text = "DmgCounter!";
			this.Color = Color4.Purple;
			this.font = Renderer.GetFont("Test32");
			this.velocity.Y = this.velocity.Y - this.accel;
			this.velocity.X = this.accel / 4f * ((float)Engine.rand.NextDouble() * 2f - 1f);
		}

		public void Update()
		{
			this.velocity.Y = this.velocity.Y + this.gravity;
			this.Position.Y = this.Position.Y + this.velocity.Y;
			this.Position.X = this.Position.X + this.velocity.X;
			this.alpha -= Engine.DeltaTime;
		}

		public void Render()
		{
			Vector2 position = this.Position;
			position.X -= this.font.MeasureString(this.Text).X / 2f;
			Renderer.DrawString(this.font, this.Text, position, 1f, this.Color, this.alpha);
		}
	}
}
