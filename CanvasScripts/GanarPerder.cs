using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GanarPerder : MonoBehaviour
{
    //Grupos de menus
    public GameObject MenuFinal;
    public GameObject MenuSalir;
    public bool Pausa = false;//verificar el estado
    void Update()
    {
            MenuFinal.SetActive(true);
            Pausa = true;

            Time.timeScale = 0;//detiene el juego por completo
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
    }
}

