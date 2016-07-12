using System;
using System.Collections.Generic;

namespace WeaponsGame
{
	internal class SceneManager
	{
		private System.Collections.Generic.Dictionary<string, Scene> scenes;

		private Scene activeScene;

		private string nextScene;

		public SceneManager()
		{
			this.scenes = new System.Collections.Generic.Dictionary<string, Scene>();
		}

		public void SetActiveScene(string sceneName)
		{
			if (this.activeScene != null)
			{
				this.nextScene = sceneName;
				this.activeScene.TransitionOut();
				this.activeScene.OnTransitionFinished += new Scene.TransitionFinished(this.activeScene_OnTransitionFinished);
			}
			else
			{
				this.activeScene = this.scenes[sceneName];
				this.activeScene.Load();
			}
		}

		private void activeScene_OnTransitionFinished(object sender, System.EventArgs e)
		{
			this.activeScene.UnLoad();
			this.activeScene = this.scenes[this.nextScene];
			this.activeScene.Load();
		}

		public Scene GetActiveScene()
		{
			return this.activeScene;
		}

		public void RegisterScene(string sceneName, Scene newScene)
		{
			this.scenes.Add(sceneName, newScene);
		}

		public void UnregisterScene(string sceneName)
		{
			this.scenes.Remove(sceneName);
		}

		public void UpdateScene()
		{
			this.activeScene.Update();
		}

		public void RenderScene()
		{
			this.activeScene.Render();
			this.activeScene.PostRender();
		}
	}
}
