using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private bool _isFlickering = false;
    [Range(0, 20f)] public float offMin;
    [Range(0, 20f)] public float offMax;
    [Range(0, 20f)] public float onMin;
    [Range(0, 20f)] public float onMax;
    [SerializeField] private Material materialOff;
    [SerializeField] private Light lightObj;

    private float _initialIntensity;
    private Material _initialMaterial;
    private Material[] _materialArr;

    private void Start()
    {
        _initialIntensity = lightObj.intensity;
        _initialMaterial = GetComponent<Renderer>().materials[1];
        _materialArr = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFlickering)
            StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        _isFlickering = true;
        lightObj.intensity = _initialIntensity / 2f;
        _materialArr[1] = materialOff;
        GetComponent<Renderer>().materials = _materialArr;
        yield return new WaitForSeconds(Random.Range(offMin, offMax));
        lightObj.intensity = _initialIntensity;
        _materialArr[1] = _initialMaterial;
        GetComponent<Renderer>().materials = _materialArr;
        yield return new WaitForSeconds(Random.Range(onMin, onMax));
        _isFlickering = false;
    }
}
