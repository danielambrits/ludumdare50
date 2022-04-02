using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class GroundTile : MonoBehaviour, IPointerInteractable
{
    public enum Type {
        Default,
        Wall,
        House,
        Factory,
        Helipad,
    }

    [SerializeField]
    private int helicopterCooldown;
    [SerializeField]
    private int evacuationCooldown;
    [SerializeField]
    private GameObject cooldownUiPrefab;

    private MeshRenderer meshRenderer;
    private Color defaultColor;

    private OvertextHandler uiHandler;

    private int cooldown;

    public bool isCorrupted;
    public Type type;

    public static UnityEvent OnWallBuilt = new UnityEvent();
    public static UnityEvent OnHouseBuilt = new UnityEvent();
    public static UnityEvent OnHouseDestroyed = new UnityEvent();
    public static UnityEvent OnFactoryBuilt = new UnityEvent();
    public static UnityEvent OnFactoryDestroyed = new UnityEvent();

    public static UnityEvent OnHelicopterActivated = new UnityEvent();

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        type = Type.Default;
        isCorrupted = false;
        uiHandler = null;
    }

    void OnEnable() {
        TurnManager.OnCorruptionTurnEnded.AddListener(ProgressCooldown);
    }

    void OnDisable() {
        TurnManager.OnCorruptionTurnEnded.RemoveListener(ProgressCooldown);
    }

    public void OnPointerEnter() {
        meshRenderer.material.color = Color.magenta;
    }

    public void OnPointerExit() {
        meshRenderer.material.color = defaultColor;
    }

    public void OnPointerDown() {
        switch (type) {
            case Type.Default:
                BuildWall();
                break;
            case Type.Helipad:
                ActivateHelicopter();
                break;
            default:
                // NOP
                break;
        }
    }

    public void Corrupt() {
        if (isCorrupted) {
            return;
        }
        isCorrupted = true;
        switch (type) {
            case Type.Wall:
                return;
            case Type.House:
                OnHouseDestroyed.Invoke();
                break;
            case Type.Factory:
                OnFactoryDestroyed.Invoke();
                break;
        }
        defaultColor = Color.black;
        meshRenderer.material.color = defaultColor;
    }

    private void BuildWall() {
        type = Type.Wall;
        defaultColor = Color.gray;
        meshRenderer.material.color = defaultColor;
        OnWallBuilt.Invoke();
    }

    public void BuildHouse() {
        type = Type.House;
        defaultColor = Color.green;
        meshRenderer.material.color = defaultColor;
        OnHouseBuilt.Invoke();
    }

    public void BuildFactory() {
        type = Type.Factory;
        defaultColor = Color.yellow;
        meshRenderer.material.color = defaultColor;
        OnFactoryBuilt.Invoke();
    }

    public void BuildHelipad() {
        type = Type.Helipad;
        defaultColor = Color.cyan;
        meshRenderer.material.color = defaultColor;
        uiHandler = Instantiate(cooldownUiPrefab, transform.position, Quaternion.identity).GetComponent<OvertextHandler>();
        uiHandler.SetValue(0);
    }

    private void ActivateHelicopter() {
        if (cooldown > 0) {
            return;
        }
        cooldown = helicopterCooldown;
        uiHandler.SetValue(cooldown);
        OnHelicopterActivated.Invoke();
    }

    private void ProgressCooldown() {
        if (cooldown > 0) {
            cooldown--;
            uiHandler.SetValue(cooldown);
        }
    }

}
