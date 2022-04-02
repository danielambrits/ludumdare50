using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private CorruptionHandler corruptionHandler;

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
        GroundTile.OnFactoryBuilt.AddListener(OnFactoryBuilt);
        GroundTile.OnFactoryDestroyed.AddListener(OnFactoryDestroyed);
        GroundTile.OnHelicopterActivated.AddListener(OnHelicopterUsed);
    }

    void OnDisable() {
        GroundTile.OnWallBuilt.RemoveListener(OnWallBuilt);
        GroundTile.OnHouseBuilt.RemoveListener(OnHouseBuilt);
        GroundTile.OnHouseDestroyed.RemoveListener(OnHouseDestroyed);
        GroundTile.OnFactoryBuilt.RemoveListener(OnFactoryBuilt);
        GroundTile.OnFactoryDestroyed.RemoveListener(OnFactoryDestroyed);
        GroundTile.OnHelicopterActivated.RemoveListener(OnHelicopterUsed);
    }

    private void OnWallBuilt() {
        corruptionHandler.CheckWallDone();
        remainingWallCount--;
        if (remainingWallCount <= 0) {
            TriggerNextTurn();
        }
    }

    private void OnHelicopterUsed() {
        corruptionHandler.AddDelay();
        Debug.Log("Helicopter brrr");
        TriggerNextTurn();
    }

    private void TriggerNextTurn() {
        corruptionHandler.Apply();
        remainingWallCount = GetPlacableWallCount();
        if (remainingWallCount == 0) {
            Debug.Log("GAME OVER");
            // TODO trigger game over state
        } else {
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
