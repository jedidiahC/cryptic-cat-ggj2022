using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public UnityEvent onDead;

    [SerializeField] private float _currSleepLvl = 100;
    [SerializeField] private float _damagePerLetter = 2;

    private bool _dead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_dead) { return; }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Target HIT");
            Enemy enemy = other.transform.parent.GetComponent<Enemy>();

            if (enemy.IsDead)
            {
                return;
            }

            enemy.Damage(4, enemy.CounterType);
            _currSleepLvl = Mathf.Clamp(_currSleepLvl - _damagePerLetter * enemy.WordLength, 0, 100);

            if (_currSleepLvl <= 0)
            {
                onDead.Invoke();
                _dead = true;
            }
        }
    }

    public float GetSleepLvlNormalized()
    {
        return _currSleepLvl / 100f;
    }
}
