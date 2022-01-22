using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _currSleepLvl = 100;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Target HIT");
            Enemy enemy = other.transform.parent.GetComponent<Enemy>();

            if (enemy.IsDead)
            {
                return;
            }

            enemy.Damage(4, enemy.CounterType);
            _currSleepLvl = Mathf.Clamp(_currSleepLvl - 20, 0, 100);
        }
    }

    public float GetSleepLvlNormalized()
    {
        return _currSleepLvl / 100f;
    }
}
