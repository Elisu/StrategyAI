using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    public Transform spawn;
    public GameObject unit;

    public Role role;
    float delay = 300;

    protected Army troops;

    // Start is called before the first frame update
    void Start()
    {
        troops = MasterScript.GetArmy(role);
        troops.Add(new Troop<Swordsmen>(10, role, unit));
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        delay -= 1;

        if (delay <= 0)
            FindAction();
    }


    protected virtual void FindAction()
    { 

    }
}
