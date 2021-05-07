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
        OwnArmy.Add(new Troop<Swordsmen>(50, role, game));
    }

    public abstract Tuple<Attacker, IAction> GetActions();

}
