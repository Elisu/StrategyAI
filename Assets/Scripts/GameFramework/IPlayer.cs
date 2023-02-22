using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Common base class for AIPlayer and HumanPlayer
/// </summary>
[DataContract]
public abstract class IPlayer
{
    /// <summary>
    /// The side the player is on - defender/attacker
    /// </summary>
    [DataMember]
    public Role Side { get; private set; }

    /// <summary>
    /// Represents all the troops, buildings and towers of the player
    /// </summary>
    protected internal GameInfo Info { get; private set; }

    internal void Start(GameInfo game, Role side)
    {
        this.Side = side;
        this.Info = game;
    }

    protected internal abstract Tuple<Attacker, IAction> GetActions();


    /// <summary>
    /// UnitFinder provides list of charachteristics of available units - pick one based on those charachteristics
    /// </summary>
    /// <returns>index of chosen unit</returns>
    protected internal abstract int PickToBuy();
}
