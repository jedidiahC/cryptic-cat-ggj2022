using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneProjectile : MonoBehaviour
{
    private Vector3 _dir;
    private float _speed;
    private bool _hit = false;

    public void SetProjectileSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDir(Vector3 dir)
    {
        _dir = dir;
    }

    private void Update()
    {
        transform.position += _dir * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("On trigger enter");
        Enemy enemy = other.transform.parent.GetComponent<Enemy>();

        if (enemy != null && !_hit)
        {
            enemy.Damage(1, enemy.CounterType);
            _hit = true;
        }

        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.2f);
    }
}
