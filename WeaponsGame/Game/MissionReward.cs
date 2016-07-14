using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponsGame.Game
{
    class MissionReward
    {
        public string RewardName;
        public bool DisplayReward = false;

        public MissionReward() { }

        public virtual void GiveReward() { }

        public virtual string GetRewardText() { return ""; }
    }

    class MissionItemReward : MissionReward
    {
        public BaseItem item;

        public MissionItemReward() { }

        public override void GiveReward()
        {
            //TODO: give item
            
            base.GiveReward();
        }

        public override string GetRewardText()
        {
            string txt = RewardName;

            if (DisplayReward) { txt += " - " + item.Name; }

            return txt;
        }
    }

    class MissionMoneyReward : MissionReward
    {
        public int rewardAmount;

        public MissionMoneyReward() { }

        public override void GiveReward()
        {
            Engine.game.cash += rewardAmount;

            base.GiveReward();
        }

        public override string GetRewardText()
        {
            string txt = RewardName;

            if (DisplayReward) { txt += " - $" + rewardAmount; }

            return txt;
        }
    }

    class MissionCharacterReward : MissionReward
    {
        public CharacterArchetypeStruct characterTemplate;
        public int quantity = 1;

        public override void GiveReward()
        {
            for (int i = 0; i < quantity; i++) { Engine.game.Personell.Add(characterTemplate.ToCharacterCard()); }

            base.GiveReward();
        }

        public override string GetRewardText()
        {
            return quantity + " x " + characterTemplate.Name;
        }
    }
}
