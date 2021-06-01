using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseModifiersMatrix
{
    public static float GetDefense(Type of, Type against)
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
                            return 0.8f;
                        case nameof(Catapult):
                            return 0.5f;
                        case nameof(Swordsmen):
                            return 0.9f;
                        default:
                            return 0.9f;
                    }
                }

            case nameof(Cavalry):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.95f;
                        case nameof(Cavalry):
                            return 0.8f;
                        case nameof(Catapult):
                            return 0.5f;
                        case nameof(Swordsmen):
                            return 0.9f;
                        default:
                            return 0.9f;
                    }
                }

            case nameof(Catapult):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.95f;
                        case nameof(Cavalry):
                            return 0.8f;
                        case nameof(Catapult):
                            return 0.5f;
                        case nameof(Swordsmen):
                            return 0.9f;
                        default:
                            return 0.9f;
                    }
                }

            case nameof(Swordsmen):
                {
                    switch (against.Name)
                    {
                        case nameof(Archers):
                            return 0.95f;
                        case nameof(Cavalry):
                            return 0.8f;
                        case nameof(Catapult):
                            return 0.5f;
                        case nameof(Swordsmen):
                            return 0.9f;
                        default:
                            return 0.9f;
                    }
                }
            default:
                return 1f;

        }
    }


}
