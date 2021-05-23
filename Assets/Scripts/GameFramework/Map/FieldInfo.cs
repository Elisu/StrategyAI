using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour
{
    [SerializeField]
    private SquareType square;
    [SerializeField]
    private Role side;

    public SquareType Square => square;
    public Role Side => side;

}