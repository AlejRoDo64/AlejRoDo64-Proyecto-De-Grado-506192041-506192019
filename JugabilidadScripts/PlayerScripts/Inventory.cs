using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{

    public GameObject CanvasGanar;
    public Text Coleccionados;
    public int amount = 0;
    public int TotalEnemies;
    public int Kills = 0;

    void Update()
    {
        Coleccionados.text = "Relatos recolectados: " + amount;
        if (amount == 4 || (Kills == TotalEnemies && amount == 4))
        {
            CanvasGanar.SetActive(true);
        }
    }
}
