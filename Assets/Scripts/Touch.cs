using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Touch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    public Inventory inventory;
    private float _reachDistance = 2;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider != null)
                {
                    // copy all from below
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_layerMask))
            {
                if (hit.collider != null)
                {
                    if (hit.distance < _reachDistance)
                    {
                        switch (hit.collider.gameObject.tag)
                        {
                            case "Key":
                                hit.collider.gameObject.GetComponent<Key>().AddToInventory(inventory);
                                break;
                            case "Keycard":
                                hit.collider.gameObject.GetComponent<Key>().AddToInventory(inventory);
                                break;
                            case "Door":
                            case "DoorCompanion":
                                Key matchingKey = hit.collider.gameObject.GetComponent<Door>().Toggle(inventory.GetKeys());
                                if (matchingKey != null)
                                    inventory.RemoveKey(matchingKey);
                                break;
                            case "DoorKeycard":
                            case "DoorKeycardCompanion":
                                hit.collider.gameObject.GetComponent<Door>().Toggle(inventory.GetKeycards());
                                break;
                        }
                    }
                }
            }
        }
#endif
    }
}
