using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerController : AITrainer
{
    HumanPlayer player = new HumanPlayer();

    Attacker selectedUnit;

    public override Type AIPlayerType => throw new NotImplementedException();

    public override void GenerationDone()
    {
        throw new NotImplementedException();
    }

    public override AIPlayer GetChampion()
    {
        throw new NotImplementedException();
    }

    protected override List<AIPlayer> CreatPopulation()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    //p   
                    //player. new Move(hit.transform.position.x, hit.transform.position.z, troop);
                    ////Scheduler.Attacker.Enqueue(move);
                }
            }
        }

    }
}
