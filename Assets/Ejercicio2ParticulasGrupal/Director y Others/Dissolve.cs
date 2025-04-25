using UnityEngine;
using UnityEngine.Playables;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private float startTime = 0.0f;          
    [SerializeField] private float dissolveDuration = 3.0f;     
    [SerializeField] private float dissolveStrength = 1.0f;      
    [SerializeField] private PlayableDirector director;
    [SerializeField] private float dissolveOffset;

    
    private float noiseOffset = 0.0f;
    private Material dissolveMaterial;
    

    void Start()
    {
        dissolveMaterial = GetComponent<Renderer>().material;
        noiseOffset = Random.Range(0.2f, 0.6f);
        dissolveMaterial.SetFloat("_NoiseOffset", noiseOffset);
    }

    void Update()
    {
        if (director != null && director.state == PlayState.Playing)
        {
            var currentTime = director.time;


            if (currentTime < startTime)
                return;

     
            float t = (float)((currentTime - startTime) / dissolveDuration);
            t = Mathf.Clamp01(t);

            dissolveStrength = Mathf.Lerp(dissolveOffset, 1.0f, t);
            dissolveMaterial.SetFloat("_DissolveStrenght", dissolveStrength);
        }
    }
}