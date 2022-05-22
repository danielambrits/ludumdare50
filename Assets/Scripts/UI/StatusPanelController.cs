using UnityEngine;

public class StatusPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject evacuationStatusUi;
    [SerializeField]
    private GameObject helicopterStatusUi;

    void OnEnable() {
        Tile.OnWallBuilt.AddListener(() => {
            evacuationStatusUi.SetActive(false);
            helicopterStatusUi.SetActive(false);
        });
        TurnManager.OnCorruptionTurnEnded.AddListener(() => {
            evacuationStatusUi.SetActive(true);
            helicopterStatusUi.SetActive(true && (Helipad.overallCount > 0) && (Helipad.inCooldownCount < Helipad.overallCount));
        });
        Helipad.OnHelipadCountChange.AddListener(() => {
            helicopterStatusUi.SetActive(true && (Helipad.overallCount > 0) && (Helipad.inCooldownCount < Helipad.overallCount));
        });
    }

    void OnDisable() {
        Tile.OnWallBuilt.RemoveListener(() => {
            evacuationStatusUi.SetActive(false);
            helicopterStatusUi.SetActive(false);
        });
        TurnManager.OnCorruptionTurnEnded.RemoveListener(() => {
            evacuationStatusUi.SetActive(true);
            helicopterStatusUi.SetActive(true && (Helipad.overallCount > 0) && (Helipad.inCooldownCount < Helipad.overallCount));
        });
        Helipad.OnHelipadCountChange.RemoveListener(() => {
            helicopterStatusUi.SetActive(true && (Helipad.overallCount > 0) && (Helipad.inCooldownCount < Helipad.overallCount));
        });
    }

}
