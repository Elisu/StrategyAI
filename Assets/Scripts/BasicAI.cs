using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public Transform spawn;
    public GameObject unit;

    public Role role;
    float delay = 300;

    Army troops;

    // Start is called before the first frame update
    void Start()
    {
        troops = MasterScript.GetArmy(role);
        troops.Add(new Troop<Swordsmen>(10, role, unit));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        delay -= 1;

        if (delay <= 0)
            FindAction();
    }


    void FindAction()
    {
        ITroop troop = troops.GetTroopFree();

        if (troop != null)
        {
            if (!MacroActions.AttackInRange(troop))
            {
                MacroActions.AttackClosest(troop);
            }
        }

    }
}
