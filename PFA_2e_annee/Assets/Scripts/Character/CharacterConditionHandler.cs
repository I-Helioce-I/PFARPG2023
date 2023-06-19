using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterConditionHandler : MonoBehaviour
{
    private bool _isStunned = false;
    public bool IsStunned
    {
        get
        {
            return _isStunned;
        }
        set
        {
            _isStunned = value;
            StunParticles(value);
        }
    }
    public bool IsDefending = false;
    public bool InitiativeFirstInLine = false;

    [SerializeField] private Transform _overHead;
    [SerializeField] private ParticleSystem _stunnedParticles;
    private ParticleSystem _currentStunParticles;

    public void StunParticles(bool show)
    {
        if (show)
        {
            ParticleSystem particles = Instantiate<ParticleSystem>(_stunnedParticles, _overHead.position, transform.rotation);
            particles.Play();
            _currentStunParticles = particles;
        }
        else
        {
            if (_currentStunParticles == null) return;
            _currentStunParticles.Stop();
            Destroy(_currentStunParticles.gameObject);
            _currentStunParticles = null;
        }

    }
}
