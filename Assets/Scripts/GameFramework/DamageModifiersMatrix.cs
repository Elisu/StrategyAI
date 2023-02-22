using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifiersMatrix
{
    /// <summary>
    /// How much of damage is kept when type against attacking type of
    /// </summary>
    /// <param name="of"></param>
    /// <param name="against"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static float DamageModifier(Type against, Type of, int distance = 1)
    {
        switch (of.Name)
        {
            case nameof(Archers):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.95f;
                        case nameof(Cavalry):
                            if (distance > 1)
                                return 0.8f;
                            else
                                return 5f;
                        case nameof(Catapult):
                            return 0.5f;
                        case nameof(Swordsmen):
                            if (distance > 1)
                                return 0.85f;
                            else
                                return 0.6f;
                        default:
                            return 0.8f;
                    }
                }

            case nameof(Cavalry):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.95f;
                        case nameof(Cavalry):
                            return 0.9f;
                        case nameof(Catapult):
                            return 0.8f;
                        case nameof(Swordsmen):
                            return 0.8f;
                        default:
                            return 0.9f;
                    }
                }

            case nameof(Catapult):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.9f;
                        case nameof(Cavalry):
                            return 0.7f;
                        case nameof(Catapult):
                            return 0.7f;
                        case nameof(Swordsmen):
                            return 0.7f;
                        default:
                            return 1f;
                    }
                }

            case nameof(Swordsmen):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.90f;
                        case nameof(Cavalry):
                            return 0.7f;
                        case nameof(Catapult):
                            return 0.95f;
                        case nameof(Swordsmen):
                            return 0.9f;
                        default:
                            return 0.8f;
                    }
                }

            case nameof(BasicTower):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.90f;
                        case nameof(Cavalry):
                            return 0.7f;
                        case nameof(Catapult):
                            return 0.6f;
                        case nameof(Swordsmen):
                            return 0.8f;
                        default:
                            return 0.8f;
                    }
                }

            default:
                return 1f;

        }
    }


}
