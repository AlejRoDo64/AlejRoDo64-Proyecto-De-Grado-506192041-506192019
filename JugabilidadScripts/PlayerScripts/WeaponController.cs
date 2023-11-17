using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private PController Player;
    public Transform shootSpawn;
    public GameObject bulletPrefab;
    //Tiempo de disparo
    public float fireRate = 0.5f;  // Cadencia de disparo en segundos
    private float nextFireTime = 0f;
    //Cantidad Balas
    public int maxBullets = 7;  // Número máximo de balas
    private int currentBullets = 0;

    public Text bulletsText;  // Referencia al objeto Text

    void Start()
    {
        currentBullets = maxBullets;  // Comienza con el número máximo de balas
        UpdateBulletsText();  // Actualiza el texto inicialmente
        Player = GetComponent<PController>();
    }

    void Update()
    {
        // Linea que mostrara a donde apunta el personaje del shootSpawn
        Debug.DrawLine(shootSpawn.position, shootSpawn.forward * 25f, Color.blue);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.red);

        // Lanzar un rayo desde la cámara hasta el centro de la pantalla
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.green);

        //Donde golpe de la camara con algun objeto
        RaycastHit cameraHit;

        if (Physics.Raycast(ray, out cameraHit))
        {
            Vector3 shootDirection = cameraHit.point - shootSpawn.position;
            shootSpawn.rotation = Quaternion.LookRotation(shootDirection);

            // Comprobar si se puede disparar y aún tienes balas
            if (Input.GetMouseButtonDown(0) && Time.timeScale != 0 && Time.time >= nextFireTime && currentBullets > 0)
            {
                Shoot();
                // Actualizar el tiempo del próximo disparo
                nextFireTime = Time.time + 1f / fireRate;
                currentBullets--;  // Reducir el número de balas
                UpdateBulletsText();  // Actualizar el texto después de disparar
            }
            /*if(currentBullets == 0){
                Player.UnequipMusket();
                Player.EquipMusket();
            }*/
        }
    }
    public void Shoot()
    {
        Instantiate(bulletPrefab, shootSpawn.position, shootSpawn.rotation);
    }
    void UpdateBulletsText()
    {
        // Buscar el objeto de texto en el musket
        Text bulletsText = GetComponentInChildren<Text>();
        // Asegurarse de que la referencia al objeto Text no sea nula
        if (bulletsText != null)
        {
            // Actualizar el texto con la cantidad actual de balas
            bulletsText.text = "= " + currentBullets.ToString();
        }
        else
        {
            Debug.Log(bulletsText);
        }
    }
}
