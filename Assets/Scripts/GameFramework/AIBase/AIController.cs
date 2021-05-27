//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AIController : IController<AIPlayer>
//{
//    public virtual int RunsPerGenerations => runsPerGen;
//    float delay = 10;

//    protected int runsPerGen = 1;
//    private bool scheduleInProgress = false;
//    private bool running;

//    private void Start()
//    {
        
//    }

//    public override void Activate(AIPlayer player, Instance game)
//    {
//        this.player = player;
//        //player.Start(game, );
//    }

//    protected void FixedUpdate()
//    {
//        if (!running)
//            return;

//        delay -= 1;

//        if (delay <= 0 || !scheduleInProgress)
//        {
//            StartCoroutine(ScheduleFind());
//            delay = 100;
//        }

//    }

//    private IEnumerator ScheduleFind()
//    {
//        scheduleInProgress = true;
//        for (int i = 0; i < OwnArmy.Count; i++)
//        {
//            player.FindAction(OwnArmy[i]);
//            yield return new WaitForSeconds(1);
//        }

//        scheduleInProgress = false;
//    }
//}
