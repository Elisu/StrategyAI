using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayerController : MonoBehaviour
{
    public abstract IPlayer GetPlayer();

    public virtual IPlayer Load(string championFile) => GetPlayer();
}
