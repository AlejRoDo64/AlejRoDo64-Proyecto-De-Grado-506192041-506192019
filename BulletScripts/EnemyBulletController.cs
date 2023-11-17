using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    public float bulletPower = 10f;
    public float lifeTime = 4f;
    public float bulletDamage = 1f;
    public LayerMask hitboxMask;
    private Transform target;

    // Nueva función para establecer el objetivo
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        hitboxMask = LayerMask.GetMask("Hitbox");

        if (target != null)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Establece la velocidad de la bala en la dirección hacia el objetivo
            bulletRb.velocity = directionToTarget * bulletPower;
        }

        // Destruye la bala después de cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletPower * Time.fixedDeltaTime, hitboxMask))
        {
            GameObject targetObject = hit.collider.gameObject;

            BodyPartHitCheck playerBodyPart = targetObject.GetComponent<BodyPartHitCheck>();
            NPCBodyPartHitCheck NPCBodyPart = targetObject.GetComponent<NPCBodyPartHitCheck>();

            if (playerBodyPart != null)//Si dispara al jugador hace daño al jugador
            {
                Debug.Log("Disparo en " + playerBodyPart.BodyName);
                playerBodyPart.TakeHit(bulletDamage);
            }
            if (NPCBodyPart != null)//Si dispara al NPC hace daño al jugador
            {
                Debug.Log("Disparo en " + NPCBodyPart.BodyName);
                NPCBodyPart.TakeHit(bulletDamage);
            }

            Destroy(gameObject);
        }
    }
}
