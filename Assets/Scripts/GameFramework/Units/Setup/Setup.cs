using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Setup<T> : MonoBehaviour where T: Unit
{
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int range;
    [SerializeField]
    public GameObject unitPrefab;

    private static Setup<T> instance;
    
    public static int Health { get => instance.health; }
    public static float Speed { get => instance.speed; }
    public static int Defense { get => instance.defense; }
    public static int Damage { get => instance.damage; }
    public static int Range { get => instance.range; }
    public static GameObject UnitPrefab { get => instance.unitPrefab; }

    public Setup()
    {
        instance = this;
    }

}

