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
    private TextMeshProUGUI evacuationUi;
    [SerializeField]
    private GameObject gameEndPanelUi;
    [SerializeField]
    private TextMeshProUGUI gameEndTextUi;
    [SerializeField]
    private TextMeshProUGUI gameFailureReasonTextUi;

    private int houseCount;
    private int factoryCount;

    private int buildableWallCount;
    private int remainingWallCount;

    public static UnityEvent OnCorruptionTurnEnded = new UnityEvent();
    public static UnityEvent OnPlayerTurnEnded = new UnityEvent();

    public static UnityEvent OnGameWon = new UnityEvent();
    public static UnityEvent OnGameLost = new UnityEvent();

    public static bool wallAlreadyBuiltIntHisTurn;

    void Awake() {
        houseCount = 0;
        factoryCount = 0;
        wallAlreadyBuiltIntHisTurn = false;
        Helipad.overallCount = 0; // TODO better place?
        Helipad.inCooldownCount = 0;
    }

    void Start() {
        buildableWallCount = GetBuildableWallCount();
        remainingWallCount = buildableWallCount;
        remainingWallUi.text = string.Format("Buildable walls: {0}/{1}", remainingWallCount, buildableWallCount);
    }

    void OnEnable() {
        Tile.OnWallBuilt.AddListener(OnWallBuilt);
        Tile.OnHouseBuilt.AddListener(OnHouseBuilt);
        Tile.OnHouseDestroyed.AddListener(OnHouseDestroyed);
        Tile.OnDelayedHouseBuilt.AddListener(OnEvacuationStarted);
        Tile.OnFactoryBuilt.AddListener(OnFactoryBuilt);
        Tile.OnFactoryDestroyed.AddListener(OnFactoryDestroyed);
        Tile.OnDelayedFactoryBuilt.AddListener(OnEvacuationStarted);
        Helipad.OnHelicopterActivated.AddListener(OnHelicopterUsed);
    }

    void OnDisable() {
        Tile.OnWallBuilt.RemoveListener(OnWallBuilt);
        Tile.OnHouseBuilt.RemoveListener(OnHouseBuilt);
        Tile.OnHouseDestroyed.RemoveListener(OnHouseDestroyed);
        Tile.OnDelayedHouseBuilt.RemoveListener(OnEvacuationStarted);
        Tile.OnFactoryBuilt.RemoveListener(OnFactoryBuilt);
        Tile.OnFactoryDestroyed.RemoveListener(OnFactoryDestroyed);
        Tile.OnDelayedFactoryBuilt.RemoveListener(OnEvacuationStarted);
        Helipad.OnHelicopterActivated.RemoveListener(OnHelicopterUsed);
    }

    private void OnWallBuilt() {
        wallAlreadyBuiltIntHisTurn = true;
        CorruptionHandler.WallState wallState = corruptionHandler.CheckWallDone();
        switch (wallState) {
            case CorruptionHandler.WallState.Done:
                ShowUiOnVictory();
                playerInput.enabled = false;
                OnGameWon.Invoke();
                break;
            case CorruptionHandler.WallState.Unsolvable:
                playerInput.enabled = false;
                ShowUiOnFailure("Radiation can not be surrounded anymore.");
                OnGameLost.Invoke();
                break;
            default:
                remainingWallCount--;
                remainingWallUi.text = string.Format("Buildable walls: {0}/{1}", remainingWallCount, buildableWallCount);
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
            ShowUiOnFailure("Radiation reached the edge of the area.");
            playerInput.enabled = false;
            OnGameLost.Invoke();
            return;
        }

        OnCorruptionTurnEnded.Invoke();

        buildableWallCount = GetBuildableWallCount();
        if (buildableWallCount == 0) {
            ShowUiOnFailure("All resources for walls are destroyed.");
            playerInput.enabled = false;
            OnGameLost.Invoke();
        } else {
            remainingWallCount = buildableWallCount;
            remainingWallUi.text = string.Format("Buildable walls: {0}/{1}", remainingWallCount, buildableWallCount);
            Debug.Log(string.Format("Buildabe walls this turn: {0}", buildableWallCount));
            wallAlreadyBuiltIntHisTurn = false;
            playerInput.enabled = true;
        }
    }

    private void OnHouseBuilt() {
        houseCount++;
        evacuationUi.text = "Evacuation AVAILABLE";
        Debug.Log(string.Format("HOUSE BUILT: {0}", houseCount));
    }
    
    private void OnFactoryBuilt() {
        factoryCount++;
        evacuationUi.text = "Evacuation AVAILABLE";
        Debug.Log(string.Format("FACTORY BUILT: {0}", factoryCount));
    }

    private void OnHouseDestroyed() {
        if (houseCount > 0) {
            houseCount--;
        }
        Debug.Log(string.Format("HOUSE DESTROYED: {0}", houseCount));
    }

    private void OnFactoryDestroyed() {
        if (factoryCount > 0) {
            factoryCount--;
        }
        Debug.Log(string.Format("FACTORY DESTROYED: {0}", factoryCount));
    }

    private void OnEvacuationStarted() {
        evacuationUi.text = "Evacuation in progress...";
        TriggerNextTurn();
    }

    private int GetBuildableWallCount() {
        return Mathf.Min(houseCount, factoryCount);
    }

    private void ShowUiOnVictory() {
        gameEndPanelUi.SetActive(true);
        gameEndTextUi.text = "SUCCESS";
        gameFailureReasonTextUi.text = "";
    }

    private void ShowUiOnFailure(string reason) {
        gameEndPanelUi.SetActive(true);
        gameEndTextUi.text = "GAME OVER";
        gameFailureReasonTextUi.text = reason;
    }

}
