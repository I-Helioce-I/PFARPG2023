using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleHandler : MonoBehaviour
{
    public List<VisualEffect> visualEffects = new List<VisualEffect>();
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public void PlayParticle(int index)
    {
        ParticleSystem newparticle = Instantiate<ParticleSystem>(particleSystems[index], transform.position, Quaternion.identity);
        newparticle.Play();
        var main = newparticle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

    public void PlayVFX(int index)
    {
        VisualEffect newparticle = Instantiate<VisualEffect>(visualEffects[index], transform.position, Quaternion.identity);
        newparticle.Play();
    }
}
