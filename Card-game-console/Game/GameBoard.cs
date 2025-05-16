using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace ForJohnny
{
    [DataContract]
    /// <summary> Игровая линия, на которой существа бьются друг с другом </summary>
    public struct GameLine {
        [DataMember]
        public CardWarrior EnemyWarrior { get; set; }
        [DataMember]
        public CardWarrior FriendlyWarrior { get; set; }

        public GameLine(bool init) {
            this.EnemyWarrior = new CardWarrior();
            this.FriendlyWarrior = new CardWarrior();
        } 
    }

    [DataContract]
    /// <summary> Класс определяющий игровое поле и основые этапы игры на нем </summary>
    public class GameBoard
    {
        [DataMember]
        /// <summary> Здоровье врага </summary>
        public int Enemy_HP { get; set; }
        [DataMember]
        /// <summary> Здоровье игрока </summary>
        public int Player_HP { get; set; }

        [DataMember]
        /// <summary> Очки маны врага </summary>
        public int Enemy_MP { get; set; }
        [DataMember]
        /// <summary> Очки маны игрока </summary>
        public int Player_MP { get; set; }

        /// <summary> Карты в руке врага </summary>
        public List<Card> EnemyCards { get; set; }
        /// <summary> Карты в руке игрока </summary>
        public List<Card> PlayerCards { get; set; }

   

        /// <summary> Карты колоде врага </summary>
        public List<Card> EnemyCardDeck { get; set; }
        /// <summary> Карты колоде игрока </summary>
        public List<Card> PlayerCardDeck { get; set; }


        [DataMember]
        /// <summary> Игровые линии, на которых существа бьются друг с другом </summary>
        public GameLine[] gameLines { get; set; }

        [DataMember]
        /// <summary> Максимальное значение маны и врага и игрока </summary>
        protected const int MaxMP = 10;

        [DataMember]
        /// <summary> Счетчик раундов </summary>
        public int RoundNumber { get; set; }

        [DataMember]
        /// <summary> Отключение вывода статистики </summary>
        public bool DebugOff { get; set; }

        /// <summary> 1 этап битвы </summary>
        public void RoundBegin() {

            for (int i = 0; i < gameLines.Length; i++)
            {
                CardWarrior attack_warrior = (RoundNumber % 2 != 0) ? (gameLines[i].FriendlyWarrior) : (gameLines[i].EnemyWarrior);
                CardWarrior defend_warrior = (RoundNumber % 2 != 0) ? (gameLines[i].EnemyWarrior) : (gameLines[i].FriendlyWarrior);

                attack_warrior.AttackStatus();
                defend_warrior.DefendStatus();
            }
        }

        /// <summary> 2 этап битвы - основной, существа наносят урон </summary>
        public void RoundStart()
        {
            int attack_damage;
            int defend_damage;

            for (int i = 0; i < gameLines.Length; i++)
            {
                CardWarrior attack_warrior = (RoundNumber % 2 != 0) ? (gameLines[i].FriendlyWarrior) : (gameLines[i].EnemyWarrior);
                CardWarrior defend_warrior = (RoundNumber % 2 != 0) ? (gameLines[i].EnemyWarrior) : (gameLines[i].FriendlyWarrior);

                if (defend_warrior.GameStatus)
                {
                    attack_damage = attack_warrior.AttackPoints;
                    defend_damage = defend_warrior.AttackPoints;

                    defend_warrior.Damage(attack_damage);
                    attack_warrior.Damage(defend_damage);
                }
                else {
                    if (RoundNumber % 2 != 0)
                    {
                        if (!DebugOff)
                            Debug.Log("Враг получает урон");
                        Enemy_HP -= attack_warrior.AttackPoints;
                    }
                    else
                    {
                        if (!DebugOff)
                            Debug.Log("Игрок получает урон");
                        Player_HP -= attack_warrior.AttackPoints;
                    }
                }
            }
        }

        /// <summary> 3 этап битвы - выжившие существа возвращаются в руку </summary>
        public void RoundEnd()
        {
            for (int i = 0; i < gameLines.Length; i++)
            {
                if (gameLines[i].EnemyWarrior.GameStatus)
                    EnemyCards.Add(gameLines[i].EnemyWarrior);

                gameLines[i].EnemyWarrior = new CardWarrior();

                if (gameLines[i].FriendlyWarrior.GameStatus)
                    PlayerCards.Add(gameLines[i].FriendlyWarrior);

                gameLines[i].FriendlyWarrior = new CardWarrior();
            }

            CheckHP();
        }

        /// <summary> Проверка здоровья игрока и врага </summary>
        private void CheckHP() {
            if (Player_HP <= 0 || Enemy_HP <= 0)
            {
                Console.Clear();

                Console.WriteLine("-------Игра оконченна-------");
                Console.WriteLine((Enemy_HP <= 0) ? ("Вы победили") : ((Player_HP <= 0) ? ("Вы проиграли") : ("")));

                Debug.Log("-------Игра оконченна-------");
                Console.WriteLine("Нажмите чтобы продолжить...");

                if(File.Exists("GameSave.json"))
                    File.Delete("GameSave.json");

                Console.WriteLine(Debug.GetLogs());
                Console.ReadKey();
            }
            
        }

        /// <summary> Сохранение (Сериализация данных) </summary>
        public void Save() {

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            string PlayerHand = JsonConvert.SerializeObject(PlayerCards, settings);
            string EnemyHand = JsonConvert.SerializeObject(EnemyCards, settings);

            string PlayerDeck = JsonConvert.SerializeObject(PlayerCardDeck, settings);
            string EnemyDeck = JsonConvert.SerializeObject(EnemyCardDeck, settings);

            

            DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));

            using (FileStream fileStream = new FileStream("PlayerHandCards.json", FileMode.Create))
                jsonF.WriteObject(fileStream, PlayerHand);

            using (FileStream fileStream = new FileStream("EnemyHandCards.json", FileMode.Create))
                jsonF.WriteObject(fileStream, EnemyHand);

            using (FileStream fileStream = new FileStream("PlayerDeckCards.json", FileMode.Create))
                jsonF.WriteObject(fileStream, PlayerDeck);

            using (FileStream fileStream = new FileStream("EnemyDeckCards.json", FileMode.Create))
                jsonF.WriteObject(fileStream, EnemyDeck);

        }

        /// <summary> Загрузка (Десериализация данных) </summary>
        public void Load() {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));

            using (FileStream fileStream = new FileStream("PlayerHandCards.json", FileMode.Open))
                PlayerCards = JsonConvert.DeserializeObject<List<Card>>((string)jsonF.ReadObject(fileStream), settings);

            using (FileStream fileStream = new FileStream("EnemyHandCards.json", FileMode.Open))
                EnemyCards = JsonConvert.DeserializeObject<List<Card>>((string)jsonF.ReadObject(fileStream), settings);

            using (FileStream fileStream = new FileStream("PlayerDeckCards.json", FileMode.Open))
                PlayerCardDeck = JsonConvert.DeserializeObject<List<Card>>((string)jsonF.ReadObject(fileStream) , settings);

            using (FileStream fileStream = new FileStream("EnemyDeckCards.json", FileMode.Open))
                EnemyCardDeck = JsonConvert.DeserializeObject<List<Card>>((string)jsonF.ReadObject(fileStream), settings);

        }
    }
}
