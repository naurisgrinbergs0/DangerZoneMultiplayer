using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Inventory inventory;
    private float reachDistance = 3;
    public bool ignoreInputs = false;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ignoreInputs)
        {
            // joystick
            /*if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if(hit.collider != null)
                    {
                        // copy all from below
                    }
                }
            }*/

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~layerMask))
                {
                    if (hit.collider != null)
                    {
                        if (hit.distance < reachDistance)
                        {
                            GameObject go = hit.collider.gameObject;
                            switch (go.tag)
                            {
                                case "Key":
                                    go.GetComponent<Key>().AddToInventory(inventory);
                                    break;
                                case "Keycard":
                                    go.GetComponent<Key>().AddToInventory(inventory);
                                    break;
                                case "Door":
                                case "DoorCompanion":
                                    Key matchingKey = go.GetComponent<Door>().Toggle(inventory.GetKeys());
                                    if (matchingKey != null)
                                        inventory.RemoveKey(matchingKey);
                                    break;
                                case "DoorKeycard":
                                case "DoorKeycardCompanion":
                                    go.GetComponent<Door>().Toggle(inventory.GetKeycards());
                                    break;
                                case "Drawer":
                                    go.GetComponent<Drawer>().Toggle();
                                    break;
                            }
                        }
                    }
                }
            }
        }
#endif
    }
}
