using System;
using System.Collections.Generic;
using System.IO;
using WeaponsGame.Scenes;

namespace WeaponsGame.Game
{
	internal class Game
	{
		public Player localPlayer;

		public int cash = 0;

		public Mission activeMission;

		public System.Collections.Generic.List<Mission> missionsAvailable;

		public System.Collections.Generic.List<CharacterArchetypeStruct> enemyArchetypes;

		public Game()
		{
			this.missionsAvailable = new System.Collections.Generic.List<Mission>();
			this.enemyArchetypes = new System.Collections.Generic.List<CharacterArchetypeStruct>();
		}

		public void Startup()
		{
			this.localPlayer = new Player();
			this.RegisterArchetypes();
			for (int i = 0; i < 4; i++)
			{
				this.missionsAvailable.Add(this.GenerateMissions(Engine.rand.Next(5, 20)));
			}
			MissionScene newScene = new MissionScene();
			Engine.sceneManager.RegisterScene("MissionScene", newScene);
			BaseMenuScene newScene2 = new BaseMenuScene();
			Engine.sceneManager.RegisterScene("BaseMenuScene", newScene2);
			Engine.sceneManager.SetActiveScene("BaseMenuScene");
		}

		public Mission GenerateMissions(int count)
		{
			Mission mission = new Mission();
			mission.level = Engine.rand.Next(1, 20);
			mission.enemyCount = count;
			string[] names = System.Enum.GetNames(typeof(EnvironmentType));
			mission.environment = (EnvironmentType)System.Enum.Parse(typeof(EnvironmentType), names[Engine.rand.Next(1, 3)]);
			return mission;
		}

		public void RegisterArchetypes()
		{
			string[] files = System.IO.Directory.GetFiles("content/archetypes");
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string path = array[i];
				CharacterArchetypeStruct item = CharacterArchetypeStruct.LoadFromFile(path);
				this.enemyArchetypes.Add(item);
			}
		}

		public void PostMissionFinish()
		{
			this.missionsAvailable.Add(this.GenerateMissions(Engine.rand.Next(5, 20)));
		}
	}
}
