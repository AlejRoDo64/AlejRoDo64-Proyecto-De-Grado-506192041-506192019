using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCanvasEscenarioControl : MonoBehaviour
{
    public GameObject[] Scenaryobjects;//Grupodeobjetosdel nivel
    public bool Dialog;
    void Start()
    {  
        Dialog = true;
        Escenario(!Dialog);
    }
    void Update()
    {
        if (!Dialog)
        {
            Escenario(!Dialog);
        }
    }
    public void Resumir()
    {
        Dialog = false;
    }
    public void Escenario(bool state)
    {
        //contrario a la presentasion de inicio es decir se inicia la historia el nivel esta apagado
        //al terminar la hisoria es decir inicia los objetos del nivel.
        int objestosInicio = Scenaryobjects.Length;
        for (int i = 0; i < objestosInicio; i++)
        {
            Scenaryobjects[i].SetActive(state);
        }
        gameObject.SetActive(!state);
        Scenaryobjects[objestosInicio - 1].SetActive(!state);


    }
}
