using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;

namespace FaplesNet
{
    public class UserLogin
    {
        [BsonId]
        public Guid id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
    }

    public class Character
    {
        // Base Information
        public string Name { get; set; }
        public int Class { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public long Experience { get; set; }

        // Attributes
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Luck { get; set; }

        // Statistics
        public int WeaponAttack { get; set; }
        public int Defense { get; set; }
        public int Accuracy { get; set; }
        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int MagicAccuracy { get; set; }
        public int Evasiveness { get; set; }
        public int Speed { get; set; }
        public int Jump { get; set; }
    }


    public class CharacterMap
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public object id { get; set; }
        public string Name { get; set; }
    }

    public enum eClass
    {
        Beginner = 0,
        Warrior = 1,
        Mage = 2,
        Rogue = 3,
        Swordsman = 11,
        Bulkwark = 12,
        Spearman = 13,
        ElementMage = 21,
        SoulMage = 22,
        Enchanter = 23,
        Bandit = 31,
        Renegade = 32,
        Ranger = 33
    }
}
