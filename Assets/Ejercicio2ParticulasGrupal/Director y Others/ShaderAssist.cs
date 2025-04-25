using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ShaderAssist : MonoBehaviour
{
    private ParticleSystem ps;
    private Material matInstance;
    private float dissolveSpeed;
    private float agePercent;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        matInstance = Instantiate(renderer.material);
        renderer.material = matInstance;

        dissolveSpeed = Random.Range(0.2f, 0.6f);
        matInstance.SetFloat("DissolveSpeed", dissolveSpeed);
    }

    void Update()
    {
        if (ps.particleCount > 0)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
            ps.GetParticles(particles, 1); 

            if (particles.Length > 0)
            {
                float age = particles[0].startLifetime - particles[0].remainingLifetime;
                agePercent = Mathf.Clamp01(age / particles[0].startLifetime);
                matInstance.SetFloat("AgePercent", agePercent);
            }
        }
    }
}