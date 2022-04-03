using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private CorruptionHandler corruptionHandler;

    [SerializeField]
    private TextMeshProUGUI remainingWallUi;
    [SerializeField]
    private TextMeshProUGUI gameEndUi;

    private int houseCount;
    private int factoryCount;

    private int remainingWallCount;

    public static UnityEvent OnCorruptionTurnEnded = new UnityEvent();

    void Awake() {
        houseCount = 0;
        factoryCount = 0;
        remainingWallCount = GetPlacableWallCount();
    }

    void OnEnable() {
        GroundTile.OnWallBuilt.AddListener(OnWallBuilt);
        GroundTile.OnHouseBuilt.AddListener(OnHouseBuilt);
        GroundTile.OnHouseDestroyed.AddListener(OnHouseDestroyed);
        GroundTile.OnDelayedHouseBuilt.AddListener(TriggerNextTurn);
        GroundTile.OnFactoryBuilt.AddListener(OnFactoryBuilt);
        GroundTile.OnFactoryDestroyed.AddListener(OnFactoryDestroyed);
        GroundTile.OnDelayedFactoryBuilt.AddListener(TriggerNextTurn);
        GroundTile.OnHelicopterActivated.AddListener(OnHelicopterUsed);
    }

    void OnDisable() {
        GroundTile.OnWallBuilt.RemoveListener(OnWallBuilt);
        GroundTile.OnHouseBuilt.RemoveListener(OnHouseBuilt);
        GroundTile.OnHouseDestroyed.RemoveListener(OnHouseDestroyed);
        GroundTile.OnDelayedHouseBuilt.RemoveListener(TriggerNextTurn);
        GroundTile.OnFactoryBuilt.RemoveListener(OnFactoryBuilt);
        GroundTile.OnFactoryDestroyed.RemoveListener(OnFactoryDestroyed);
        GroundTile.OnDelayedFactoryBuilt.RemoveListener(TriggerNextTurn);
        GroundTile.OnHelicopterActivated.RemoveListener(OnHelicopterUsed);
    }

    private void OnWallBuilt() {
        if (corruptionHandler.CheckWallDone()) {
            // TODO move
            gameEndUi.text = "SUCCESS";
        } else {
            remainingWallCount--;
            remainingWallUi.text = string.Format("Buildable walls: {0}", remainingWallCount);
            if (remainingWallCount <= 0) {
                TriggerNextTurn();
            }
        }
    }

    private void OnHelicopterUsed() {
        corruptionHandler.AddDelay();
        TriggerNextTurn();
    }

    private void TriggerNextTurn() {
        corruptionHandler.Apply();
        remainingWallCount = GetPlacableWallCount();
        if (remainingWallCount == 0) {
            gameEndUi.text = "GAME OVER";
            // TODO trigger game over state
        } else {
            remainingWallUi.text = string.Format("Buildable walls: {0}", remainingWallCount);
            Debug.Log(string.Format("Builadbe walls this turn: {0}", remainingWallCount));
            OnCorruptionTurnEnded.Invoke();
        }
    }

    private void OnHouseBuilt() {
        houseCount++;
    }
    
    private void OnFactoryBuilt() {
        factoryCount++;
    }

    private void OnHouseDestroyed() {
        if (houseCount > 0) {
            houseCount--;
        }
    }

    private void OnFactoryDestroyed() {
        if (factoryCount > 0) {
            factoryCount--;
        }
    }

    private int GetPlacableWallCount() {
        return Mathf.Min(houseCount, factoryCount);
    }

}
