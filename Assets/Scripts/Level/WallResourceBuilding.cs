using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class WallResourceBuilding : MonoBehaviour, IPointerInteractable
{
    
    [SerializeField]
    private int evacuationCooldown;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectColor;

    [Header("UI")]
    [SerializeField]
    private GameObject cooldownUiPrefab;

    private MeshRenderer meshRenderer;
    private OvertextHandler uiHandler;
    
    private Tile baseTile;
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

        cooldown = 0;
        
        uiHandler = Instantiate(cooldownUiPrefab, transform.position, Quaternion.identity).GetComponent<OvertextHandler>();
        uiHandler.transform.SetParent(transform);
        uiHandler.SetValue(cooldown);
    }

    public void OnPointerEnter() {
        meshRenderer.material.color = selectColor;
    }

    public void OnPointerExit() {
        meshRenderer.material.color = defaultColor;
    }

    public void OnPointerDown() {
        baseTile.NotifyToEvacuate();
    }

    public void SetBaseTile(Tile tile) {
        baseTile = tile;
    }

    public void Relocate(Tile newTile) {
        transform.position = newTile.transform.position;
        transform.SetParent(newTile.transform);
        baseTile = newTile;

        cooldown = evacuationCooldown;
        uiHandler.SetValue(cooldown);

        meshRenderer.enabled = false;
    }

    private void ProgressCooldown() {
        if (cooldown > 0) {
            cooldown--;
            uiHandler.SetValue(cooldown);
            if (cooldown == 0) {
                baseTile.NotifySuccesfulEvacuation();
                meshRenderer.enabled = true;
            }
        }
    }

}
