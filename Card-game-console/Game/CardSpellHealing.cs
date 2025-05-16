using System.Runtime.Serialization;

namespace ForJohnny
{
    [DataContract]
    /// <summary> Заклинание лечения </summary>
    public class CardSpellHealing: Card
    {
        [DataMember]
        private int treatment;

        public CardSpellHealing(int price, string name,int treatment_value)
        {
            Price = price;
            Name = name;
            treatment = treatment_value;

            DebugOff = false;
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Spell_Healing;
            }
        }

        public void Treatment(CardWarrior cardWarrior)
        {
            if (!DebugOff)
                Debug.Log($"Карта '{Name}' лечение {cardWarrior.CardStatus()} на величину {treatment}");
            cardWarrior.Treatment(treatment);
        }

        public override string CardStatus()
        {
            return base.CardStatus() + $"Карта лечения '{Name}' Лечение: {treatment}";
        }
    }
}
