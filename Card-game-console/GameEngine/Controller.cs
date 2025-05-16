using System;
using System.Runtime.Serialization;

namespace ForJohnny
{

    [DataContract]
    /// <summary> Игровая логика </summary>
    class Controller
    {
        [DataMember]
        GameManager game;

        EnemyAI enemyAI;

        [DataMember]
        public bool WaitFlag { get; set; }
        [DataMember]
        public bool BattleFlag { get; set; }

        [DataMember]
        private string logs;

        public Controller() {
            game = new GameManager();
            enemyAI = new EnemyAI(game);
            Debug.DebugInit(game);
            logs = "";
        }

        public string GameOutPut() {
            return game.GameStatus() + game.BorderStatus() + game.PlayerHandStatus();
        }

        public void PlayCard()
        {
            try
            {
                Console.WriteLine("\nСписок карт в руке");

                if (game.PlayerCards.Count == 0)
                    throw new Exception("В руке нет карт");

                for (int i = 0; i < game.PlayerCards.Count; i++)
                    Console.WriteLine($"{i + 1}. {game.PlayerCards[i].CardStatus()}");

                Console.WriteLine("\nВыберите какую карту сыграть:");
                int j = Convert.ToInt32(Console.ReadLine());

                game.PutCard(j);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);

                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        public void Skip() {

            if (enemyAI.GameMove())
            {
                if (WaitFlag)
                {
                    WaitFlag = false;
                    return;
                }
                game.RoundBegin();
                Info();

                game.RoundStart();
                Info();

                game.RoundEnd();
                Info();

                game.NewRound();
            }

            Info();
        }

        private void Info() {
            Console.Clear();
            Console.WriteLine(GameOutPut());
            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }

        public void Save() {
            logs = Debug.SavingLogs();
            game.Save();
        }

        public void Load() {
            Debug.LoadLogs(logs);
            game.Load();
            enemyAI = new EnemyAI(game);
        }
    }
}
