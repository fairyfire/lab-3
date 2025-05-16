using System;
using System.Collections.Generic;
using System.Linq;

namespace ForJohnny
{
    public static class CardsManager
    {
        private static Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        /// <summary> Колода карт игрока </summary>
        private static List<Card> Player_Deck = new List<Card>() {
            new CardWarrior(1, "Воин", 1, 1, Effects.Null),
            new CardWarrior(1, "Воин с щитом", 1, 1, Effects.Hardness),
            new CardWarrior(2, "Сильный воин", 2, 2, Effects.Null),
            new CardWarrior(4, "Паладин", 2, 2, Effects.Hardness),
            new CardWarrior(1, "Живое пламя", 4, 4, Effects.Finite),

            new CardSpellAttack(2, "Огненный шар", 2),
            new CardSpellImprove(1, "Волшебный щит", Effects.Hardness),
            new CardSpellImprove(1, "Каменная кожа", 0, 2),
            new CardSpellHealing(0, "Слабое заклинание исцеления", 1),
        };

        /// <summary> Колода карт врага </summary>
        private static List<Card> Enemy_Deck = new List<Card>() {
            new CardWarrior(1, "Скелет", 1, 1, Effects.Null),
            new CardWarrior(1, "Сильный Скелет", 3, 3, Effects.Null),
            new CardWarrior(1, "Скелет", 1, 1, Effects.Null),
            new CardWarrior(1, "Скелет с щитом", 1, 1, Effects.Hardness),
            new CardWarrior(1, "Скелет", 1, 1, Effects.Null),

            new CardSpellAttack(2, "Огненный шар", 2),
        };

        /// <summary> Получение перемешанной колоды Игроком </summary>
        public static List<Card> GetPlayerDeck() {
            return GetDeck(Player_Deck);
        }

        /// <summary> Получение перемешанной колоды Врагом </summary>
        public static List<Card> GetEnemyDeck() {
            return GetDeck(Enemy_Deck);
        }

        /// <summary> Перемешивание колоды </summary>
        private static List<Card> GetDeck(List<Card> deck) {

            for (int i = deck.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);

                var temp = deck[j];
                deck[j] = deck[i];
                deck[i] = temp;
            }
            return deck;
        }

    }
}
