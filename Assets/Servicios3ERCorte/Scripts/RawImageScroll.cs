using UnityEngine;
using UnityEngine.UI;

public class RawImageScroll : MonoBehaviour
{
    public RawImage imagen;
    public float speed;


    // Update is called once per frame
    void Update()
    {
        if (imagen == null) return;

        // Obtener el rect actual
        Rect uvRect = imagen.uvRect;

        // Aumentar el desplazamiento horizontal (x = U)
        uvRect.y += speed;

        // Asignar el nuevo rect a la imagen
        imagen.uvRect = uvRect;
    }
}
