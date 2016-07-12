using OpenTK;
using OpenTK.Graphics;
using System;

namespace WeaponsGame.UI
{
	internal class MissionBigtext
	{
		public string BigtextTexture = "mission_start";

		private float scale = 0f;

		private float startTime = 0f;

		private float endTime = 0f;

		private float alpha = 1f;

		private float scaleDelay = 2f;

		private float fadeDelay = 2f;

		public bool started = false;

		public bool shouldFade = true;

		private Texture texture;

		private Vector2 position;

		public MissionBigtext()
		{
			this.position = default(Vector2);
		}

		public void Start()
		{
			this.startTime = Engine.ElapsedTime;
			this.endTime = this.startTime + this.scaleDelay + this.fadeDelay;
			this.started = true;
			this.texture = Renderer.GetTexture(this.BigtextTexture);
			this.position = new Vector2((float)Engine.BaseWindow.ClientSize.Width / 2f, (float)Engine.BaseWindow.ClientSize.Height / 6f);
			this.alpha = 1f;
			this.scale = 20f;
		}

		public void Update()
		{
			if (this.started)
			{
				if (this.scale > 1f)
				{
					this.scale = HelperFuncs.CosineInterpolate(this.scale, 1f, (Engine.ElapsedTime - this.startTime) / this.scaleDelay);
				}
				else if (this.shouldFade)
				{
					this.alpha = HelperFuncs.CosineInterpolate(1f, 0f, Engine.ElapsedTime / this.endTime);
				}
				if (Engine.ElapsedTime > this.endTime)
				{
					this.started = false;
				}
			}
		}

		public void Render()
		{
			Renderer.DrawTexturedRectangleCentered(this.position.X, this.position.Y, this.texture.Width * this.scale, this.texture.Height * this.scale, this.texture.ID, new Color4(1f, 1f, 1f, this.alpha), null);
		}
	}
}
