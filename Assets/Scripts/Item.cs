using UnityEditor;
using UnityEngine;

public abstract class Item
{
    public string name;
    public GameObject prefab;
    public Transform transLeftHand;
    public Transform transRightHand;

    private const string prefabPath = "Prefabs/";

    public Item(string name, Transform transLeftHand = null, Transform transRightHand = null)
    {
        this.name = name;
        this.prefab = Resources.Load<GameObject>($"{prefabPath}{name}");
        this.transLeftHand = transLeftHand;
        this.transRightHand = transRightHand;
    }

    public GameObject Make(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, pos, rot);
        if(parent != null)
            go.transform.parent = parent;
        return go;
    }
}