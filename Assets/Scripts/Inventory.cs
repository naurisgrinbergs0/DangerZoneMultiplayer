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

    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    public void Start()
    {
        Key k = new Key();
        k.code = DoorCode.A;
        AddKey(k);
        

        prefabs.Add("key", Resources.Load<GameObject>("Prefabs/Key"));
        prefabs.Add("keycard", Resources.Load<GameObject>("Prefabs/Keycard"));
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
        GameObject keyObj = Instantiate(prefabs["key"], transform.position, UnityEngine.Random.rotation);
        keyObj.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
    }
    public void DropKeycard(Key key)
    {
        _keycards.Remove(key);
        GameObject keyObj = Instantiate(prefabs["keycard"], transform.position, UnityEngine.Random.rotation);
        GetComponent<Renderer>().materials[0].color = KEYCARD_CODE_COLORS[key.code];
        keyObj.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
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
