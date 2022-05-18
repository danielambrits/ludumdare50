using UnityEngine;

public class HelpPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject cancelHelpUi;

    void OnEnable() {
        WallResourceBuilding.OnEvacuationStarted.AddListener(() => cancelHelpUi?.SetActive(true));
        WallResourceBuilding.OnEvacuationCancelled.AddListener(() => cancelHelpUi?.SetActive(false));
    }

    void OnDisable() {
        WallResourceBuilding.OnEvacuationStarted.RemoveListener(() => cancelHelpUi?.SetActive(true));
        WallResourceBuilding.OnEvacuationCancelled.RemoveListener(() => cancelHelpUi?.SetActive(false));
    }

}
