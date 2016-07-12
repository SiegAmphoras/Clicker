using System;
using System.Collections.Generic;
using System.Linq;

namespace WeaponsGame.Game
{
	internal class Mission
	{
		public int level = 1;

		public int enemyCount = 1;

		public EnvironmentType environment;

		private System.Collections.Generic.List<CharacterArchetypeStruct> archetypes;

		public Mission()
		{
			this.environment = EnvironmentType.Forest;
			this.archetypes = new System.Collections.Generic.List<CharacterArchetypeStruct>();
			this.FillArchetypes();
		}

		public void FillArchetypes()
		{
			this.archetypes.AddRange((from i in Engine.game.enemyArchetypes
			where i.environment == this.environment
			select i).ToArray<CharacterArchetypeStruct>());
		}

		public CharacterCard[] GenerateMissionRoster()
		{
			System.Collections.Generic.List<CharacterCard> list = new System.Collections.Generic.List<CharacterCard>();
			for (int i = 0; i < this.enemyCount; i++)
			{
				list.Add(this.archetypes[Engine.rand.Next(0, this.archetypes.Count)].ToCharacterCard());
			}
			return list.ToArray();
		}

		public int GetSlotCount()
		{
			return 1 + (int)System.Math.Floor(System.Math.Pow((double)this.level, 0.64999997615814209) / 3.0);
		}
	}
}
