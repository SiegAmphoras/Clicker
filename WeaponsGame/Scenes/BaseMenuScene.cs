using OpenTK;
using OpenTK.Input;
using System;
using WeaponsGame.Game.UI;
using WeaponsGame.UI;

namespace WeaponsGame.Scenes
{
	internal class BaseMenuScene : Scene
	{
		private menu_gui menu;

		private float yaw = 0f;

		public override void Load()
		{
			base.Load();
			this.cursorTexture = Renderer.GetTexture("cursor_pointer");
			this.cursorOffset = new Vector2(0f, 0f);
			this.menu = new menu_gui(this.guiManager);
			this.menu.Construct();
			(this.guiManager["menuPanel"]["quitButton"] as Button).Clicked += new Button.ClickedEvent(this.BaseMenuScene_Clicked);
		}

		private void BaseMenuScene_Clicked(MouseButtonEventArgs e)
		{
			base.TransitionOut();
			base.OnTransitionFinished += new Scene.TransitionFinished(this.BaseMenuScene_TransitionQuit);
		}

		private void BaseMenuScene_TransitionQuit(object sender, System.EventArgs e)
		{
			Engine.reference.Quit();
		}

		public override void UnLoad()
		{
			base.UnLoad();
		}

		public override void Update()
		{
			this.yaw += 0.5f;
			(this.guiManager["statsPanel"]["statsCashLabel"] as TextLabel).Text = string.Concat(new object[]
			{
				"$: ",
				Engine.game.cash,
				"     m: ",
				Engine.game.missionsAvailable.Count
			});
			(this.guiManager["statsPanel"]["statsPersonnelLabel"] as TextLabel).Text = "P: " + Engine.game.Personell.Count;
			base.Update();
		}

		public override void Render()
		{
			Engine.renderer.Prepare3D();
			Renderer.DrawTestCube(new Vector3(0f, 0f, -10f), new Vector3(this.yaw, this.yaw, 0f), Vector3.One * 1f);
			Engine.renderer.End3D();
			Engine.renderer.Prepare2D();
			base.Render();
			Engine.renderer.End2D();
		}
	}
}
