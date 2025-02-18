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

            // Upewnij siê, ¿e Emission jest aktywne
            material.EnableKeyword("_EMISSION");

            // Pobieramy ID w³aœciwoœci EmissionColor
            emissionID = Shader.PropertyToID("_EmissionColor");
        }
    }

    void Update()
    {
        if (material != null)
        {
            // Tworzenie efektu pulsuj¹cej poœwiaty
            float glowIntensity = Mathf.PingPong(Time.time * pulseSpeed, maxGlowIntensity);
            Color emissionColor = color * glowIntensity;

            // Ustawienie tylko jasnoœci Emission, bez zmiany tekstury
            material.SetColor(emissionID, emissionColor);
        }
    }
}
