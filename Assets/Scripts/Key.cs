using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public string code;
    [SerializeField] private Color color;

    public void Start()
    {
        if(CompareTag("Keycard"))
            GetComponent<Renderer>().materials[0].color = color;
    }

    public void AddToInventory(Inventory inventory)
    {
        // key
        if(gameObject.CompareTag("Key"))
        {
            inventory.AddKey(this);
        }
        // keycard
        else if(gameObject.CompareTag("Keycard"))
        {
            inventory.AddKeycard(this);
        }

        gameObject.SetActive(false);
    }

}
