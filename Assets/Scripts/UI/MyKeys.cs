using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Door;

public class MyKeys : MonoBehaviour
{
    private UIDocument ui;
    private VisualElement uiRoot;
    [SerializeField] Inventory inventory;
    [SerializeField] Movement movement;
    private Dictionary<DoorCode, Button> uiBtnKeySlots = new Dictionary<DoorCode, Button>();
    private Dictionary<DoorCode, Button> uiBtnKeycardSlots = new Dictionary<DoorCode, Button>();

    private void Awake()
    {
        ui = GetComponent<UIDocument>();
        ui.enabled = true;
        uiRoot = ui.rootVisualElement;

        var btnKeyList = uiRoot.Q("container-keys").Children().Where(c => c.ClassListContains("btn-key-slot")).ToList();
        var btnKeycardList = uiRoot.Q("container-keycards").Children().Where(c => c.ClassListContains("btn-keycard-slot")).ToList();

        foreach (Button b in btnKeyList)
            uiBtnKeySlots.Add(ParseDoorCode(b.name.Replace("btn-key-slot-", "").ToUpper()), b);
        foreach (Button b in btnKeycardList)
            uiBtnKeycardSlots.Add(ParseDoorCode(b.name.Replace("btn-keycard-slot-", "").ToUpper()), b);
    }

    public void OnEnable()
    {
        uiRoot.visible = false;
        uiRoot.Q<Button>("btn-done").clicked += CloseUI;

        foreach(var b in uiBtnKeySlots)
        {
            b.Value.RegisterCallback<ClickEvent>(e => DropKey(b.Key));
            b.Value.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
        foreach (var b in uiBtnKeycardSlots)
        {
            b.Value.RegisterCallback<ClickEvent>(e => DropKeycard(b.Key));
            b.Value.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
    }

    public void Update()
    {
        // open my keys
        if (Input.GetKey(KeyCode.E))
        {
            movement.ignoreInputs = true;
            ui.rootVisualElement.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            foreach(var b in uiBtnKeySlots)
            {
                DisplayStyle disp = inventory.GetKey(b.Key) != null ? DisplayStyle.Flex : DisplayStyle.None;
                b.Value.style.display = new StyleEnum<DisplayStyle>(disp);
            }
            foreach (var b in uiBtnKeycardSlots)
            {
                DisplayStyle disp = inventory.GetKeycard(b.Key) != null ? DisplayStyle.Flex : DisplayStyle.None;
                b.Value.style.display = new StyleEnum<DisplayStyle>(disp);
            }
        }
    }

    void CloseUI()
    {
        ui.rootVisualElement.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        movement.ignoreInputs = false;
    }

    void DropKey(DoorCode btnCode)
    {
        uiBtnKeySlots[btnCode].style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        inventory.DropKey(inventory.GetKey(btnCode));
    }
    void DropKeycard(DoorCode btnCode)
    {
        uiBtnKeycardSlots[btnCode].style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        inventory.DropKeycard(inventory.GetKeycard(btnCode));
    }
}
