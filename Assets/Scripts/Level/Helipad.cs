using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class Helipad : MonoBehaviour, IPointerInteractable
{

    [SerializeField]
    private int helicopterCooldown;
    [SerializeField]
    private int startingCooldown;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectColor;

    [Header("UI")]
    [SerializeField]
    private GameObject cooldownUiPrefab;

    public static UnityEvent OnHelicopterActivated = new UnityEvent();

    private MeshRenderer meshRenderer;
    private OvertextHandler uiHandler;

    private int cooldown;

    void OnEnable() {
        TurnManager.OnCorruptionTurnEnded.AddListener(ProgressCooldown);
    }

    void OnDisable() {
        TurnManager.OnCorruptionTurnEnded.RemoveListener(ProgressCooldown);
    }

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = defaultColor;

        cooldown = startingCooldown;
        
        uiHandler = Instantiate(cooldownUiPrefab, transform.position, Quaternion.identity).GetComponent<OvertextHandler>();
        uiHandler.SetValue(cooldown);
    }

    public void OnPointerEnter() {
        meshRenderer.material.color = selectColor;
    }

    public void OnPointerExit() {
        meshRenderer.material.color = defaultColor;
    }

    public void OnPointerDown() {
        ActivateHelicopter();
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
