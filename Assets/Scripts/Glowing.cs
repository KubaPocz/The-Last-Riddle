using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    private static float pulseSpeed = 0.3f;
    private static float maxGlowIntensity = 0.35f;
    private static Color color = Color.white;
    private float localGlowTime;

    private Material[] materials;
    private int emissionID;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null && gameObject.CompareTag("Interactable"))
        {
            Material[] originalMaterials = renderer.sharedMaterials;
            materials = new Material[originalMaterials.Length];

            emissionID = Shader.PropertyToID("_EmissionColor");

            bool changed = false;

            for (int i = 0; i < originalMaterials.Length; i++)
            {
                if (!originalMaterials[i].name.Contains("Outline"))
                {
                    materials[i] = new Material(originalMaterials[i]);
                    materials[i].EnableKeyword("_EMISSION");
                    changed = true;
                }
                else
                {
                    materials[i] = originalMaterials[i];
                }
            }

            if (changed)
            {
                renderer.materials = materials;
            }
        }
    }

    void Update()
    {
        if (materials == null || !gameObject.CompareTag("Interactable")) return;

        localGlowTime = Mathf.PingPong(Time.time * pulseSpeed, maxGlowIntensity);
        Color emissionColor = color * localGlowTime;

        foreach (var mat in materials)
        {
            if (!mat.name.Contains("Outline"))
            {
                mat.SetColor(emissionID, emissionColor);
            }
        }
    }
}
