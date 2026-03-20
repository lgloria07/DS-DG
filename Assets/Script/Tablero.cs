using UnityEngine;

public class Tablero : MonoBehaviour
{
    public int width = 10;
    public int height = 6;

    public float cellWidth = 1f;
    public float cellHeight = 1f;

    public enum TipoCasilla
    {
        Caminable,
        Bloqueada,
        Combate
    }

    public TipoCasilla[,] grid;

    void Start()
    {
        grid = new TipoCasilla[width, height];

        // Por defecto todo caminable
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = TipoCasilla.Caminable;
            }
        }

        ConfigurarCasillas();
    }

    void ConfigurarCasillas()
    {
        // 🔴 ZONA ROJA (enemigo - NO caminable)
        grid[3, 2] = TipoCasilla.Combate;
        grid[4, 2] = TipoCasilla.Combate;
        grid[3, 3] = TipoCasilla.Combate;
        grid[4, 3] = TipoCasilla.Combate;

        // ⚪ BARRA GRIS ARRIBA (bloqueada)
        for (int x = 3; x <= 6; x++)
        {
            grid[x, 4] = TipoCasilla.Bloqueada;
        }

        // ⚪ BARRA GRIS ABAJO (bloqueada)
        for (int x = 3; x <= 6; x++)
        {
            grid[x, 1] = TipoCasilla.Bloqueada;
        }

        // 🟩 CUADRO DE ACCESO (el gris donde sí puedes pararte)
        grid[6, 2] = TipoCasilla.Caminable;
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(
            x * cellWidth + cellWidth / 2,
            y * cellHeight - 0.28f,
            0
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = GridToWorld(x, y);
                Gizmos.DrawWireCube(pos, new Vector3(cellWidth, cellHeight, 1));
            }
        }
    }
}