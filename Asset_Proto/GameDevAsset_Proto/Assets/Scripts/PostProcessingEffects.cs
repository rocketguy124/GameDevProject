using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffects : MonoBehaviour
{
    public static PostProcessingEffects instance;

    public PostProcessVolume volume;

    private LensDistortion LD;
    private ChromaticAberration CA;
    private ColorGrading CG;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out LD);
        volume.profile.TryGetSettings(out CA);
        volume.profile.TryGetSettings(out CG);

        LD.intensity.value = 0f;
    }


    public void TimeStopping()
    {
        StartCoroutine(EffectTimingStop());


        CA.intensity.value = Mathf.Lerp(CA.intensity.value, 0.5f, 100f * Time.deltaTime);
        CG.hueShift.value = Mathf.Lerp(CG.hueShift.value, 30f, 200f * Time.deltaTime);
        CG.saturation.value = Mathf.Lerp(CG.saturation.value, 65f, 200f * Time.deltaTime);


    }

    public void TimeResuming()
    {
        CA.intensity.value = Mathf.Lerp(CA.intensity.value, 0f, 100f);
        CG.hueShift.value = Mathf.Lerp(CG.hueShift.value, 0f, 200f);
        CG.saturation.value = Mathf.Lerp(CG.saturation.value, 0f, 200f);
        LD.intensity.value = Mathf.Lerp(LD.intensity.value, 0.0f, 1f);


    }

    public IEnumerator EffectTimingStop()
    {
        float t = 0;
        float TimeNeededHerePleaseHelp = 0.5f;
        while (t < TimeNeededHerePleaseHelp)
        {
            LD.intensity.value = Mathf.Lerp(0f, 75f, t / TimeNeededHerePleaseHelp);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (t > 0f)
        {
            LD.intensity.value = Mathf.Lerp(LD.intensity.value, -75.0f, ((1-(t/ TimeNeededHerePleaseHelp))));
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (t < TimeNeededHerePleaseHelp)
        {
            LD.intensity.value = Mathf.Lerp(-75f, 0f, t / TimeNeededHerePleaseHelp);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
}
