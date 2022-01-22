using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private RuneWords _words;
    [SerializeField] private RuneProjectile _runeProjectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireInterval = 1;
    [SerializeField] private float _projectileSpeed = 5.0f;
    [SerializeField] private int _maxProjectiles = 20;

    private float _timer = 0;
    private float _projectilesFired = 0;

    private List<Transform> _targetsInRange = new List<Transform>();

    public void SetRuneWords(RuneWords words)
    {
        _words = words;
    }

    private void Awake()
    {
        Debug.Assert(_runeProjectilePrefab != null, "_runeProjectilePrefab is not assigned!");
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            for (int i = 0; i < _targetsInRange.Count; i++)
            {
                Transform target = _targetsInRange[i];

                if (target != null)
                {
                    if (target.parent.GetComponent<Enemy>().IsDead)
                    {
                        _targetsInRange.Remove(target);
                        continue;
                    }

                    Vector3 dir = (target.transform.position - this.transform.position).normalized;
                    RuneProjectile projectile = Instantiate<RuneProjectile>(_runeProjectilePrefab, _firePoint.transform.position, Quaternion.identity);
                    projectile.SetDir(dir);
                    projectile.SetProjectileSpeed(_projectileSpeed);

                    _timer = _fireInterval;

                    _projectilesFired++;
                    if (_projectilesFired >= _maxProjectiles)
                    {
                        gameObject.SetActive(false);
                    }
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("On trigger enter");
        // Enemy enemy = other.transform.parent.GetComponent<Enemy>();
        // enemy.Damage(5, _words);
        // gameObject.SetActive(false);

        Enemy enemy = other.transform.parent.GetComponent<Enemy>();
        if (enemy != null && !_targetsInRange.Contains(other.transform))
        {
            _targetsInRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.parent.GetComponent<Enemy>();

        if (enemy != null && _targetsInRange.Contains(other.transform))
        {
            _targetsInRange.Remove(other.transform);
        }
    }
}
