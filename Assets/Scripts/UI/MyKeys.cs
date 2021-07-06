using UnityEngine;
using UnityEngine.UIElements;

public class MyKeys : MonoBehaviour
{
    private UIDocument ui;
    [SerializeField] Inventory inventory;
    [SerializeField] Movement movement;


    private void Awake()
    {
        ui = GetComponent<UIDocument>();
        ui.enabled = true;
    }

    public void OnEnable()
    {
        VisualElement root = ui.rootVisualElement;
        root.visible = false;
        
        root.Q<Button>("btn-done").clicked += CloseUI;
        
        for(int i = 0; i < 8; i++)
        {
            Button btnKey = root.Q<Button>($"btn-key{i+1}");
            btnKey.RegisterCallback<ClickEvent>(e => DropKey(btnKey, i));
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
        }
    }

    void CloseUI()
    {
        ui.rootVisualElement.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        movement.ignoreInputs = false;
    }

    void DropKey(Button btnKey, int index)
    {
        btnKey.RemoveFromHierarchy();
        //inventory.RemoveKey();
    }
}
