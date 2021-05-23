using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Setup<T> : MonoBehaviour where T: Unit
{
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int range;
    [SerializeField]
    private int price;
    [SerializeField]
    private GameObject unitPrefab;

    private static Setup<T> instance;
    
    public static int Health { get => instance.health; }
    public static float Speed { get => instance.speed; }
    public static int Damage { get => instance.damage; }
    public static int Range { get => instance.range; }
    public static int Price { get => instance.price; }
    public static GameObject UnitPrefab { get => instance.unitPrefab; }


    public Setup()
    {
        instance = this;
    }

}


