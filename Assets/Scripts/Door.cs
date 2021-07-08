using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Door companion;
    [SerializeField] private Color color;
    [SerializeField] private float angleOffset;
    public bool isLocked = false;
    public DoorCode code = DoorCode.NONE;

    public enum DoorCode
    {
        NONE,
        A, B, C, D, E, F, G, H,
        RED, BLUE, GREEN, BROWN, PINK, PURPLE, ORANGE, YELLOW
    }
    public static Dictionary<DoorCode, Color> KEYCARD_CODE_COLORS = new Dictionary<DoorCode, Color>() {
        {DoorCode.RED, Color.red },
        {DoorCode.BLUE, Color.blue },
        {DoorCode.GREEN, Color.green },
        {DoorCode.BROWN, new Color(0.478f, 0.219f, 0) },
        {DoorCode.PINK, new Color(1, 0.458f, 0.952f) },
        {DoorCode.PURPLE, new Color(0.650f, 0, 0.741f) },
        {DoorCode.ORANGE, new Color(1, 0.470f, 0.101f) },
        {DoorCode.YELLOW, Color.yellow }
    };
    public static DoorCode ParseDoorCode(string code)
    {
        foreach (DoorCode dc in Enum.GetValues(typeof(DoorCode)))
            if (dc.ToString("g") == code)
                return dc;
        return DoorCode.NONE;
    }



    public void Start()
    {
        // set initial angle
        if(angleOffset != 0)
            transform.Rotate(new Vector3(0, 0, angleOffset));

        // set code reader color
        if(CompareTag("DoorKeycard"))
            GetComponent<Renderer>().materials[3].color = color;

        // don't allow movement if locked
        if (CompareTag("DoorKeycard") || CompareTag("Door"))
        {
            if (isLocked)
            {
                GetComponent<Rigidbody>().freezeRotation = true;
                // if this is keyreader door then if companion exists, it should be locked too
                if (companion != null)
                {
                    companion.isLocked = true;
                    companion.GetComponent<Rigidbody>().freezeRotation = true;
                }
            }
            // code is the same for companion
            if (companion != null)
                companion.code = code;
        }
    }

    /// <summary>
    /// Opens/Closes door
    /// If any key unlocks this door it is returned, otherwise - null
    /// </summary>
    public Key Toggle(List<Key> keys)
    {
        Key matchingKey = null;

        // check if door is open
        bool isOpen = System.Math.Abs(transform.localRotation.normalized.y) > 0.1;

        // determine wether to allow toggle
        bool allowToggle = false;

        if (isLocked)
        {
            foreach (Key key in keys)
            {
                if(key.code == this.code)
                {
                    // if companion door exists - unlock it as well
                    if (companion != null)
                    {
                        companion.isLocked = false; // if was locked then it is unlocked now
                        companion.GetComponent<Rigidbody>().freezeRotation = false; // enable door rotation
                    }

                    isLocked = false; // if was locked then it is unlocked now
                    GetComponent<Rigidbody>().freezeRotation = false; // enable door rotation
                    allowToggle = true;

                    matchingKey = key;
                    break;
                }
            }
        }
        else if(!isLocked || isOpen)
        {
            allowToggle = true;
        }

        // toggle
        if (allowToggle)
        {
            Vector3 dir = Vector3.zero;
            if (CompareTag("DoorKeycard") || CompareTag("Door"))
                dir = isOpen ? transform.forward * 100 : -transform.forward * 100;
            else if(CompareTag("DoorKeycardCompanion") || CompareTag("DoorCompanion"))
                dir = isOpen ? -transform.forward * 100 : transform.forward * 100;

            GetComponent<Rigidbody>().AddTorque(dir, ForceMode.Impulse);
        }

        return matchingKey;
    }
}
