using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UnitFinder : MonoBehaviour
{
    public static IReadOnlyList<UnitInfo> UnitStats { get; private set; }

    public static int LowestPriceIndex { get; private set; } = 0;
    public static int HighestPriceIndex { get; private set; }
    public static int LowestSpeed { get; private set; }
    public static int HighestSpeed { get; private set; }
    public static int LowestHealth { get; private set; }
    public static int HighestHealth { get; private set; }
    public static int LowestDamage { get; private set; }
    public static int HighestDamage { get; private set; }

    public struct UnitInfo
    {
        public int Price { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public int Range { get; private set; }
        public float Speed { get; private set; }
        public Type UnitType { get; private set; }

        public UnitInfo(int price, int health, int damage, int range, float speed, Type type)
        {
            Price = price;
            Health = health;
            Damage = damage;
            Range = range;
            Speed = speed;
            UnitType = type;
        }
    }
        

    private void Awake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
                                                   .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                                                               t.BaseType.GetGenericTypeDefinition() == typeof(Setup<>));

        List<UnitInfo> unitStats = new List<UnitInfo>();

        foreach (Type unit in types)
        {
            if (unit.Equals(typeof(BasicTowerSetup)))
                continue;

            int priceValue = GetValue<int>(unit, "Price");
            int healthValue = GetValue<int>(unit, "Health");
            int damageValue = GetValue<int>(unit, "Damage");
            int rangeValue = GetValue<int>(unit, "Range");
            float speedValue = GetValue<float>(unit, "Speed");
            Type type = unit.BaseType.GetGenericArguments()[0];

            unitStats.Add(new UnitInfo(priceValue, healthValue, damageValue, rangeValue, speedValue, type));

            if (unitStats[LowestPriceIndex].Price > priceValue)
                LowestPriceIndex = unitStats.Count - 1;
        }

        UnitStats = unitStats.AsReadOnly();

    }

    private T GetValue<T>(Type unit, string fieldName) where T : notnull
    {
        PropertyInfo field = unit.BaseType.GetProperty(fieldName);
        return (T)field.GetValue(null);
    }

    public static int PickOnBudget(int budget)
    {
        int selected = -1;
        for (int i = 0; i < UnitStats.Count; i++)
            if (UnitStats[i].Price <= budget && UnitStats[i].Price > UnitStats[selected].Price)
                selected = i;

        return selected;
    }

}
