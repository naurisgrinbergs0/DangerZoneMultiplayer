using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Key> keys = new List<Key>();
    private List<Key> keycards = new List<Key>();
    private List<string> tools = new List<string>();

    [SerializeField] private Canvas canvasCollect;
    [SerializeField] private Image tKeycardRed;
    [SerializeField] private Image tKeycardBlue;
    [SerializeField] private Image tKeycardGreen;
    [SerializeField] private Image tKeycardBrown;
    [SerializeField] private Image tKeycardPink;
    [SerializeField] private Image tKeycardPurple;
    [SerializeField] private Image tKeycardOrange;
    [SerializeField] private Image tKeycardYellow;
    [SerializeField] private Image tKey;

    private Image[] tKeys = new Image[8];

    public void Start()
    {
        // instantiate keys
        tKey.gameObject.SetActive(false);
        for(int i = 0; i < 8; i++)
        {
            Image keyImg = Instantiate(tKey, canvasCollect.transform) as Image;
            keyImg.transform.Translate(-(tKey.rectTransform.rect.width / 2) * i, 0, 0);
            tKeys[i] = keyImg;
        }

        UpdateCanvas();
    }


    public void AddKey(Key key)
    {
        keys.Add(key);
        UpdateCanvas();
    }

    public void AddKeycard(Key keycard)
    {
        keycards.Add(keycard);
        UpdateCanvas();
    }

    public List<Key> GetKeys()
    {
        return keys;
    }

    public List<Key> GetKeycards()
    {
        return keycards;
    }

    private void UpdateCanvas()
    {
        // update keys
        for (int i = 0; i < keys.Count; i++)
            tKeys[i].gameObject.SetActive(i <= keys.Count);

        // update keycards
        Dictionary<string, Image> kcs = new Dictionary<string, Image>()
        {
            { "RED", tKeycardRed }
            , { "BLUE", tKeycardBlue }
            , { "GREEN", tKeycardGreen }
            , { "BROWN", tKeycardBrown }
            , { "PINK", tKeycardPink }
            , { "PURPLE", tKeycardPurple }
            , { "ORANGE", tKeycardOrange }
            , { "YELLOW", tKeycardYellow }
        };

        int cardsTranslated = 0;
        foreach (var kc in kcs) 
        {
            // show available cards & hide other
            bool enabled = keycards.Find(k => k.code == kc.Key) != null;
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
