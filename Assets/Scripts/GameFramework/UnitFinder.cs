using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UnitFinder : MonoBehaviour
{
    public static List<int> prices = new List<int>();
    internal static List<Type> unitTypes = new List<Type>();

    private void Awake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
                                                   .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                                                               t.BaseType.GetGenericTypeDefinition() == typeof(Setup<>));
        
        foreach (Type unit in types)
        {
            PropertyInfo price = unit.BaseType.GetProperty("Price");
            int priceValue = (int)price.GetValue(null);
            prices.Add(priceValue);
            unitTypes.Add(unit.BaseType.GetGenericArguments()[0]);
        }

    }

    public static int PickOnBudget(int budget)
    {
        return 2;
    }
}
