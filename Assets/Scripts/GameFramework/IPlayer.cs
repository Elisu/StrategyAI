using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common base class for AIPlayer and HumanPlayer
/// </summary>
public abstract class IPlayer
{
    /// <summary>
    /// The side the player is on - defender/attacker
    /// </summary>
    public Role role;

    /// <summary>
    /// Represents all the troops, buildings and towers of the player
    /// </summary>
    protected internal Army OwnArmy { get; internal set; }

    internal void Start(Instance game, Role role)
    {
        this.role = role;
        OwnArmy = new Army(role);

        //OwnArmy.Add(new Troop<Swordsmen>(50, role, game));
        //OwnArmy.Add(new Troop<Cavalry>(10, role, game));
    }

    protected internal abstract Tuple<Attacker, IAction> GetActions();

    /// <summary>
    /// Called at the end of a run
    /// </summary>
    protected internal abstract void RunOver(GameStats stats);


    protected internal abstract int PickToBuy();
}
