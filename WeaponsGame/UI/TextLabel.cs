using OpenTK;
using System;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class TextLabel : UIElement
	{
		public TextAlignment align = TextAlignment.Left;

		private WeaponsGame.Font font;

		public TextLabel(string name) : base(name)
		{
			this.tint = System.Drawing.Color.FromArgb(64, 64, 64);
			this.Text = "";
		}

		public void SetFont(WeaponsGame.Font f)
		{
			this.font = f;
		}

		public override void Render()
		{
			System.Drawing.Rectangle elementBounds = (base.parent as Panel).elementBounds;
			Vector2 position = new Vector2((float)elementBounds.X, (float)elementBounds.Y) + this.position;
			if (this.align == TextAlignment.Right)
			{
				position.X -= this.font.MeasureString(this.Text).X;
			}
			else if (this.align == TextAlignment.Center)
			{
				position.X -= this.font.MeasureString(this.Text).X / 2f;
			}
			Renderer.DrawString(this.font, this.Text, position, this.tint);
			base.Render();
		}
	}
}
