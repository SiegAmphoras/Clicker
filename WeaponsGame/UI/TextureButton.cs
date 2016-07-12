using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class TextureButton : UIElement
	{
		public delegate void ClickedEvent(MouseButtonEventArgs e);

		public string buttonTexture = "green_button11";

		private float textPad = 16f;

		private WeaponsGame.Font textFont;

		private TextAlignment alignment = TextAlignment.Center;

		private bool isDown = false;

		private Color4 textTint = System.Drawing.Color.FromArgb(64, 64, 64);

		public event TextureButton.ClickedEvent Clicked;

		public TextureButton(string name) : base(name)
		{
			this.Text = "Button";
			this.bounds = new System.Drawing.Rectangle(0, 0, 100, 50);
			this.textFont = Renderer.GetFont("Test18");
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Render()
		{
			Vector2 vector = this.textFont.MeasureString(this.Text);
			Vector2 vector2 = this.screenPosition + new Vector2(0f, (float)this.bounds.Height / 2f - vector.Y / 2f);
			if (this.alignment != TextAlignment.Left)
			{
				if (this.alignment == TextAlignment.Center)
				{
					vector2.X += (float)this.bounds.Width / 2f - this.textFont.MeasureString(this.Text).X / 2f;
				}
				else if (this.alignment == TextAlignment.Right)
				{
					vector2.X += (float)this.bounds.Width - this.textFont.MeasureString(this.Text).X;
				}
			}
			string textureName = this.buttonTexture;
			Renderer.StartClip(new System.Drawing.Rectangle((int)this.screenPosition.X, (int)this.screenPosition.Y, this.bounds.Width, this.bounds.Height));
			Renderer.DrawTexturedRectangle(this.screenPosition.X, this.screenPosition.Y, (float)this.bounds.Width, (float)this.bounds.Height, textureName);
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
			}
			base.OnClick(e);
		}
	}
}
