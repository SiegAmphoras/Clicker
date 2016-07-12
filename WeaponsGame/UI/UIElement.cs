using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class UIElement
	{
		public string Name;

		public string Text;

		public Vector2 position;

		public Vector2 screenPosition;

		public Color4 tint = System.Drawing.Color.White;

		public System.Drawing.Rectangle bounds;

		public System.Drawing.Rectangle screenBounds;

		internal Vector2 edgePadding = new Vector2(8f, 8f);

		private bool mouseEntered = false;

		public UIElement parent
		{
			get;
			protected set;
		}

		public Vector2 ControlEdgePadding
		{
			get
			{
				return this.edgePadding;
			}
		}

		public UIElement(string name = "element")
		{
			this.Name = name;
			this.Text = "";
		}

		public virtual void Render()
		{
		}

		public virtual void Update()
		{
			this.bounds.X = (int)this.position.X;
			this.bounds.Y = (int)this.position.Y;
			if (this.parent != null)
			{
				this.screenPosition = new Vector2(this.parent.position.X + this.position.X + this.parent.ControlEdgePadding.X, this.parent.position.Y + this.position.Y + this.parent.ControlEdgePadding.Y);
			}
			else
			{
				this.screenPosition = new Vector2(this.position.X, this.position.Y);
			}
			this.screenBounds = new System.Drawing.Rectangle((int)this.screenPosition.X, (int)this.screenPosition.Y, this.bounds.Width, this.bounds.Height);
			if (this.screenBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y))
			{
				if (!this.mouseEntered)
				{
					this.OnMouseEnter(new MouseButtonEventArgs(Engine.input.Mouse.X, Engine.input.Mouse.Y, MouseButton.Left, false));
					this.mouseEntered = true;
				}
			}
			else if (this.mouseEntered)
			{
				this.OnMouseExit(new MouseButtonEventArgs(Engine.input.Mouse.X, Engine.input.Mouse.Y, MouseButton.Left, false));
				this.mouseEntered = false;
			}
		}

		public virtual void OnMouseExit(MouseButtonEventArgs e)
		{
		}

		public virtual void OnMouseEnter(MouseButtonEventArgs e)
		{
		}

		public virtual void OnClick(MouseButtonEventArgs e)
		{
		}

		public void SetParent(UIElement newParent)
		{
			this.parent = newParent;
		}

		public virtual void Destroy()
		{
		}
	}
}
