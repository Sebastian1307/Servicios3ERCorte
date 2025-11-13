using UnityEngine;

/// <summary>
/// Comportamiento simple para objeto que cae.
/// Tag "Deadly" si debe matar al jugador al colisionar con él.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FallingObject : MonoBehaviour
{
    public float lifeTime = 12f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // limpieza
    }

    // Opcional: detectar colisiones con el jugador para contarlos (atrapados/esquivados)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Si quieres que atrapar aumente contador, avisa a GameManager
            GameManager.Instance.ObjectCaught();
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            GameManager.Instance.ObjectMissed();
            Destroy(gameObject);
        }
    }
}
