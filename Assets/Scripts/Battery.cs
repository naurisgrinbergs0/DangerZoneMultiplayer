using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public int charge;

    void Start()
    {
        int val = Random.Range(1, 101);
        if (val < 20)
            charge = Random.Range(5, 31);
        else
            charge = Random.Range(31, 101);
    }

    public void Discharge(int percent)
    {
        charge = Mathf.Max(0, charge - percent);
    }
}
