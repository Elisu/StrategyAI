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

    public override IPlayer LoadChampion()
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

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    Vector2 actualPos = new Vector2(hit.transform.position.x / gameMap.SizeMultiplier, hit.transform.position.z / gameMap.SizeMultiplier);
                    Vector2Int pos = Vector2Int.RoundToInt(actualPos); 
                    
                    IObject objectInPosition = gameMap[pos];

                    if (selectedUnit != null)
                    {
                        if (objectInPosition is Field)
                            if (selectedUnit is TroopBase troop)
                            {
                                player.SetAction(selectedUnit, new Move(pos, troop));
                                selectedUnit = null;
                            }    
                                

                        if (objectInPosition is Damageable toDamage)
                        {
                            if (toDamage.Side != player.Side)
                            {
                                player.SetAction(selectedUnit, new Attack(toDamage, selectedUnit));
                                selectedUnit = null;
                            }                                
                        }                            
                    }
                    else if (objectInPosition is Attacker unit)
                        if (unit.Side == player.Side)
                        {
                            selectedUnit = unit;
                            selectedUnit.Visual.HoverOverEnter();
                        }
                                         

                }
            }
        }

    }

    public void AddUnit(int type) => player.SetToBuy(type);
}
