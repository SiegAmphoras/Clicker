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

            string[] lines = Text.Split('\n');

            Vector2 position = new Vector2((float)elementBounds.X, (float)elementBounds.Y) + this.position;

            float lineY = 0;

            foreach (string s in lines)
            {
                position = new Vector2((float)elementBounds.X, (float)elementBounds.Y) + this.position;
                position.Y += lineY;

                Vector2 linesize = font.MeasureString(s);

                if (this.align == TextAlignment.Right)
                {
                    position.X -= linesize.X;
                }
                else if (this.align == TextAlignment.Center)
                {
                    position.X -= linesize.X / 2f;
                }

                Renderer.DrawString(this.font, s, position, this.tint);

                lineY += font.Size;
            }

			base.Render();
		}
	}
}
