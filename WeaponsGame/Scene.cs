using OpenTK;
using OpenTK.Graphics;
using System;

namespace WeaponsGame
{
	internal class Scene
	{
		public delegate void TransitionFinished(object sender, System.EventArgs e);

		public GUIManager guiManager;

		public Texture cursorTexture;

		public Vector2 cursorOffset;

		private bool inputEnable = true;

		private bool transition = false;

		private float transitionTarget = 0f;

		private float transitionAlpha = 1f;

		private float transitionTime = 0f;

		public event Scene.TransitionFinished OnTransitionFinished;

		public virtual void Load()
		{
			this.guiManager = new GUIManager();
			this.cursorOffset = new Vector2(-16f, -16f);
			this.cursorTexture = Renderer.GetTexture("cursor_crosshair");
			this.TransitionIn();
		}

		public virtual void UnLoad()
		{
			this.guiManager = null;
			this.OnTransitionFinished = null;
		}

		public virtual void Update()
		{
			if (this.inputEnable)
			{
				this.guiManager.Update();
			}
			if (this.transition)
			{
				this.transitionTime += Engine.DeltaTime;
				this.transitionAlpha = HelperFuncs.CosineInterpolate(this.transitionAlpha, this.transitionTarget, this.transitionTime / 1f);
				if (this.transitionAlpha == this.transitionTarget)
				{
					if (this.OnTransitionFinished != null)
					{
						this.OnTransitionFinished(this, null);
					}
					this.transition = false;
				}
			}
		}

		public virtual void Render()
		{
			this.guiManager.Render();
			Renderer.DrawTexturedRectangle((float)Engine.input.Mouse.X + this.cursorOffset.X, (float)Engine.input.Mouse.Y + this.cursorOffset.Y, this.cursorTexture.Width, this.cursorTexture.Height, this.cursorTexture.ID, Color4.White, null);
		}

		public virtual void PostRender()
		{
			Engine.renderer.Prepare2D();
			Renderer.DrawTexturedRectangle(0f, 0f, (float)Engine.BaseWindow.Width, (float)Engine.BaseWindow.Height, "quad", new Color4(0f, 0f, 0f, this.transitionAlpha), null);
			Engine.renderer.End2D();
		}

		public void DisableInput()
		{
			this.inputEnable = false;
		}

		public void EnableInput()
		{
			this.inputEnable = true;
		}

		public void TransitionOut()
		{
			this.DisableInput();
			this.transitionTime = 0f;
			this.transitionTarget = 1f;
			this.transition = true;
		}

		public void TransitionIn()
		{
			this.EnableInput();
			this.transitionTime = 0f;
			this.transitionTarget = 0f;
			this.transition = true;
		}
	}
}
