using System.Runtime.Serialization;

namespace ForJohnny
{
    [DataContract]
    /// <summary> Заклинание улучшения </summary>
    public class CardSpellImprove: Card
    {
        [DataMember]
        private int attack_mod;
        [DataMember]
        private int health_mod;

        [DataMember]
        private Effects effect_mod;

        public CardSpellImprove() {
            Price = 0;
            Description = "";
            DebugOff = false;
        }

        public CardSpellImprove(int price, string name, int attack_mod_value, int health_mod_value) {
            Price = price;
            Name = name;

            attack_mod = attack_mod_value;
            health_mod = health_mod_value;

            effect_mod = Effects.Null;
            DebugOff = false;
        }

        public CardSpellImprove(int price, string name, int attack_mod_value, int health_mod_value, Effects effect) {
            Price = price;
            Name = name;

            attack_mod = attack_mod_value;
            health_mod = health_mod_value;

            effect_mod = effect;
            DebugOff = false;
        }

        public CardSpellImprove(int price, string name, Effects effect) {
            Price = price;
            Name = name;

            effect_mod = effect;

            attack_mod = 0;
            health_mod = 0;

            DebugOff = false;
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Spell_Improve;
            }
        }

        public void Modification(CardWarrior friendly_warrior) {

            if (!DebugOff)
                Debug.Log($"Карта '{Name}' модификация {friendly_warrior.CardStatus()} на величину {attack_mod}/{health_mod}");

            friendly_warrior.Modification(attack_mod, health_mod);

            if(effect_mod != Effects.Null)
                friendly_warrior.Effect = effect_mod;
        }

        public override string CardStatus()
        {
            string effect = string.Empty;
            switch (effect_mod)
            {
                case Effects.Hardness: effect = "и Стойкость"; break;
                case Effects.Upgradable: effect = "и Грабитель"; break;
                case Effects.Finite: effect = ", но Выгорание"; break;
            }

            return base.CardStatus() + $"Карта усиления {Name} Усиление {attack_mod}/{health_mod} {effect}";
        }
    }
}
