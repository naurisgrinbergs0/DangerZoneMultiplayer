using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public void Toggle()
    {
        bool isOpen = transform.localPosition.normalized.z > 0.1;

        float multiplier = 340f;
        GetComponent<Rigidbody>().AddForce(
            isOpen ? transform.up * multiplier : -transform.up * multiplier);
    }
}
