using UnityEngine;

/// <summary>
/// Control lateral simple usando Rigidbody. Detecta colisi�n mortal con tag "Deadly".
/// Asume que el objeto ya tiene Rigidbody (no Kinematic).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f; // velocidad lateral
    private Rigidbody rb;
    private Vector3 moveVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Entrada horizontal simple: A/D, flechas o joystick horizontal por defecto
        float h = Input.GetAxis("Horizontal");
        moveVelocity = new Vector3(h * moveSpeed, rb.linearVelocity.y, 0f);
    }

    void FixedUpdate()
    {
        // Mantener la f�sica con Rigidbody
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, 0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con algo mortal, notificar al GameManager
        if (collision.collider.CompareTag("Deadly"))
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
