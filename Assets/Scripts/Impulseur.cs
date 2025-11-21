using UnityEngine;

public class Impulseur : MonoBehaviour
{
    public float mass = 3.0f;
    public Vector3 impact = Vector3.zero;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // appeler la fonction pour ajouter de la force
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    void Update()
    {
        // application de la force d'impact
        //if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
        // perte de vitesse à chaque fois
        //impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
}
