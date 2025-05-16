using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForJohnny;

namespace CardGameTest
{
    [TestClass]
    public class CardsWarriorsTest
    {
        [TestMethod]
        public void AttackTest()
        {
            
            //arrange
            int expected_attack = 2;
            int health = 3;

            Effects effect = Effects.Null;

            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", expected_attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            int actual_attack = CardWarriorTest.AttackPoints;

            //assert
            Assert.AreEqual(expected_attack, actual_attack);
        }

        [TestMethod]
        public void DamageTest()
        {
            //arrange
            int attack = 2;
            int health = 3;

            Effects effect = Effects.Null;

            int damage = 2;

            int expected_health = health - attack;


            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.Damage(damage);
            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }

        [TestMethod]
        public void DeathTest()
        {
            //arrange
            int attack = 2;
            int health = 3;

            Effects effect = Effects.Null;

            int damage = 3;

            bool expected_status = false;

            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.Damage(damage);
            bool actual_status = CardWarriorTest.GameStatus;

            //assert
            Assert.AreEqual(expected_status, actual_status);
        }

        [TestMethod]
        public void EffectHardnessTest()
        {
            //arrange
            int attack = 2;
            int health = 3;

            Effects effect = Effects.Hardness;

            int damage = 2;

            int expected_health = health - attack + 1;

           
            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.Damage(damage);
            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }

        [TestMethod]
        public void EffectFiniteTest()
        {
            
            //arrange
            int attack = 2;
            int health = 4;

            Effects effect = Effects.Finite;

            int damage = 2;

            int expected_health = health - attack - 1;


            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.AttackStatus();
            CardWarriorTest.Damage(damage);
            int actual_health = CardWarriorTest.Health;

            //assert
            Assert.AreEqual(expected_health, actual_health);
        }
        [TestMethod]
        public void EffectUpgradableTest()
        {

            //arrange
            int attack = 2;
            int health = 4;

            Effects effect = Effects.Upgradable;

            int expected_attack = attack + 2;


            CardWarrior CardWarriorTest = new CardWarrior(1, "Тестовый Воин", attack, health, effect);
            CardWarriorTest.DebugOff = true;

            //act
            CardWarriorTest.AttackStatus();
            CardWarriorTest.AttackStatus();

            int actual_attack = CardWarriorTest.AttackPoints;

            //assert
            Assert.AreEqual(expected_attack, actual_attack);
        }

    }
}
