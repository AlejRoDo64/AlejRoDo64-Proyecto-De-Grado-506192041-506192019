using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBodyPartHitCheck : MonoBehaviour
{
    [HideInInspector]//Oculta el añadir para el diseño
    public NPCController NPC;

    public string BodyName;
    public float Multiplier;
    public float LastDamage;
    public void TakeHit(float damage)
    {
        LastDamage = damage * Multiplier;

        this.NPC.TakeDamage(LastDamage);

        Debug.Log(damage + " * " + Multiplier + " = " + LastDamage);
    }
}
