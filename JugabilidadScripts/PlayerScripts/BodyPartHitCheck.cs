using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHitCheck : MonoBehaviour
{
    [HideInInspector]//Oculta el añadir para el diseño
    public PController PLAYER;

    public string BodyName;
    public float Multiplier;
    public float LastDamage;

    public void TakeHit(float damage)
    {
        LastDamage = damage * Multiplier;

        this.PLAYER.TakeDamage(LastDamage);
    }
}
