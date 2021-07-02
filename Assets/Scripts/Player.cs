using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public byte health = 100;

    void Start()
    {

    }

    public void TakeDamage(byte damage)
    {
        health -= damage;

        // if health is negative - player died
        if (health <= 0) 
            Invoke(nameof(Destroy), 0.5f);
    }
    private void Destroy()
    {
        // died
    }
}