using UnityEngine;
using System.Collections.Generic;

public class GeneradorMapa : MonoBehaviour
{
    public GameObject tableroPrefab;

    public int cantidad = 5;

    public float anchoChunk = 15f;
    public float altoChunk = 10f;

    List<Vector2> posicionesOcupadas = new List<Vector2>();

    void Start()
    {
        Tablero t = tableroPrefab.GetComponentInChildren<Tablero>();

        anchoChunk = t.width * t.cellWidth;
        altoChunk = t.height * t.cellHeight;

        Generar();
    }

    void Generar()
    {
        Vector2 posicionActual = Vector2.zero;

        for (int i = 0; i < cantidad; i++)
        {
            GameObject nuevo = Instantiate(tableroPrefab);

            nuevo.transform.position = posicionActual;

            posicionesOcupadas.Add(posicionActual);

            Vector2 direccion = DireccionAleatoria();

            Vector2 nuevaPos = posicionActual + new Vector2(
                direccion.x * anchoChunk,
                direccion.y * altoChunk
            );

            if (!posicionesOcupadas.Contains(nuevaPos))
            {
                posicionActual = nuevaPos;
            }
        }
    }

    Vector2 DireccionAleatoria()
    {
        int r = Random.Range(0, 4);

        switch (r)
        {
            case 0: return Vector2.right;
            case 1: return Vector2.left;
            case 2: return Vector2.up;
            default: return Vector2.down;
        }
    }
}