using UnityEngine;

[CreateAssetMenu(menuName = "Personaje")]
public class PersonajeData : ScriptableObject
{
    public string nombre;
    public Sprite sprite;

    public int vida;
    public int ataque;

    // después puedes agregar habilidades
}