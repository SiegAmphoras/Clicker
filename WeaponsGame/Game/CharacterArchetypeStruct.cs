using System;
using System.IO;
using System.Xml.Serialization;

namespace WeaponsGame.Game
{
	public struct CharacterArchetypeStruct
	{
		public string Name;

		public float attackRate;

		public int shotsPerAttack;

		public string characterImageSet;

		public string fireSound;

		public int baseDamage;

		public float pctCritical;

		public float accuracy;

		public int BaseHealth;

		public float gunshotDelay;

		public EnvironmentType environment;

		public static CharacterArchetypeStruct FromCharacterCard(CharacterCard card)
		{
			return new CharacterArchetypeStruct
			{
				Name = card.Name,
				attackRate = card.attackRate,
				shotsPerAttack = card.shotsPerAttack,
				characterImageSet = card.characterImageSet,
				fireSound = card.fireSound,
				baseDamage = card.baseDamage,
				pctCritical = card.pctCritical,
				accuracy = card.accuracy,
				BaseHealth = card.BaseHealth,
				gunshotDelay = card.gunshotDelay,
				environment = card.environment
			};
		}

		public static CharacterArchetypeStruct Default()
		{
			return new CharacterArchetypeStruct
			{
				Name = "UndefinedArchetype",
				attackRate = 3f,
				shotsPerAttack = 2,
				characterImageSet = "en_rus1_ak",
				fireSound = "weapon/fire6.wav",
				baseDamage = 3,
				pctCritical = 0.1f,
				accuracy = 0.75f,
				BaseHealth = 200,
				gunshotDelay = 0.3f,
				environment = EnvironmentType.Forest
			};
		}

		public CharacterCard ToCharacterCard()
		{
			return new CharacterCard
			{
				Name = this.Name,
				attackRate = this.attackRate,
				shotsPerAttack = this.shotsPerAttack,
				characterImageSet = this.characterImageSet,
				fireSound = this.fireSound,
				baseDamage = this.baseDamage,
				pctCritical = this.pctCritical,
				accuracy = this.accuracy,
				BaseHealth = this.BaseHealth,
				gunshotDelay = this.gunshotDelay,
				environment = this.environment
			};
		}

		public static CharacterArchetypeStruct LoadFromFile(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CharacterArchetypeStruct));
			CharacterArchetypeStruct result;
			using (System.IO.TextReader textReader = new System.IO.StreamReader(path))
			{
				object obj = xmlSerializer.Deserialize(textReader);
				result = (CharacterArchetypeStruct)obj;
			}
			return result;
		}
	}
}
