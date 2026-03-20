using UnityEngine;
using System.Collections;
using static Tablero;

public class PlayerMovement : MonoBehaviour
{
    public PersonajeData personajeActual;
    public Tablero tablero;

    public Transform visual;

    Vector2Int posicion = new Vector2Int(0, 0);

    SpriteRenderer sr;

    void Start()
    {
        StartCoroutine(Inicializar());
    }

    IEnumerator Inicializar()
    {
        // ⏳ Esperar a que exista al menos un tablero
        yield return new WaitUntil(() => FindObjectOfType<Tablero>() != null);

        tablero = FindObjectOfType<Tablero>();

        // ⏳ Esperar a que el grid esté listo
        yield return new WaitUntil(() => tablero.grid != null);

        // 📍 Posicionar jugador
        transform.position = tablero.GridToWorld(posicion.x, posicion.y);

        if (visual != null)
            sr = visual.GetComponent<SpriteRenderer>();

        AplicarPersonaje();

        Debug.Log("✅ Player inicializado en: " + tablero.transform.position);
    }

    void AplicarPersonaje()
    {
        if (sr == null || personajeActual == null)
        {
            Debug.LogWarning("⚠️ Falta SpriteRenderer o PersonajeData");
            return;
        }

        sr.sprite = personajeActual.sprite;

        AjustarEscala();
        AjustarVisual();
    }

    void AjustarEscala()
    {
        if (sr == null) return;

        float alturaDeseada = tablero.cellHeight * 1.1f;
        float escala = alturaDeseada / sr.bounds.size.y;

        visual.localScale = new Vector3(escala, escala, 1);
    }

    void AjustarVisual()
    {
        if (sr == null) return;

        float altura = sr.bounds.size.y;
        visual.localPosition = new Vector3(0, altura / 2f, 0);
    }

    void Update()
    {
        if (tablero == null || tablero.grid == null) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Mover(Vector2Int.right);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Mover(Vector2Int.left);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Mover(Vector2Int.up);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Mover(Vector2Int.down);
    }

    void Mover(Vector2Int direccion)
    {
        Vector2Int nuevaPos = posicion + direccion;

        // 🔄 CAMBIO DE TABLERO
        if (nuevaPos.x < 0 || nuevaPos.x >= tablero.width ||
            nuevaPos.y < 0 || nuevaPos.y >= tablero.height)
        {
            Tablero nuevoTablero = BuscarTableroEnDireccion(direccion);

            if (nuevoTablero != null)
            {
                tablero = nuevoTablero;

                // Ajustar posición al lado contrario
                if (direccion == Vector2Int.right)
                    posicion = new Vector2Int(0, posicion.y);

                else if (direccion == Vector2Int.left)
                    posicion = new Vector2Int(tablero.width - 1, posicion.y);

                else if (direccion == Vector2Int.up)
                    posicion = new Vector2Int(posicion.x, 0);

                else if (direccion == Vector2Int.down)
                    posicion = new Vector2Int(posicion.x, tablero.height - 1);

                transform.position = tablero.GridToWorld(posicion.x, posicion.y);

                Debug.Log("➡️ Cambio de tablero a: " + tablero.transform.position);
            }

            return;
        }

        // 🚫 Validar casilla
        TipoCasilla tipo = tablero.grid[nuevaPos.x, nuevaPos.y];

        if (tipo == TipoCasilla.Bloqueada || tipo == TipoCasilla.Combate)
            return;

        // ✅ Movimiento normal
        posicion = nuevaPos;
        transform.position = tablero.GridToWorld(posicion.x, posicion.y);
    }

    Tablero BuscarTableroEnDireccion(Vector2Int direccion)
    {
        float distanciaX = tablero.width * tablero.cellWidth;
        float distanciaY = tablero.height * tablero.cellHeight;

        Vector3 offset = new Vector3(
            direccion.x * distanciaX,
            direccion.y * distanciaY,
            0
        );

        Vector3 posicionBusqueda = tablero.transform.position + offset;

        Tablero[] todos = FindObjectsOfType<Tablero>();

        foreach (Tablero t in todos)
        {
            if (Vector3.Distance(t.transform.position, posicionBusqueda) < 0.1f)
            {
                return t;
            }
        }

        return null;
    }
}