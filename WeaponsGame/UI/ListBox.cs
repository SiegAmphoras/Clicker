using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class ListBox : UIElement
	{
		public delegate void SelectedIndexChanged(MouseButtonEventArgs e);

		public System.Collections.Generic.List<string> Items;

		public int SelectedIndex = 0;

		public string tex = "panel2";

		private string scrollbarTex = "panel3";

		private string itemTex = "panel3";

		public float scrollOffset = 0f;

		public WeaponsGame.Font textFont;

		private Vector2 textPos;

		public event ListBox.SelectedIndexChanged OnSelectedIndexChanged;

		public ListBox(string name) : base(name)
		{
			this.Items = new System.Collections.Generic.List<string>();
			this.textFont = Renderer.GetFont("Test18");
			this.textPos = new Vector2(this.edgePadding.X, 10f);
		}

		public override void OnClick(MouseButtonEventArgs e)
		{
			if (e.IsPressed)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(this.screenBounds.X, this.screenBounds.Y + 32 + (i * 24 + i * 2), this.bounds.Width - 32, 24);
					if (rectangle.Contains(e.X, e.Y))
					{
						this.SelectedIndex = i;
						if (this.OnSelectedIndexChanged != null)
						{
							this.OnSelectedIndexChanged(e);
						}
						break;
					}
				}
			}
			base.OnClick(e);
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Render()
		{
			Renderer.DrawPanel(this.screenPosition.X, this.screenPosition.Y, (float)this.bounds.Width, 32f, this.tex, this.tint);
			Renderer.DrawPanel(this.screenPosition.X, this.screenPosition.Y + 30f, (float)this.bounds.Width, (float)(this.bounds.Height - 30), this.tex, this.tint);
			Renderer.DrawString(this.textFont, this.Text, this.screenPosition + this.textPos, this.tint);
			Renderer.StartClip(new System.Drawing.Rectangle((int)this.screenPosition.X, (int)this.screenPosition.Y + 32, this.bounds.Width - 32, this.bounds.Height - 24));
			for (int i = 0; i < this.Items.Count; i++)
			{
				string line = this.Items[i];
				Vector2 right = new Vector2(8f, (float)(32 + 24 * i + 2 * i + 6) + this.scrollOffset * 24f);
				if (this.SelectedIndex == i)
				{
					Renderer.DrawPanel(this.screenPosition.X, this.screenPosition.Y + (float)(32 + 24 * i + 2 * i) + this.scrollOffset * 24f, (float)(this.bounds.Width - 32), 24f, this.itemTex, this.tint);
					Renderer.DrawString(this.textFont, line, this.screenPosition + right, Color4.Black);
				}
				else
				{
					Renderer.DrawString(this.textFont, line, this.screenPosition + right, this.tint);
				}
			}
			Renderer.EndClip();
			base.Render();
		}
	}
}
