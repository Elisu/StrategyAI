using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class HumanPlayerController : IPlayerController
{
    HumanPlayer player = new HumanPlayer();

    Attacker selectedUnit;

    IObjectMap gameMap;

    Text moneyText;

    public override IPlayer GetPlayer()
    {
        return player;
    }

    public void SetMap(IObjectMap map, Text text)
    {
        gameMap = map;
        moneyText = text;
    }

    void Update()
    {
        moneyText.text = player.Money.ToString();

        //On mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                //If mouse pointing at some collider
                if (hit.transform != null)
                {
                    //We get the position mouse is pointing at on the map 
                    Vector2 actualPos = new Vector2(hit.transform.position.x / gameMap.SizeMultiplier, hit.transform.position.z / gameMap.SizeMultiplier);
                    Vector2Int pos = Vector2Int.RoundToInt(actualPos);

                    //Get the object on the map in given position
                    IObject objectInPosition = gameMap[pos];

                    //If we have previously selected our unit
                    if (selectedUnit != null)
                    {
                        //If object in position is just an empty field and our selected unit is troop -> we move
                        if (objectInPosition is Field)
                            if (selectedUnit is TroopBase troop)
                                player.SetAction(selectedUnit, new Move(pos, troop));

                        //If object on map is damageable and is on the enemy side -> attack
                        if (objectInPosition is Damageable toDamage)
                        {
                            if (toDamage.Side != player.Side)
                                player.SetAction(selectedUnit, new Attack(toDamage, selectedUnit));
                        }
                    }

                    //If we don't have anything selected and object on map is our unit -> select it
                    if (objectInPosition is Attacker unit && unit.Side == player.Side)
                    {
                        if (selectedUnit != null)
                            selectedUnit.Visual.HoverOverExit();

                        selectedUnit = unit;
                        selectedUnit.Visual.HoverOverEnter();
                    }


                }
            }
        }

        //Deselect
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnit != null)
                selectedUnit.Visual.HoverOverExit();

            selectedUnit = null;
        }

    }        

    public void AddUnit(int type) => player.SetToBuy(type);

}
