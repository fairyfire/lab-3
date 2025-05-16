using System.Linq;

namespace ForJohnny
{
    /// <summary> Искусственный интеллект врага </summary>
    public class EnemyAI
    {
        private GameManager game;
        private bool attack_status;

        public EnemyAI(GameManager game_status) {
            game = game_status;
        }
        public bool GameMove() {

            attack_status = (game.RoundNumber % 2 == 0);

            if (attack_status)
            {
                if (MovePossible(CardType.Warrior) && SearchOnBoard(false) < 2)
                {
                    PutWarroir();
                    return false;
                }
                else if (MovePossible(CardType.Spell_Attack) && SearchOnBoard(true) > 0)
                {
                    SpellingAttack();
                    return false;
                }

                if (MovePossible(CardType.Spell_Improve) && SearchOnBoard(false) > 0)
                {
                    SpellingImprove();
                    return false;
                }
            }
            else {
                PutDefend();

                if (MovePossible(CardType.Spell_Improve) && SearchOnBoard(false) > 0)
                {
                    SpellingImprove();
                    return false;
                }
            }

            return true;
        }

        /// <summary> Определение самый дешевой карты, заданного типа </summary>
        private Card СheapestCard(CardType cardType)
        {
            var cards = from card in game.EnemyCards
                        where card.GetCardType == cardType
                        orderby card.Price
                        select card;

            if (cards.Count<Card>() == 0)
                return new Card();
            else
                return cards.First<Card>();
        }

        /// <summary> Поиск существа на поле </summary>
        private int SearchOnBoard(bool Enemy) {

            var game_lines = from line in game.gameLines
                             where (Enemy) ? (line.FriendlyWarrior.GameStatus) : (line.EnemyWarrior.GameStatus)
                             select line;

            return game_lines.Count<GameLine>();
        }

        /// <summary> Размещение существ для атаки </summary>
        private void PutWarroir() {
            for (int i = 0; i < game.gameLines.Length; i++)
                if (!game.gameLines[i].EnemyWarrior.GameStatus)
                {
                    CardWarrior warrior;

                    if (СheapestCard(CardType.Warrior).GetCardType != CardType.CardNull)
                    {
                        warrior = (CardWarrior)СheapestCard(CardType.Warrior);

                        if (warrior.Price <= game.Enemy_MP)
                        {
                            Debug.Log($"Round {game.RoundNumber} Враг поместил на поле врага {game.gameLines[i].EnemyWarrior.CardStatus()}");
                            game.gameLines[i].EnemyWarrior = warrior;
                            game.Enemy_MP -= warrior.Price;
                            game.EnemyCards.Remove(warrior);

                            break;
                        }
                    }
                }
        }

        /// <summary> Размещение существ для защиты </summary>
        private void PutDefend() {

            for (int i = 0; i < game.gameLines.Length; i++)
                if (game.gameLines[i].FriendlyWarrior.GameStatus && !game.gameLines[i].EnemyWarrior.GameStatus)
                {
                    CardWarrior warrior;

                    if (СheapestCard(CardType.Warrior).GetCardType != CardType.CardNull)
                    {
                        warrior = (CardWarrior)СheapestCard(CardType.Warrior);

                        if (warrior.Price <= game.Enemy_MP)
                        {
                            Debug.Log($"Round {game.RoundNumber} Враг поместил на поле врага для защиты {game.gameLines[i].EnemyWarrior.CardStatus()}");
                            game.gameLines[i].EnemyWarrior = warrior;
                            game.Enemy_MP -= warrior.Price;
                            game.EnemyCards.Remove(warrior);
                        }
                    }
                }
        }

        /// <summary> Размещение карты улучшения </summary>
        private void SpellingImprove() {
            for (int i = 0; i < game.gameLines.Length; i++)
                if (game.gameLines[i].EnemyWarrior.GameStatus)
                {
                    CardSpellImprove spellImprove;

                    if (СheapestCard(CardType.Spell_Improve).GetCardType != CardType.CardNull)
                    {
                        spellImprove = (CardSpellImprove)СheapestCard(CardType.Spell_Improve);

                        if (spellImprove.Price <= game.Enemy_MP)
                        {
                            Debug.Log($"Round {game.RoundNumber} Враг сыграл заклинание усиление '{spellImprove.Name}' по {game.gameLines[i].EnemyWarrior.CardStatus()}");
                            spellImprove.Modification(game.gameLines[i].EnemyWarrior);
                            game.Enemy_MP -= spellImprove.Price;
                            game.EnemyCards.Remove(spellImprove);

                            Debug.Log($"Round {game.RoundNumber} Враг сыграл заклинание усиление");

                            break;
                        }
                    }
                }

        }

        /// <summary> Выбрасывание карты, наносящей урон </summary>
        private void SpellingAttack() {
            for (int i = 0; i < game.gameLines.Length; i++)
                if (game.gameLines[i].FriendlyWarrior.GameStatus)
                {
                    CardSpellAttack spellAttack;

                    if (СheapestCard(CardType.Spell_Attack).GetCardType != CardType.CardNull)
                    {
                        spellAttack = (CardSpellAttack)СheapestCard(CardType.Spell_Attack);

                        if (spellAttack.Price <= game.Enemy_MP)
                        {
                            Debug.Log($"Round {game.RoundNumber} Враг сыграл заклинание атаки {spellAttack.Name} по {game.gameLines[i].FriendlyWarrior.CardStatus()}");

                            spellAttack.Damage(game.gameLines[i].FriendlyWarrior);
                            game.Enemy_MP -= spellAttack.Price;
                            game.EnemyCards.Remove(spellAttack);

                            break;
                        }
                    }
                }
        }

        /// <summary> Определение можно ли сыграть карту </summary>
        private bool MovePossible(CardType cardType) {
            return (СheapestCard(cardType).GetCardType != CardType.CardNull && СheapestCard(cardType).Price <= game.Enemy_MP);
        }
    }
}
