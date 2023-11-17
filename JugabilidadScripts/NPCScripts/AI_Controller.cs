using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Controller
 : MonoBehaviour
{
    //Caracteristicas Navegacion
    [Header("Configuración de navegacion")]
    private string nombre;
    public NavMeshAgent agent;
    public Transform Objetivo;
    public LayerMask whatIsGround, whatIsPlayer;
    public float Velocidad, sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    //Distancia de movimiento aleatorio
    public float walkPointRange;

    //Animaciones
    [Header("Configuración de animacion")]
    public Animator animator;
    public string walkAnimationParameter = "IsWalking";
    public string attackAnimationParameter = "IsAttacking";

    //Atacar
    [Header("Configuración de ataque")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectile;

    //Patrullaje
    [Header("Configuración de patrullaje")]
    private Vector3 walkPoint;
    private bool walkPointSet;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Velocidad;
        //Dependiendo del NPC
        GameObject NPCName = this.gameObject;
        //Objetivo al azar Para los realistas
        int ObjetivoRandom = Random.Range(0, 2);
        nombre = NPCName.name;
        if (nombre == "Patriota 2R")//Si es un Patriota
        {
            Objetivo = GameObject.Find("RealistaC 1").transform;
        }
        else
        {
            Objetivo = GameObject.Find("Patriota 1R").transform;
        }
    }
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)//Si no esta en alcance ni en ataque
        {
            SetAnimationParameter(walkAnimationParameter, true);
            SetAnimationParameter(attackAnimationParameter, false);
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)//si esta al alcance y no al ataque
        {
            SetAnimationParameter(walkAnimationParameter, true);
            SetAnimationParameter(attackAnimationParameter, false);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)//si esta en el alcance y en el ataque
        {
            SetAnimationParameter(walkAnimationParameter, false);
            SetAnimationParameter(attackAnimationParameter, true);
            AttackPlayer();
        }
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        if (nombre == "Patriota 2R")
            if (Objetivo.name == "Killed")//Si muere//si exite otro realista
                Objetivo = GameObject.Find("RealistaC 1").transform;
    }
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    void ChasePlayer()
    {
        agent.SetDestination(Objetivo.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(Objetivo);//Se ataca el objetivo vivo

        if (!alreadyAttacked)
        {
            // Attack code here
            float alturaRandom = Random.Range(1f, 2f);

            // Posición de inicio de la bala para evitar chocar con el hitbox del mismo
            Vector3 spawnPosition = transform.position + transform.forward * 3f + transform.up * alturaRandom;

            // Crea el proyectil en la posición ajustada
            Rigidbody rb = Instantiate(projectile, spawnPosition, Quaternion.identity).GetComponent<Rigidbody>();

            // Aplica fuerzas al proyectil
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 1f, ForceMode.Impulse);

            // End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    void SetAnimationParameter(string parameterName, bool value)
    {
        if (animator != null)
            animator.SetBool(parameterName, value);
    }
    void OnDrawGizmosSelected()
    {
        //Ver distancia de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //ver distancia de deteccion
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
