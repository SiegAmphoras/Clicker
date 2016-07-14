using OpenTK;
using System;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class TextLabel : UIElement
	{
		public TextAlignment align = TextAlignment.Left;

        public bool WrapText = false; //TODO: implement proper line wrapping

		public WeaponsGame.Font font;

        public TextLabel(string name)
            : base(name)
        {
            this.tint = System.Drawing.Color.FromArgb(64, 64, 64);
            this.Text = "";
        }

		public override void Render()
		{
			Rectangle elementBounds = (base.parent as Panel).elementBounds;

            Vector2 position = new Vector2((float)elementBounds.X, (float)elementBounds.Y) + this.position;

            if (this.align == TextAlignment.Right)
            {
                position.X -= font.MeasureString(Text).X;
            }
            else if (this.align == TextAlignment.Center)
            {
                position.X -= font.MeasureString(Text).X / 2f;
            }

            Renderer.DrawString(this.font, Text, position, this.tint);

			base.Render();
		}
	}
}
