using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    MeshRenderer renderer;

    Attacker attacker;

    private void Awake()
    {
        if (lr != null)
            lr = Instantiate(lr);

        renderer = GetComponent<MeshRenderer>();
    }

    internal void Set(Attacker unit)
    {
        attacker = unit;

        if (attacker.Side == Role.Attacker)
        {
            renderer.material.color = Color.red;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
        }
        else
        {
            renderer.material.color = Color.black;
            lr.startColor = Color.black;
            lr.endColor = Color.black;
        }

    }

    private void Update()
    {
        if (lr == null)
            return;

        if (attacker.Target != null && attacker != null && attacker.Target.Health > 0)
        {
            lr.enabled = true;
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, attacker.Target.Visual.transform.position);
        }           
        else
            lr.enabled = false;
    }

    private void OnDestroy()
    {
        if (lr != null)
            Destroy(lr);
    }

    public void HoverOverEnter()
    {
        renderer.material.SetFloat("_OutlineWidth", 0.4f);
    }

    public void HoverOverExit()
    {
        renderer.material.SetFloat("_OutlineWidth", 0f);


    }    
}
