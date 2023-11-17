using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuJuego : MonoBehaviour
{
    //Grupos de menus
    public GameObject ObjetoMenuPausa;
    public GameObject MenuSalir;
    public bool Pausa = false;//verificar el estado
    void Start()
    {
        if (Pausa == false)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//boton para poner pausa
        {
            if (Pausa == false)//activar pausa si no esta en pausa
            {
                ObjetoMenuPausa.SetActive(true);
                Pausa = true;

                Time.timeScale = 0;//detiene el juego por completo
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                /* AudioSource[] sonidos = FindObjectsOfType<AudioSource>();//Apagar sonidos

                 foreach (AudioSource sonido in sonidos)
                 {
                     sonido.Pause();
                 }*/
            }
            else if (Pausa == true)
            {
                Resumir();
                Cursor.visible = false;
            }
        }
    }
    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        MenuSalir.SetActive(false);
        Pausa = false;

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        /*AudioSource[] sonidos = FindObjectsOfType<AudioSource>();//Encender sonidos

        foreach (AudioSource sonido in sonidos)
        {
            sonido.Play();
        }*/
    }
}
