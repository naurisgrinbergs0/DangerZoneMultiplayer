using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Door;

public class Inventory : MonoBehaviour
{
    private List<Key> _keys = new List<Key>();
    private List<Key> _keycards = new List<Key>();
    private List<Battery> _batteries = new List<Battery>();
    private List<string> _tools = new List<string>();

    private Dictionary<string, ToolDetails> prefabs = new Dictionary<string, ToolDetails>();

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public void Start()
    {
        prefabs.Add("Key", new ToolDetails("Key"));
        prefabs.Add("Keycard", new ToolDetails("Keycard"));
        prefabs.Add("Flashlight", new ToolDetails("Flashlight", ));
        

        //Vector3(-0.000676000025, 0.00103499996, -0.00221699988)
            //Vector3(356.636719, 12.2085152, 269.534058)

        //////////////////////////////////////////////////////////////// SOME TEST INSTANCES ///////////////////////

        GameObject go1 = prefabs["Key"].Make(transform.position + transform.up * 3, UnityEngine.Random.rotation);
        go1.GetComponent<Key>().code = DoorCode.A;
        GameObject go2 = prefabs["Key"].Make(transform.position + transform.up * 3, UnityEngine.Random.rotation);
        go2.GetComponent<Key>().code = DoorCode.B;
        GameObject no1 = prefabs["Keycard"].Make(transform.position + transform.up * 3, UnityEngine.Random.rotation);
        no1.GetComponent<Key>().code = DoorCode.RED;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        GameObject flashlight = prefabs["Flashlight"].Make();
        flashlight.transform.parent = leftHand.transform;
        flashlight.transform.position = leftHand.position;
    }


    public Key GetKey(DoorCode code)
    {
        return _keys.Find(k => k.code == code);
    }
    public Key GetKeycard(DoorCode code)
    {
        return _keycards.Find(k => k.code == code);
    }
    public Battery GetBattery(int index)
    {
        return _batteries[index];
    }

    public void AddKey(Key key)
    {
        _keys.Add(key);
    }
    public void AddKeycard(Key keycard)
    {
        _keycards.Add(keycard);
    }
    public void AddBattery(Battery battery)
    {
        _batteries.Add(battery);
    }

    public void RemoveKey(Key key)
    {
        _keys.Remove(key);
    }
    public void RemoveKeycard(Key keycard)
    {
        _keycards.Remove(keycard);
    }
    public void RemoveBattery(int index)
    {
        _batteries.RemoveAt(index);
    }


    public void DropKey(Key key)
    {
        _keys.Remove(key);
        GameObject keyObj = Instantiate(prefabs["key"], 
            transform.position + transform.forward * 0.3f + transform.up * 0.8f, UnityEngine.Random.rotation);
        //Physics.IgnoreCollision(GetComponent<Collider>(), keyObj.GetComponent<Collider>(), true);
        keyObj.GetComponent<Rigidbody>().AddForce(/*transform.forward*/GetComponent<Movement>().cameraObj.forward * 2, ForceMode.Impulse);
        keyObj.GetComponent<Key>().code = key.code;
    }
    public void DropKeycard(Key key)
    {
        _keycards.Remove(key);
        GameObject keyObj = Instantiate(prefabs["keycard"],
            transform.position + transform.forward * 0.3f + transform.up * 0.8f, UnityEngine.Random.rotation);
        //Physics.IgnoreCollision(GetComponent<Collider>(), keyObj.GetComponent<Collider>(), true);
        keyObj.GetComponent<Renderer>().materials[0].color = KEYCARD_CODE_COLORS[key.code];
        keyObj.GetComponent<Key>().code = key.code;
        //keyObj.transform.SetParent(keyObjParent);
        keyObj.GetComponent<Rigidbody>().AddForce(/*transform.forward*/GetComponent<Movement>().cameraObj.forward * 2, ForceMode.Impulse);
    }
    public void DropBattery(Battery battery)
    {
        _batteries.Remove(battery);
        GameObject batteryObj = Instantiate(prefabs["battery"],
            transform.position + transform.forward * 0.3f + transform.up * 0.8f, UnityEngine.Random.rotation);
        //Physics.IgnoreCollision(GetComponent<Collider>(), keyObj.GetComponent<Collider>(), true);
        //batteryObj.transform.SetParent(keyObjParent);
        batteryObj.GetComponent<Rigidbody>().AddForce(GetComponent<Movement>().cameraObj.forward * 2, ForceMode.Impulse);
    }


    public List<Key> GetKeys()
    {
        return _keys;
    }
    public List<Key> GetKeycards()
    {
        return _keycards;
    }
    public List<Battery> GetBatteries()
    {
        return _batteries;
    }

}
