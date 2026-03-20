using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public Vector2Int direccion;
    // Ej: (1,0)=derecha, (-1,0)=izquierda, (0,1)=arriba, etc.

    public bool ocupado = false;

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public Vector2 GetWorldDirection()
    {
        return transform.right;
        // importante para rotación automática
    }
}