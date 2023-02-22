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
    private int reloadRate;
    [SerializeField]
    private int range;
    [SerializeField]
    private int price;
    [SerializeField]
    private int bundleCount;
    [SerializeField]
    private VisualController unitPrefab;

    private static Setup<T> instance;
    
    public static int Health { get => instance.health; }
    public static float Speed { get => instance.speed; }
    public static int Damage { get => instance.damage; }
    public static int ReloadRate { get => instance.reloadRate; }
    public static int Range { get => instance.range; }
    public static int Price { get => instance.price; }
    public static int BundleCount { get => instance.bundleCount; }
    public static VisualController UnitPrefab { get => instance.unitPrefab; }


    public Setup()
    {
        instance = this;
    }

}


