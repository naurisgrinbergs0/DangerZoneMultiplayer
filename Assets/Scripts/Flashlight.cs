using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private Battery[] batteries = new Battery[2];
    public bool isShining = false;
    public int charge { 
        get {
            int c = 0;
            foreach (Battery b in batteries)
                c += b.charge;
            return c / batteries.Length;
        }
    }
    [SerializeField] private int chargeMinimum = 10;
    [SerializeField] private float dischargeTimeMinutes = 7;
    private float countdown;
    public int intensity = 100;


    private void Start()
    {
        countdown = dischargeTimeMinutes * 60f / 100f;
    }

    public void Toggle()
    {
        isShining = !isShining;
    }

    private void Update()
    {
        if (isShining && charge > chargeMinimum)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0)
            {
                foreach (Battery b in batteries)
                    b.Discharge(1);
                countdown = dischargeTimeMinutes * 60f / 100f;
            }
        }
        else if (charge <= chargeMinimum)
        {
            SwapBatteries();
        }
    }

    public void SwapBatteries()
    {
        if (charge < chargeMinimum)
        {
            for(int i = 0; i < batteries.Length; i++)
            {
                if(inventory.GetBatteries().Count > 0)
                {
                    batteries[0] = inventory.GetBattery(0);
                    inventory.RemoveBattery(0);
                }
            }
        }
    }
}
