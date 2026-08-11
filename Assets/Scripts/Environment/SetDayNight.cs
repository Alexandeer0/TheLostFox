using UnityEngine;

public class SetDayNight : MonoBehaviour
{
    public static SetDayNight Instance;

    [Header("Progress")]
    [Range(0f, 1f)]
    public float nightProgress;

    [Header("Directional Light")]
    public Light directionalLight;

    public float dayLightIntensity = 0.3f;
    public float nightLightIntensity = 0.05f;

    [Header("Ambient Light")]
    public Color dayAmbientColor = new Color(1f, 0.9f, 0.7f);
    public Color nightAmbientColor = new Color(0.1f, 0.15f, 0.25f);

    [Header("Fog")]
    public Color dayFogColor = new Color(0.7f, 0.8f, 0.7f);
    public Color nightFogColor = new Color(0.05f, 0.08f, 0.12f);

    public float dayFogDensity = 0.002f;
    public float nightFogDensity = 0.7f;

    [Header("Skybox")]
    public Material skyboxMaterial;

    public Color dayTint = new Color(0.5f, 0.6f, 1f);
    public Color nightTint = new Color(0.05f, 0.08f, 0.15f);

    public float dayExposure = 1.3f;
    public float nightExposure = 0.05f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        // Общий свет сцены
        RenderSettings.ambientLight =
            Color.Lerp(dayAmbientColor, nightAmbientColor, nightProgress);

        // Цвет тумана
        RenderSettings.fogColor =
            Color.Lerp(dayFogColor, nightFogColor, nightProgress);

        // Плотность тумана
        RenderSettings.fogDensity =
            Mathf.Lerp(dayFogDensity, nightFogDensity, nightProgress * nightProgress);

        // Цвет неба
        skyboxMaterial.SetColor("_SkyTint",
            Color.Lerp(dayTint, nightTint, nightProgress));

        // Яркость неба
        skyboxMaterial.SetFloat("_Exposure",
            Mathf.Lerp(dayExposure, nightExposure, nightProgress));

        // Directional light
        if (directionalLight != null)
        {
            directionalLight.intensity =
                Mathf.Lerp(dayLightIntensity, nightLightIntensity, nightProgress);
        }
    }
}