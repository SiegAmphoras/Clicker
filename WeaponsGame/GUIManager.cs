using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using WeaponsGame.UI;

namespace WeaponsGame
{
	internal class GUIManager
	{
		private System.Collections.Generic.List<Panel> panels;

		private Panel focusedPanel;

		private Vector2 mouseDragPos;

		private bool draggingPanel = false;

		public Panel this[string index]
		{
			get
			{
				return this.panels.Find((Panel i) => i.Name == index);
			}
		}

		public int GetPanelCount()
		{
			return this.panels.Count;
		}

		public GUIManager()
		{
			this.panels = new System.Collections.Generic.List<Panel>();
		}

		public void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (this.panels.Exists((Panel i) => i.bounds.Contains(e.X, e.Y)))
				{
					Panel panel = this.panels.First((Panel i) => i.bounds.Contains(e.X, e.Y));
					if (panel != null)
					{
						panel.OnClick(e);
					}
				}
			}
		}

		public void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (this.panels.Exists((Panel i) => i.bounds.Contains(e.X, e.Y)))
				{
					Panel panel = this.panels.First((Panel i) => i.bounds.Contains(e.X, e.Y));
					if (panel != null)
					{
						panel.OnClick(e);
					}
				}
			}
		}

		public void RegisterPanel(Panel panel)
		{
			this.panels.Add(panel);
		}

		public void RemovePanel(Panel panel)
		{
			this.panels.Remove(panel);
		}

		public void FocusPanel(Panel p)
		{
			if (this.focusedPanel != null)
			{
				this.UnfocusPanel();
			}
			if (p.Focusable)
			{
				this.panels.Remove(p);
				this.panels.Insert(0, p);
				p.SetFocused(true);
				this.focusedPanel = p;
			}
		}

		public void UnfocusPanel()
		{
			if (this.focusedPanel != null)
			{
				this.focusedPanel.SetFocused(false);
				this.focusedPanel = null;
			}
		}

		public void Update()
		{
			System.Collections.Generic.Stack<Panel> stack = new System.Collections.Generic.Stack<Panel>();
			foreach (Panel panel in this.panels)
			{
				panel.Update();
				if (panel.Destroyed)
				{
					stack.Push(panel);
				}
			}
			for (int j = 0; j < stack.Count; j++)
			{
				this.panels.Remove(stack.Pop());
			}
			stack = null;
			if (Engine.input.IsButtonDown(MouseButton.Left))
			{
				if (!Engine.input.WasButtonDown(MouseButton.Left))
				{
					if (this.panels.Exists((Panel i) => i.hotbarBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y)))
					{
						Panel panel = this.panels.Find((Panel i) => i.hotbarBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y));
						if (panel.closeBounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y))
						{
							this.FocusPanel(panel);
							panel.Close();
							this.draggingPanel = false;
						}
						else
						{
							this.FocusPanel(panel);
							this.mouseDragPos = new Vector2((float)Engine.input.Mouse.X, (float)Engine.input.Mouse.Y);
							this.draggingPanel = true;
						}
					}
					else if (this.panels.Exists((Panel i) => i.bounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y)))
					{
						Panel panel = this.panels.Find((Panel i) => i.bounds.Contains(Engine.input.Mouse.X, Engine.input.Mouse.Y));
						this.FocusPanel(panel);
						this.draggingPanel = false;
					}
					else
					{
						this.draggingPanel = false;
						this.UnfocusPanel();
					}
				}
				else if (Engine.input.WasButtonDown(MouseButton.Left) && this.focusedPanel != null)
				{
					if (this.focusedPanel.Moveable && this.draggingPanel)
					{
						this.focusedPanel.position += -(this.mouseDragPos - new Vector2((float)Engine.input.Mouse.X, (float)Engine.input.Mouse.Y));
						this.mouseDragPos = new Vector2((float)Engine.input.Mouse.X, (float)Engine.input.Mouse.Y);
					}
				}
			}
		}

		public void Render()
		{
			foreach (Panel current in this.panels.Reverse<Panel>())
			{
				current.Render();
			}
		}
	}
}
