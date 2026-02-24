using UnityEngine;

/// <summary>
/// Ruota continuamente l'ostacolo attorno al proprio centro.
/// La velocità viene impostata da TrackGenerator con una variazione casuale.
/// </summary>
public class RotatingObstacle : MonoBehaviour
{
    [Tooltip("Gradi al secondo (positivo = antiorario, negativo = orario)")]
    public float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
