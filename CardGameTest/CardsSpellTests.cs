using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForJohnny;

namespace CardGameTest
{
    [TestClass]
    class CardsSpellTests
    {
        [TestMethod]
        public void HealSpellTest()
        {

            //arrange
            int attack = 2;
            int health = 3;

            int damage = 2;

            int expected_health = health - damage + 1;

            Effects effect = Effects.Null;

            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardSpellHealing CardSpellHealing = new CardSpellHealing(0, "Тестовое заклинание", 1);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.Damage(damage);
            CardSpellHealing.Treatment(CardWarriorTest);

            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }
        [TestMethod]
        public void AttackSpellTest()
        {

            //arrange
            int attack = 2;
            int health = 3;

            int damage = 2;

            int expected_health = health - damage;

            Effects effect = Effects.Null;

            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardSpellAttack CardSpellAttack = new CardSpellAttack(2, "Тестовое заклинание", damage);
            CardWarriorTest.DebugOff = true;

            //act
            CardSpellAttack.Damage(CardWarriorTest);

            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }
        [TestMethod]
        public void ImproveSpellTest()
        {

            //arrange
            int attack = 2;
            int health = 3;

            int improve = 2;

            int expected_health = health + improve;

            Effects effect = Effects.Null;

            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardSpellImprove CardSpellImprove = new CardSpellImprove(1, "Тестовое заклинание", 0, 2);
            CardWarriorTest.DebugOff = true;

            //act
            CardSpellImprove.Modification(CardWarriorTest);

            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }
    }
}
