using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTroop :  MonoBehaviour //ITroop
{
    [SerializeField]
    private Transform player;

    [SerializeField] 
    private float speed;

    private void Start()
    {
        Speed = speed;
    }

    public bool TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage, int index, int count)
    {
        throw new System.NotImplementedException();
    }

    public Vector2 ActualPosition 
    { 
        get { return new Vector2(player.position.x/5, player.position.z/5); }
        set { player.position = new Vector3(value.x * 5, 0, value.y * 5); }
    }

    public float Speed { get; private set; }

    public int Damage => throw new System.NotImplementedException();

    public int Range => throw new System.NotImplementedException();

    public int Defense => throw new System.NotImplementedException();

    public int Health => throw new System.NotImplementedException();

    public Vector2Int Position { get => Vector2Int.RoundToInt(ActualPosition); }

    public Role Side => throw new System.NotImplementedException();

    public bool Passable { get => false; }

    public int Size => throw new System.NotImplementedException();

    public int Count => throw new System.NotImplementedException();

    public State CurrentState => throw new System.NotImplementedException();
}
