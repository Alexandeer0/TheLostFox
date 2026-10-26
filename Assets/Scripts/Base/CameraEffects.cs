using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] CameraController playerCamera;

    [Range(0, 1)]
    public float effectStrength;

    Bloom bloom;
    Vignette vignette;
    DepthOfField depthOfField;
    ChromaticAberration chromaticAberration;
    FilmGrain filmGrain;

    void Awake()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out depthOfField);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out filmGrain);
    }

    void Update()
    {
        if (playerCamera.mode == 2 || playerCamera.mode == 4) return;
        
        if (effectStrength > 0) playerCamera.SetCameraMode(3);
        else playerCamera.SetCameraMode(1);
        
        playerCamera.velocity = Mathf.Lerp(0f, 1f, effectStrength);

        if (bloom != null)
        {
            bloom.intensity.value = Mathf.Lerp(0.25f, 150f, effectStrength);
        }

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 0.35f, effectStrength);
            vignette.smoothness.value = 0.8f;
        }

        if (depthOfField != null)
        {
            depthOfField.active = true;
            depthOfField.mode.value = DepthOfFieldMode.Gaussian;

            depthOfField.gaussianStart.value = Mathf.Lerp(8f, 100f, effectStrength);
            depthOfField.gaussianEnd.value = Mathf.Lerp(10000f, 15f, effectStrength);
        }

        if (filmGrain != null)
        {
            filmGrain.intensity.value = Mathf.Lerp(0f, 1f, effectStrength);
        }

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, effectStrength);
        }
    }
}