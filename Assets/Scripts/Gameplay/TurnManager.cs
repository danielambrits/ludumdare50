using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private CorruptionHandler corruptionHandler;

    [Header("Settings")]
    [SerializeField]
    private int placableWallCount;

    private int remainingWallCount;

    void Awake() {
        remainingWallCount = placableWallCount;
    }

    void OnEnable() {
        GroundTile.OnWallBuild.AddListener(OnWallBuild);
    }

    void OnDisable() {
        GroundTile.OnWallBuild.RemoveListener(OnWallBuild);
    }

    private void OnWallBuild() {
        corruptionHandler.CheckWallDone();
        remainingWallCount--;
        if (remainingWallCount <= 0) {
            remainingWallCount = placableWallCount;
            corruptionHandler.Apply();
        }
    }

}
