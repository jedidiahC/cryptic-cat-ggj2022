using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySegment : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticles = null;
    [SerializeField] private TrailRenderer _trail = null;
    [SerializeField] private Letter _letter = null;

    private void Awake()
    {
        Debug.Assert(_deathParticles != null, "_deathParticles is not assigned!");
        Debug.Assert(_letter != null, "_letter is not assigned!");
        Debug.Assert(_trail != null, "_trail is not assigned!");
    }

    public void SetColor(Color color)
    {
        _letter.SetColor(color);
    }

    public void SetTrailColor(Color color)
    {
        _trail.startColor = color;
    }

    public void OnDeath()
    {
        _deathParticles.gameObject.SetActive(true);
        _letter.SetVisible(false);
        _trail.gameObject.SetActive(false);
    }
}
