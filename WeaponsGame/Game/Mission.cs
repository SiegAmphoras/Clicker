using System;
using System.Collections.Generic;
using System.Linq;

namespace WeaponsGame.Game
{
	internal class Mission
	{
		public int level = 1;

        public int enemiesKilled = 0;
		public int enemyCount = 1;

		public EnvironmentType environment;

        public List<MissionReward> Rewards;

        public bool DidPlayerWin = false;

		List<CharacterArchetypeStruct> archetypes;

		public Mission()
		{
            Rewards = new List<MissionReward>();

			environment = EnvironmentType.Forest;
			archetypes = new List<CharacterArchetypeStruct>();
			FillArchetypes();
		}

		public void FillArchetypes()
		{
			archetypes.AddRange((from i in Engine.game.enemyArchetypes
			where i.environment == this.environment
			select i).ToArray<CharacterArchetypeStruct>());
		}

		public CharacterCard[] GenerateMissionRoster()
		{
			List<CharacterCard> list = new List<CharacterCard>();
			for (int i = 0; i < enemyCount; i++)
			{
				list.Add(archetypes[Engine.rand.Next(0, archetypes.Count)].ToCharacterCard());
			}
			return list.ToArray();
		}

		public int GetSlotCount()
		{
			return 1 + (int)System.Math.Floor(System.Math.Pow((double)level, 0.64999997615814209) / 3.0);
		}
	}
}
