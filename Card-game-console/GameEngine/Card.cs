using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ForJohnny
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardType
    {
        CardNull,
        Warrior,
        Spell_Improve,
        Spell_Attack,
        Spell_Healing
    }

    [DataContract]
    public class Card
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        /// <summary> Описание карты </summary>
        public string Description { get; set; }

        [DataMember]
        /// <summary> Отключение вывода статистики </summary>
        public bool DebugOff { get; set; }

        public virtual CardType GetCardType
        {
            get { return CardType.CardNull; }
        }

        public virtual string CardStatus()
        {
            return $"({Price})";
        }

        public Card()
        {
            Price = 0;
            Description = "";
        }
    }
}
