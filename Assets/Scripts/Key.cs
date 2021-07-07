using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Door;

public class Key : MonoBehaviour
{
    public DoorCode code;

    public void Start()
    {
        if(CompareTag("Keycard"))
            GetComponent<Renderer>().materials[0].color = KEYCARD_CODE_COLORS[code];
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

    public void RemoveFromInventory(Inventory inventory)
    {
        // key
        if (gameObject.CompareTag("Key"))
        {
            inventory.RemoveKey(this);
        }
        // keycard
        else if (gameObject.CompareTag("Keycard"))
        {
            inventory.RemoveKey(this);
        }
    }
}
