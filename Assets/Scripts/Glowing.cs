using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private float pulseSpeed = 0.2f;
    private float maxGlowIntensity = 0.25f;
    private Color color = Color.white;
    private Material material;
    private int emissionID;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            material = renderer.material;

            // Upewnij si�, �e Emission jest aktywne
            material.EnableKeyword("_EMISSION");

            // Pobieramy ID w�a�ciwo�ci EmissionColor
            emissionID = Shader.PropertyToID("_EmissionColor");
        }
    }

    void Update()
    {
        if (material != null)
        {
            // Tworzenie efektu pulsuj�cej po�wiaty
            float glowIntensity = Mathf.PingPong(Time.time * pulseSpeed, maxGlowIntensity);
            Color emissionColor = color * glowIntensity;

            // Ustawienie tylko jasno�ci Emission, bez zmiany tekstury
            material.SetColor(emissionID, emissionColor);
        }
    }
}
