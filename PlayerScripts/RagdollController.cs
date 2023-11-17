using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    //Se añade al modelo del personaje
    Animator playerAnim;

    Rigidbody playerBody;
    Rigidbody[] playerBones;

    PController PLAYER;

    public List<HitMultiplier> hitStats;
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        playerBody = GetComponentInParent<Rigidbody>();
        playerBones = GetComponentsInChildren<Rigidbody>();

        PLAYER = GetComponentInParent<PController>();

        SetUp();
    }

    void Update()
    {

    }

    public void SetUp()
    {
        LayerMask layerOfHits = LayerMask.NameToLayer("Hitbox");

        foreach (Rigidbody bone in playerBones)
        {
            //colocar los huesos en collider continuo y lo añade a layer of hits
            bone.collisionDetectionMode = CollisionDetectionMode.Continuous;
            bone.gameObject.layer = layerOfHits;

            BodyPartHitCheck partToCheck = bone.gameObject.AddComponent<BodyPartHitCheck>();

            partToCheck.PLAYER = PLAYER;

            string bName = bone.gameObject.name.ToLower();

            foreach (HitMultiplier hit in hitStats)
            {//si contiene el nombre asigando en el nombre del hueso
                if (bName.Contains(hit.boneName))
                {//hace la multplicacion de daño
                    partToCheck.Multiplier = hit.multiplyBy;
                    partToCheck.BodyName = hit.boneName;
                    break;
                }
            }
        }
        Active(false);
    }
    public void Active(bool state)
    {
        foreach (Rigidbody bone in playerBones)
        {
            Collider c = bone.GetComponent<Collider>();

            if (bone.useGravity != state)
            {
                c.isTrigger = !state;
                bone.isKinematic = !state;

                // Ajusta el modo de detección de colisiones
                if (state)
                {
                    bone.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }
                else
                {
                    bone.collisionDetectionMode = CollisionDetectionMode.Discrete;
                }

                bone.useGravity = state;
                bone.velocity = playerBody.velocity;
            }
        }

        playerAnim.enabled = !state;
        playerBody.useGravity = !state;
        playerBody.detectCollisions = !state;
        playerBody.isKinematic = state;

        // Llamada a la configuración del modo de detección de colisiones del cuerpo principal (playerBody)
        if (state)
        {
            playerBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else
        {
            playerBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

}

[System.Serializable]
public class HitMultiplier
{
    public String boneName = "Head";
    public float multiplyBy = 1;
}