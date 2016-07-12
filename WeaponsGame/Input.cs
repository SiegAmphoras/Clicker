using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace WeaponsGame
{
	internal class Input
	{
		private GameWindow gw;

		private System.Collections.Generic.List<MouseButton> mousebuttons;

		private System.Collections.Generic.List<MouseButton> lastMousebuttons;

		private System.Collections.Generic.List<Key> keyState;

		private System.Collections.Generic.List<Key> lastKeyState;

		public MouseDevice Mouse
		{
			get
			{
				return this.gw.Mouse;
			}
		}

		public Input(ref GameWindow gw)
		{
			this.gw = gw;
			this.mousebuttons = new System.Collections.Generic.List<MouseButton>();
			this.lastMousebuttons = new System.Collections.Generic.List<MouseButton>();
			this.keyState = new System.Collections.Generic.List<Key>();
			this.lastKeyState = new System.Collections.Generic.List<Key>();
		}

		private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.mousebuttons.Remove(e.Button);
		}

		private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.mousebuttons.Add(e.Button);
		}

		public void Update()
		{
			this.lastMousebuttons.Clear();
			this.lastMousebuttons.AddRange(this.mousebuttons.ToArray());
			this.mousebuttons.Clear();
			MouseState state = OpenTK.Input.Mouse.GetState();
			foreach (MouseButton mouseButton in System.Enum.GetValues(typeof(MouseButton)))
			{
				if (state.IsButtonDown(mouseButton))
				{
					this.mousebuttons.Add(mouseButton);
				}
			}
			this.lastKeyState.Clear();
			this.lastKeyState.AddRange(this.keyState.ToArray());
			this.keyState.Clear();
			KeyboardState state2 = Keyboard.GetState();
			foreach (Key key in System.Enum.GetValues(typeof(Key)))
			{
				if (state2.IsKeyDown(key))
				{
					this.keyState.Add(key);
				}
			}
		}

		public bool IsButtonDown(MouseButton button)
		{
			return this.mousebuttons.Contains(button);
		}

		public bool WasButtonDown(MouseButton button)
		{
			return this.lastMousebuttons.Contains(button);
		}

		public bool IsKeyDown(Key key)
		{
			return this.keyState.Contains(key);
		}

		public bool WasKeyDown(Key key)
		{
			return this.lastKeyState.Contains(key);
		}
	}
}
