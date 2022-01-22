using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private RuneWords _word;

    public void Execute()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger enter");
        Enemy enemy = other.transform.parent.GetComponent<Enemy>();
        enemy.Damage(5, _word);
    }
}
