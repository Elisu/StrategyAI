using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public Role role;
    float delay = 5000;

    List<ITroop> troops = new List<ITroop>();

    // Start is called before the first frame update
    void Start()
    {
        troops.Add(new Troop<Swordsmen>(10, role));
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
        
    }
}
