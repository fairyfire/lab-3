using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ForJohnny
{
    [DataContract]
    /// <summary> Класс определяющий действия игрока на игровом поле </summary>
    public class GameBoardEvents: GameBoard
    {
        /// <summary> Новый раунд игры </summary>
        public void NewRound() {
            if (PlayerCardDeck.Count > 0)
            {
                PlayerCards.Add(PlayerCardDeck[0]);
                PlayerCardDeck.RemoveAt(0);
            }

            if (EnemyCardDeck.Count > 0)
            {
                EnemyCards.Add(EnemyCardDeck[0]);
                EnemyCardDeck.RemoveAt(0);
            }

            RoundNumber++;
            Player_MP = Enemy_MP = RoundNumber + 1;

            Player_MP = Enemy_MP = (Enemy_MP > MaxMP) ? (MaxMP):(Enemy_MP);
        }

        /// <summary> Игрок размещается карту </summary>
        public void PutCard(int card_number) {

            var selected_card = PlayerCards[card_number - 1];

            if (selected_card.Price > Player_MP)
                throw new Exception("Нехватает MP, чтобы разыграть карту");

            var lines = from GameLine line in gameLines
                        where line.EnemyWarrior.GameStatus == true
                        select line;

            if (lines.Count<GameLine>() == 0 && RoundNumber % 2 == 0)
                throw new Exception("Нет атакующих врагов");
            

            switch (selected_card.GetCardType)
            {
                case CardType.Warrior: PutWarrior((CardWarrior)selected_card); break;
                case CardType.Spell_Attack: PutSpellAttack((CardSpellAttack)selected_card); break;
                case CardType.Spell_Healing: PutSpellHealing((CardSpellHealing)selected_card); break;
                case CardType.Spell_Improve: PutImproveSpell((CardSpellImprove)selected_card); break;
            }
        }

        /// <summary> Игрок размещается существо на поле </summary>
        public void PutWarrior(CardWarrior warrior) {
            for (int i = 0; i < gameLines.Length; i++)
                if (!gameLines[i].FriendlyWarrior.GameStatus)
                {
                    Player_MP -= warrior.Price;
                    gameLines[i].FriendlyWarrior = warrior;
                    PlayerCards.Remove(warrior);
                    return;
                }

            throw new Exception("Нет места для нового бойца");
        }

        /// <summary> Игрок выбрасывает карту, наносящую урон </summary>
        public void PutSpellAttack(CardSpellAttack spell) {

            var active_enemys = new List<CardWarrior>(0);
            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.EnemyWarrior.GameStatus)
                    active_enemys.Add(gameLine.EnemyWarrior);

            }

            if (active_enemys.Count == 0)
                throw new Exception("Нет врагов, чтобы применить заклинание атаки");

            Console.WriteLine("\nВыберите врага для заклинания:");

            for (int i = 0; i < active_enemys.Count; i++)
                Console.WriteLine(active_enemys[i].CardStatus());

            int j = Convert.ToInt32(Console.ReadLine());

            Player_MP -= spell.Price;
            spell.Damage(active_enemys[j - 1]);
            PlayerCards.Remove(spell);
        }

        /// <summary> Игрок выбрасывает карту, лечащую союзное существо </summary>
        public void PutSpellHealing(CardSpellHealing spell) {
            var friendly_warriors = new List<CardWarrior>(0);
            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.FriendlyWarrior.GameStatus)
                    friendly_warriors.Add(gameLine.FriendlyWarrior);
            }

            if (friendly_warriors.Count == 0)
                throw new Exception("Нет бойцов, чтобы применить заклинание");

            Console.WriteLine("\nВыберите бойца для лечения:");

            for (int i = 0; i < friendly_warriors.Count; i++)
                Console.WriteLine(friendly_warriors[i].CardStatus());

            int j = Convert.ToInt32(Console.ReadLine());

            Player_MP -= spell.Price;
            spell.Treatment(friendly_warriors[j - 1]);
            PlayerCards.Remove(spell);

        }

        /// <summary> Игрок выбрасывает карту улучшение </summary>
        public void PutImproveSpell(CardSpellImprove spell) {
            var line_for_spell = new List<GameLine>(0);
            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.FriendlyWarrior.GameStatus)
                    line_for_spell.Add(gameLine);
            }

            if (line_for_spell.Count == 0)
                throw new Exception("Нет бойцов, чтобы применить заклинание");

            Console.WriteLine("\nВыберите линию для заклинания:");

            for (int i = 0; i < line_for_spell.Count; i++)
            {
                string enemy_warrior = (line_for_spell[i].EnemyWarrior.GameStatus) ? (line_for_spell[i].EnemyWarrior.CardStatus()) : ("###");
                string friendly_warrior = (line_for_spell[i].FriendlyWarrior.GameStatus) ? (line_for_spell[i].FriendlyWarrior.CardStatus()) : ("###");

                Console.WriteLine($"{((enemy_warrior == "###" && friendly_warrior == "###") ? ($"Поле {i + 1} пустое") : ($"Поле {i + 1} {enemy_warrior} vs {friendly_warrior}"))}");
            }

            int j = Convert.ToInt32(Console.ReadLine());

            Player_MP -= spell.Price;
            spell.Modification(line_for_spell[j - 1].FriendlyWarrior);
            PlayerCards.Remove(spell);

        }
    }
}
