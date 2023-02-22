using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    MeshRenderer renderer;

    Attacker attacker;

    Color originalColor;

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

        if (attacker != null &&  attacker.Health > 0 && attacker.Target != null && attacker.Target.Health > 0)
        {
            if (attacker.Target.Visual == null)
            {
                int k;
                k = 0;
            }

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
        if (renderer != null)
        {
            originalColor = renderer.material.color;
            renderer.material.color = Color.white;
        }
    }

    public void HoverOverExit()
    {
        if (renderer != null)
            renderer.material.color = originalColor;
    }    
}
