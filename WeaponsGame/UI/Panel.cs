using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WeaponsGame.UI
{
	internal class Panel : UIElement
	{
		public string panelTexture = "metalPanel_yellow";

		private bool focused = false;

		public bool DrawBackground = true;

		public bool ShowHotbarControl = true;

		public bool Moveable = true;

		public bool Focusable = true;

		public bool Closable = true;

		private System.Collections.Generic.List<UIElement> controls;

		public WeaponsGame.Font titleFont;

		private float hotbarHeight = 33f;

		private float displayDelay = 0.25f;

		private float openTime = 0f;

		private float destroyTime = 0f;

		public float Alpha = 1f;

		public bool Opening
		{
			get;
			protected set;
		}

		public bool Closing
		{
			get;
			protected set;
		}

		public bool Destroyed
		{
			get;
			protected set;
		}

		public float HotbarHeight
		{
			get
			{
				return this.hotbarHeight;
			}
		}

		public System.Drawing.Rectangle hotbarBounds
		{
			get
			{
				return new System.Drawing.Rectangle((int)this.position.X, (int)this.position.Y, this.bounds.Width, (int)this.hotbarHeight);
			}
		}

		public System.Drawing.Rectangle elementBounds
		{
			get
			{
				return new System.Drawing.Rectangle((int)(this.position.X + this.edgePadding.X), (int)(this.position.Y + this.hotbarHeight), (int)((float)this.bounds.Width - this.edgePadding.Y * 2f), (int)((float)this.bounds.Height - this.hotbarHeight - this.edgePadding.Y));
			}
		}

		public System.Drawing.Rectangle closeBounds
		{
			get
			{
				return new System.Drawing.Rectangle((int)(this.position.X + (float)this.bounds.Width - this.edgePadding.X - 16f), (int)(this.position.Y + this.edgePadding.X), 16, 16);
			}
		}

		public UIElement this[string name]
		{
			get
			{
				return this.controls.Find((UIElement i) => i.Name == name);
			}
		}

		public Panel(string name) : base(name)
		{
			this.edgePadding.Y = this.edgePadding.Y + this.hotbarHeight;
			this.Text = "Panel";
			this.position = new Vector2(0f, 0f);
			this.bounds = new System.Drawing.Rectangle(0, 0, 300, 200);
			this.controls = new System.Collections.Generic.List<UIElement>();
			this.titleFont = Renderer.GetFont("Test18");
			this.Opening = true;
			this.openTime = Engine.ElapsedTime;
			this.tint.A = 0f;
			this.InitChildren();
		}

		public virtual void InitChildren()
		{
		}

		public void AddControl(UIElement element)
		{
			element.SetParent(this);
			this.controls.Add(element);
		}

		public void Close()
		{
			if (this.Closable)
			{
				this.Closing = true;
				this.destroyTime = Engine.ElapsedTime;
			}
		}

		public void SetTexture(string texture)
		{
			this.panelTexture = texture;
		}

		public void SetFocused(bool focused)
		{
			this.focused = focused;
		}

		public override void Update()
		{
			if (this.Closing && Engine.ElapsedTime > this.destroyTime + this.displayDelay)
			{
				this.Destroyed = true;
				foreach (UIElement current in this.controls)
				{
					current.Destroy();
				}
			}
			if (this.Closing)
			{
				this.tint.A = this.tint.A - Engine.DeltaTime / this.displayDelay;
			}
			else if (this.Opening)
			{
				this.tint.A = this.tint.A + this.Alpha * (Engine.DeltaTime / this.displayDelay);
				if (Engine.ElapsedTime > this.openTime + this.displayDelay)
				{
					this.Opening = false;
				}
			}
			else
			{
				this.tint.A = this.Alpha;
			}
			foreach (UIElement current in this.controls)
			{
				current.Update();
				current.tint.A = this.tint.A;
			}
			base.Update();
		}

		public override void Render()
		{
			Renderer.StartClip(this.bounds);
			if (this.DrawBackground)
			{
				Renderer.DrawPanel(this.position.X, this.position.Y, (float)this.bounds.Width, (float)this.bounds.Height, this.panelTexture, this.tint);
				Renderer.DrawString(this.titleFont, this.Text, this.position + new Vector2(16f + this.edgePadding.X, this.hotbarHeight / 2f - this.titleFont.MeasureString(this.Text).Y / 2f), 2f, this.tint);
				if (this.ShowHotbarControl)
				{
					Renderer.DrawTexturedRectangle(this.position.X + (float)this.bounds.Width - this.edgePadding.X - 16f, this.position.Y + this.edgePadding.X, 16f, 16f, "panel1_closebutton", this.tint, null);
				}
			}
			foreach (UIElement current in this.controls)
			{
				current.Render();
			}
			base.Render();
			Renderer.EndClip();
		}

		public override void OnClick(MouseButtonEventArgs e)
		{
			UIElement uIElement = this.controls.Find((UIElement i) => i.screenBounds.Contains(e.X, e.Y));
			if (uIElement != null)
			{
				uIElement.OnClick(e);
				base.OnClick(e);
			}
		}
	}
}
