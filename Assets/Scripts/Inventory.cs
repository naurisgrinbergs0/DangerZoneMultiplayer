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
    private List<string> _tools = new List<string>();
    [SerializeField] private Transform keyObjParent;

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    public void Start()
    {
        prefabs.Add("key", Resources.Load<GameObject>("Prefabs/Key"));
        prefabs.Add("keycard", Resources.Load<GameObject>("Prefabs/Keycard"));


        GameObject go1 = Instantiate(prefabs["key"], transform.position + transform.up * 3, UnityEngine.Random.rotation);
        go1.GetComponent<Key>().code = DoorCode.A;
        GameObject go2 = Instantiate(prefabs["key"], transform.position + transform.up * 3, UnityEngine.Random.rotation);
        go2.GetComponent<Key>().code = DoorCode.B;
        GameObject no1 = Instantiate(prefabs["keycard"], transform.position + transform.up * 3, UnityEngine.Random.rotation);
        no1.GetComponent<Key>().code = DoorCode.RED;
    }


    public Key GetKey(DoorCode code)
    {
        return _keys.Find(k => k.code == code);
    }
    public Key GetKeycard(DoorCode code)
    {
        return _keycards.Find(k => k.code == code);
    }

    public void AddKey(Key key)
    {
        _keys.Add(key);
    }
    public void AddKeycard(Key keycard)
    {
        _keycards.Add(keycard);
    }

    public void RemoveKey(Key key)
    {
        _keys.Remove(key);
    }
    public void RemoveKeycard(Key keycard)
    {
        _keycards.Remove(keycard);
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
        keyObj.transform.SetParent(keyObjParent);
        keyObj.GetComponent<Rigidbody>().AddForce(/*transform.forward*/GetComponent<Movement>().cameraObj.forward * 2, ForceMode.Impulse);
    }


    public List<Key> GetKeys()
    {
        return _keys;
    }
    public List<Key> GetKeycards()
    {
        return _keycards;
    }

}
