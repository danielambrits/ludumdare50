using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private CorruptionHandler corruptionHandler;
    [SerializeField]
    private PointerInputHandler playerInput;

    [SerializeField]
    private TextMeshProUGUI remainingWallUi;
    [SerializeField]
    private GameObject gameEndPanelUi;
    [SerializeField]
    private TextMeshProUGUI gameEndTextUi;

    private int houseCount;
    private int factoryCount;

    private int remainingWallCount;

    public static UnityEvent OnCorruptionTurnEnded = new UnityEvent();
    public static UnityEvent OnPlayerTurnEnded = new UnityEvent();

    public static UnityEvent OnGameWon = new UnityEvent();
    public static UnityEvent OnGameLost = new UnityEvent();

    void Awake() {
        houseCount = 0;
        factoryCount = 0;
        remainingWallCount = GetPlacableWallCount();
    }

    void OnEnable() {
        Tile.OnWallBuilt.AddListener(OnWallBuilt);
        Tile.OnHouseBuilt.AddListener(OnHouseBuilt);
        Tile.OnHouseDestroyed.AddListener(OnHouseDestroyed);
        Tile.OnDelayedHouseBuilt.AddListener(TriggerNextTurn);
        Tile.OnFactoryBuilt.AddListener(OnFactoryBuilt);
        Tile.OnFactoryDestroyed.AddListener(OnFactoryDestroyed);
        Tile.OnDelayedFactoryBuilt.AddListener(TriggerNextTurn);
        Helipad.OnHelicopterActivated.AddListener(OnHelicopterUsed);
    }

    void OnDisable() {
        Tile.OnWallBuilt.RemoveListener(OnWallBuilt);
        Tile.OnHouseBuilt.RemoveListener(OnHouseBuilt);
        Tile.OnHouseDestroyed.RemoveListener(OnHouseDestroyed);
        Tile.OnDelayedHouseBuilt.RemoveListener(TriggerNextTurn);
        Tile.OnFactoryBuilt.RemoveListener(OnFactoryBuilt);
        Tile.OnFactoryDestroyed.RemoveListener(OnFactoryDestroyed);
        Tile.OnDelayedFactoryBuilt.RemoveListener(TriggerNextTurn);
        Helipad.OnHelicopterActivated.RemoveListener(OnHelicopterUsed);
    }

    private void OnWallBuilt() {
        CorruptionHandler.WallState wallState = corruptionHandler.CheckWallDone();
        switch (wallState) {
            case CorruptionHandler.WallState.Done:
                ShowUiOnVictory();
                playerInput.enabled = false;
                OnGameWon.Invoke();
                break;
            case CorruptionHandler.WallState.Unsolvable:
                playerInput.enabled = false;
                ShowUiOnFailure();
                OnGameLost.Invoke();
                break;
            default:
                remainingWallCount--;
                remainingWallUi.text = string.Format("Buildable walls: {0}", remainingWallCount);
                if (remainingWallCount <= 0) {
                    TriggerNextTurn();
                }
                break;
        }
    }

    private void OnHelicopterUsed() {
        corruptionHandler.AddDelay();
        TriggerNextTurn();
    }

    private void TriggerNextTurn() {
        playerInput.enabled = false;
        OnPlayerTurnEnded.Invoke();
        if (corruptionHandler.Apply() == CorruptionHandler.Result.EdgeReached) {
            ShowUiOnFailure();
            playerInput.enabled = false;
            OnGameLost.Invoke();
        }
        remainingWallCount = GetPlacableWallCount();
        if (remainingWallCount == 0) {
            ShowUiOnFailure();
            playerInput.enabled = false;
            OnGameLost.Invoke();
        } else {
            remainingWallUi.text = string.Format("Buildable walls: {0}", remainingWallCount);
            Debug.Log(string.Format("Builadbe walls this turn: {0}", remainingWallCount));
            OnCorruptionTurnEnded.Invoke();
            playerInput.enabled = true;
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

    private void ShowUiOnVictory() {
        gameEndPanelUi.SetActive(true);
        gameEndTextUi.text = "SUCCESS";
    }

    private void ShowUiOnFailure() {
        gameEndPanelUi.SetActive(true);
        gameEndTextUi.text = "GAME OVER";
    }

}
