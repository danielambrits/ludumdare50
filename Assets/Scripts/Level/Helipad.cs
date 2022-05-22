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
    public static UnityEvent OnInvalidActivation = new UnityEvent();
    public static UnityEvent OnHelipadCountChange = new UnityEvent();

    public static int overallCount;
    public static int inCooldownCount;

    private MeshRenderer meshRenderer;
    private OvertextHandler uiHandler;

    private int cooldown;

    void OnEnable() {
        overallCount++;
        OnHelipadCountChange.Invoke();
        TurnManager.OnCorruptionTurnEnded.AddListener(ProgressCooldown);
    }

    void OnDisable() {
        overallCount--;
        if (cooldown > 0) {
            inCooldownCount--;
        }
        OnHelipadCountChange.Invoke();
        TurnManager.OnCorruptionTurnEnded.RemoveListener(ProgressCooldown);
    }

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = defaultColor;

        cooldown = startingCooldown;
        if (startingCooldown > 0) {
            inCooldownCount++;
        }
        
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
        if (TurnManager.wallAlreadyBuiltIntHisTurn) {
            OnInvalidActivation.Invoke();
            return;
        }
        ActivateHelicopter();
    }

    private void ActivateHelicopter() {
        if (cooldown > 0) {
            OnInvalidActivation.Invoke();
            return;
        }
        inCooldownCount++;
        OnHelipadCountChange.Invoke();
        cooldown = helicopterCooldown;
        uiHandler.SetValue(cooldown);
        OnHelicopterActivated.Invoke();
    }

    private void ProgressCooldown() {
        if (cooldown > 0) {
            cooldown--;
            uiHandler.SetValue(cooldown);
            if (cooldown == 0) {
                inCooldownCount--;
                OnHelipadCountChange.Invoke();
            }
        }
    }

}
