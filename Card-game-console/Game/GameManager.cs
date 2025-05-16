using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ForJohnny
{
    [DataContract]
    /// <summary> Класс определяющий вывод данных об игре </summary>
    public class GameManager : GameBoardEvents
    {
        public GameManager()
        {
            Player_HP = Enemy_HP = 10;
            Player_MP = Enemy_MP = 2;

            PlayerCardDeck = CardsManager.GetPlayerDeck();
            EnemyCardDeck = CardsManager.GetEnemyDeck();

            PlayerCards = new List<Card>(0);
            EnemyCards = new List<Card>(0);

            RoundNumber = 1;

            gameLines = new GameLine[5] {
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
            };
        }

        public string BorderStatus()
        {
            string status = "Игровое поле:\n";

            for (int i = 0; i < gameLines.Length; i++)
            {

                string enemy_warrior = (gameLines[i].EnemyWarrior.GameStatus) ? (gameLines[i].EnemyWarrior.CardStatus()) : ("###");
                string friendly_warrior = (gameLines[i].FriendlyWarrior.GameStatus) ? (gameLines[i].FriendlyWarrior.CardStatus()) : ("###");

                status += $"{((enemy_warrior == "###" && friendly_warrior == "###") ? ($"Поле {i + 1} пустое") : ($"Поле {i + 1} {enemy_warrior} vs {friendly_warrior}"))}\n";
            }

            return status + "\n";
        }

        public string GameStatus()
        {
            return $"Round: {RoundNumber} {((RoundNumber % 2 != 0) ? ("Вы атакуете"):("Вы защищаетесь"))}\nHP Игрока {Player_HP} MP Игрока {Player_MP}\nHP Врага {Enemy_HP} MP Врага {Enemy_MP}\n";
        }

        public string PlayerHandStatus()
        {
            string status = "";
            for (int i = 0; i < PlayerCards.Count; i++)
                status += $"{i + 1} {PlayerCards[i].CardStatus()} \n";

            return status;
        }
    }
}
