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

		public List<Mission> missionsAvailable;

		public List<CharacterArchetypeStruct> enemyArchetypes;

        public List<Blueprint> blueprints;

		public Game()
		{
			this.missionsAvailable = new List<Mission>();
            this.enemyArchetypes = new List<CharacterArchetypeStruct>();
		    blueprints= new List<Blueprint>();
        }

		public void Startup()
		{
			this.localPlayer = new Player();
			this.RegisterArchetypes();
            RegisterBlueprints();
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
			string[] names = Enum.GetNames(typeof(EnvironmentType));
			mission.environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), names[Engine.rand.Next(1, 3)]);

            //TODO: Generate rewards
            mission.Rewards.Add(new MissionMoneyReward() { RewardName = "Completion Bonus", rewardAmount = mission.level * 500, DisplayReward = true });

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

        public void RegisterBlueprints()
        {
            string[] files = System.IO.Directory.GetFiles("content/blueprints");
            string[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                string path = array[i];
                Blueprint item = Blueprint.LoadFromFile(path);
                blueprints.Add(item);
            }
        }

		public void PostMissionFinish(bool won)
		{
            if (won)
            {
                foreach (MissionReward r in activeMission.Rewards)
                {
                    r.GiveReward();
                }
            }

			this.missionsAvailable.Add(this.GenerateMissions(Engine.rand.Next(5, 20)));
		}
	}
}
