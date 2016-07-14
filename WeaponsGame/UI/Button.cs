using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class Button : UIElement
	{
		public delegate void ClickedEvent(MouseButtonEventArgs e);

		public string buttonTexture = "button1";

		public string pressSound = "UI/button_press_sub.ogg";

		public string hoverSound = "UI/button_hover.ogg";

		private float textPad = 16f;

        public WeaponsGame.Font font;

		public TextAlignment align = TextAlignment.Center;

		public Color4 TextUpTint;

		public Color4 TextDownTint;

		private Color4 textTint;

		private bool isDown = false;

		private bool hovering = false;

		public event Button.ClickedEvent Clicked;

		public Button(string name) : base(name)
		{
			this.Text = "Button";
			this.bounds = new System.Drawing.Rectangle(0, 0, 100, 50);
            this.font = Renderer.GetFont("Test18");
		}

		public override void Update()
		{
			if (this.isDown && !this.screenBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y))
			{
				this.isDown = false;
			}
			base.Update();
		}

		public override void Render()
		{
            Vector2 vector = this.font.MeasureString(this.Text);
			Vector2 position = this.screenPosition + new Vector2(0f, ((float)this.bounds.Height / 2f) - (vector.Y / 2f));
			if (this.align != TextAlignment.Left)
			{
				if (this.align == TextAlignment.Center)
				{
                    position.X += (float)this.bounds.Width / 2f - this.font.MeasureString(this.Text).X / 2f;
				}
				else if (this.align == TextAlignment.Right)
				{
                    position.X += (float)this.bounds.Width - this.font.MeasureString(this.Text).X;
				}
			}
			string textureName = this.buttonTexture;
			if (this.isDown)
			{
				textureName = this.buttonTexture + "_pressed";
				this.textTint = this.TextDownTint;
			}
			else
			{
				if (this.hovering)
				{
					textureName = this.buttonTexture + "_hover";
				}
				else
				{
					textureName = this.buttonTexture;
				}
				this.textTint = this.TextUpTint;
			}
			Renderer.StartClip(new System.Drawing.Rectangle((int)this.screenPosition.X, (int)this.screenPosition.Y, this.bounds.Width, this.bounds.Height));
			Renderer.DrawPanel(this.screenPosition.X, this.screenPosition.Y, (float)this.bounds.Width, (float)this.bounds.Height, textureName, this.tint);
            Renderer.DrawString(this.font, this.Text, position, this.textTint);
			Renderer.EndClip();
			base.Render();
		}

		public override void OnClick(MouseButtonEventArgs e)
		{
			if (!this.isDown && e.Button == MouseButton.Left && e.IsPressed)
			{
				this.isDown = true;
			}
			else if (this.isDown && e.Button == MouseButton.Left && !e.IsPressed)
			{
				this.isDown = false;
				if (this.Clicked != null)
				{
					this.Clicked(e);
				}
				Engine.audio.PlaySound(this.pressSound);
			}
			base.OnClick(e);
		}

		public override void OnMouseEnter(MouseButtonEventArgs e)
		{
			Engine.audio.PlaySound(this.hoverSound);
			this.hovering = true;
			base.OnMouseEnter(e);
		}

		public override void OnMouseExit(MouseButtonEventArgs e)
		{
			this.hovering = false;
			base.OnMouseExit(e);
		}

		public override void Destroy()
		{
			this.Clicked = null;
			base.Destroy();
		}
	}
}
