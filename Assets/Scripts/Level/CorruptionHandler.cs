using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CorruptionHandler : MonoBehaviour
{
    [SerializeField]
    private GridWithData grid;
    [SerializeField]
    private Vector2Int startingPosition;
    [SerializeField]
    private int initialSpreadCount;
    [SerializeField]
    private int spreadPeriod;
    [SerializeField]
    private int spreadDelayLength;

    [Header("UI")]
    [SerializeField]
    private OvertextHandler uiHandler;

    public enum WallState {
        Done,
        InProgress,
        Unsolvable,
        AlgorithmError
    }

    public enum Result {
        Waiting,
        Success,
        EdgeReached
    }

    public static UnityEvent OnSpread = new UnityEvent();

    private int spreadDelayCounter;
    private int iterationCounter;
    private int corruptedCellCount;

    private Stack<Vector2Int> inspectablePositions = new Stack<Vector2Int>();
    private List<Vector2Int> gridCorners = new List<Vector2Int>();
    
    private List<Vector2Int> currentDangerZonePositions = new List<Vector2Int>();
    private List<Vector2Int> nextDangerZonePositions = new List<Vector2Int>();

    void Start() {
        iterationCounter = 0;
        spreadDelayCounter = spreadPeriod;

        corruptedCellCount = 0;

        gridCorners.Add(new Vector2Int(grid.size.x/2, grid.size.y/2));
        gridCorners.Add(new Vector2Int(-grid.size.x/2, grid.size.y/2));
        gridCorners.Add(new Vector2Int(grid.size.x/2, -grid.size.y/2));
        gridCorners.Add(new Vector2Int(-grid.size.x/2, -grid.size.y/2));

        currentDangerZonePositions.Add(new Vector2Int(0, 0));

        for (int i = 0; i < initialSpreadCount; i++) {
            Spread();
        }
    }

    public Result Apply() {
        spreadDelayCounter--;
        uiHandler.SetValue(spreadDelayCounter);
        if (spreadDelayCounter > 0) {
            Debug.Log("Nuclear spread in " + spreadDelayCounter);
            return Result.Waiting;
        }

        return Spread();
    }

    private Result Spread() {
        spreadDelayCounter = spreadPeriod;
        iterationCounter++;
        uiHandler.SetValue(spreadDelayCounter);
        Debug.Log("Spread! iteration " + iterationCounter);
        bool edgeReached = false;

        nextDangerZonePositions.Clear();
        foreach (var gridPosition in currentDangerZonePositions) {
            GridWithData.CellData cellData;
            if (grid.GetCellData(gridPosition, out cellData)) {
                if (cellData.tile.type == Tile.Type.Wall) {
                    continue;
                }
                if (cellData.tile.isCorrupted) {
                    continue;
                }
                cellData.tile.Corrupt();
                corruptedCellCount++;
         
                if ((gridPosition.x <= -grid.size.x/2) ||
                    (gridPosition.x >= grid.size.x/2) ||
                    (gridPosition.y <= -grid.size.y/2) ||
                    (gridPosition.y >= grid.size.y/2)) {
                    edgeReached = true;
                }

                Vector2Int[] neighbourPositions = new Vector2Int[4];
                neighbourPositions[0] = new Vector2Int(gridPosition.x - 1, gridPosition.y);
                neighbourPositions[1] = new Vector2Int(gridPosition.x + 1, gridPosition.y);
                neighbourPositions[2] = new Vector2Int(gridPosition.x, gridPosition.y - 1);
                neighbourPositions[3] = new Vector2Int(gridPosition.x, gridPosition.y + 1);
                                
                foreach (var neighbourPosition in neighbourPositions) {
                    if (TryToTagAsDangerZone(neighbourPosition)) {
                        nextDangerZonePositions.Add(neighbourPosition);
                    }
                }

            }
        }
        currentDangerZonePositions = new List<Vector2Int>(nextDangerZonePositions);
        
        OnSpread.Invoke();

        return (edgeReached) ? Result.EdgeReached : Result.Success;
    }

    private bool TryToTagAsDangerZone(Vector2Int gridPosition) {
        GridWithData.CellData cellData;
        if (grid.GetCellData(gridPosition, out cellData)) {
            if (cellData.tile.type == Tile.Type.Wall) {
                return false;
            }
            if (cellData.tile.isCorrupted) {
                return false;
            }
            cellData.tile.TagAsDangerZone();
            return true;
        }
        return false;
    }

    public void AddDelay() {
        spreadDelayCounter += spreadDelayLength;
    }

    public WallState CheckWallDone() {
        // Implements a Flood Fill algorithm from the starting position.
        grid.ClearAllInspection();

        inspectablePositions.Clear();
        inspectablePositions.Push(startingPosition);

        int foundCorruptedCells = 0;

        int maxIterations = 1000; // just in case...

        while (inspectablePositions.Count > 0) {
            if (maxIterations-- < 0) {
                Debug.LogWarning("Flood Fill took too may iterations.");
                return WallState.AlgorithmError;
            }

            Vector2Int gridPosition = inspectablePositions.Pop();

            // If we reached any of the corners, it is impossible that the wall is built in a closed shape
            foreach (var corner in gridCorners) {
                if (corner == gridPosition) {
                    inspectablePositions.Clear();
                    return WallState.InProgress;
                }
            }

            GridWithData.CellData cellData;
            if (grid.GetCellData(gridPosition, out cellData)) {
                if (cellData.isInspected) {
                    continue;
                }
                if (cellData.tile.isCorrupted) {
                    foundCorruptedCells++;
                }
                cellData.isInspected = true;
                grid.SetCellData(gridPosition, cellData);
                if (cellData.tile.type != Tile.Type.Wall) {
                    inspectablePositions.Push(new Vector2Int(gridPosition.x - 1, gridPosition.y));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x + 1, gridPosition.y));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x, gridPosition.y - 1));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x, gridPosition.y + 1));
                }
            }
        }

        // Corners were not reached -> the wall is done
        inspectablePositions.Clear();

        if (foundCorruptedCells != corruptedCellCount) {
            // if not all the corrupted cells are found, there must be some outside the wall
            return WallState.Unsolvable;
        }
        return WallState.Done;     
    }

}
