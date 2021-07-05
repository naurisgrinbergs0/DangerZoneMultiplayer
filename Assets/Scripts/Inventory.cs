using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Key> _keys = new List<Key>();
    private List<Key> _keycards = new List<Key>();
    private List<string> _tools = new List<string>();

    [SerializeField] private Canvas _canvasCollect;
    [SerializeField] private Image _tKeycardRed;
    [SerializeField] private Image _tKeycardBlue;
    [SerializeField] private Image _tKeycardGreen;
    [SerializeField] private Image _tKeycardBrown;
    [SerializeField] private Image _tKeycardPink;
    [SerializeField] private Image _tKeycardPurple;
    [SerializeField] private Image _tKeycardOrange;
    [SerializeField] private Image _tKeycardYellow;
    [SerializeField] private Image _tKey;

    private Image[] _tKeys = new Image[8];

    public void Start()
    {
        // instantiate keys
        _tKey.gameObject.SetActive(false);
        for(int i = 0; i < 8; i++)
        {
            Image keyImg = Instantiate(_tKey, _canvasCollect.transform) as Image;
            keyImg.transform.Translate(-(_tKey.rectTransform.rect.width / 2) * i, 0, 0);
            _tKeys[i] = keyImg;
        }

        UpdateCanvas();
    }


    public void Update()
    {
        // drop key
        if (Input.GetKey(KeyCode.E))
        {
            if(_keys.Count > 0)
            {
                
            }
        }
    }


    public void AddKey(Key key)
    {
        _keys.Add(key);
        UpdateCanvas();
    }

    public void AddKeycard(Key keycard)
    {
        _keycards.Add(keycard);
        UpdateCanvas();
    }

    public void RemoveKey(Key key)
    {
        _keys.Remove(key);
        UpdateCanvas();
    }

    public void RemoveKeycard(Key keycard)
    {
        _keycards.Remove(keycard);
        UpdateCanvas();
    }


    public List<Key> GetKeys()
    {
        return _keys;
    }

    public List<Key> GetKeycards()
    {
        return _keycards;
    }

    private void UpdateCanvas()
    {
        // update keys
        for (int i = 0; i < _tKeys.Length; i++)
            _tKeys[i].gameObject.SetActive(i <= _keys.Count - 1);

        // update keycards
        Dictionary<string, Image> kcs = new Dictionary<string, Image>()
        {
            { "RED", _tKeycardRed }
            , { "BLUE", _tKeycardBlue }
            , { "GREEN", _tKeycardGreen }
            , { "BROWN", _tKeycardBrown }
            , { "PINK", _tKeycardPink }
            , { "PURPLE", _tKeycardPurple }
            , { "ORANGE", _tKeycardOrange }
            , { "YELLOW", _tKeycardYellow }
        };

        int cardsTranslated = 0;
        foreach (var kc in kcs) 
        {
            // show available cards & hide other
            bool enabled = _keycards.Find(k => k.code == kc.Key) != null;
            kc.Value.gameObject.SetActive(enabled);

            // translate cards next to eachother
            if (enabled)
            {
                kc.Value.transform.Translate((kc.Value.rectTransform.rect.width / 2) * (float)cardsTranslated, 0, 0);
                cardsTranslated++;
            }
        }
    }

}
