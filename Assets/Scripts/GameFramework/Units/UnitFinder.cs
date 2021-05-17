using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UnitFinder : MonoBehaviour
{
    public static List<UnitInfo> unitStats = new List<UnitInfo>();
    internal static List<Type> unitTypes = new List<Type>();

    public struct UnitInfo
    {
        public int Price { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public float Speed { get; private set; }

        public UnitInfo(int price, int health, int damage, float speed)
        {
            Price = price;
            Health = health;
            Damage = damage;
            Speed = speed;
        }
    }
        

    private void Awake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
                                                   .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                                                               t.BaseType.GetGenericTypeDefinition() == typeof(Setup<>));
        
        foreach (Type unit in types)
        {
            int priceValue = GetValue<int>(unit, "Price");
            int healthValue = GetValue<int>(unit, "Health");
            int damageValue = GetValue<int>(unit, "Damage");
            float speedValue = GetValue<float>(unit, "Speed");

            unitStats.Add(new UnitInfo(priceValue, healthValue, damageValue, speedValue));
            unitTypes.Add(unit.BaseType.GetGenericArguments()[0]);
        }

    }

    private T GetValue<T>(Type unit, string fieldName)
    {
        PropertyInfo field = unit.BaseType.GetProperty(fieldName);
        return (T)field.GetValue(null);
    }

    public static int PickOnBudget(int budget)
    {
        //To Do
        return 2;
    }
}
