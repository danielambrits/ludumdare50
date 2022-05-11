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

    [Header("Radiation Algorithm")]
    [SerializeField]
    private int emptyTileEdgeScore = 2;
    [SerializeField]
    private int emptyTileCornerScore = 1;
    [SerializeField]
    private int wallTileEdgeScore = 2;
    [SerializeField]
    private int wallTileCornerScore = 1;
    [SerializeField]
    private int totalScoreThreshold = 19;

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
        currentDangerZonePositions.Add(new Vector2Int(1, 0));
        currentDangerZonePositions.Add(new Vector2Int(-1, 0));
        currentDangerZonePositions.Add(new Vector2Int(0, 1));
        currentDangerZonePositions.Add(new Vector2Int(0, -1));

        foreach(var position in currentDangerZonePositions) {
            TryToTagAsDangerZone(position);
        }
        
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

            }
        }

        for (int x = startingPosition.x - iterationCounter - 1; x <= startingPosition.x + iterationCounter + 1; x++) {
            for (int y = startingPosition.y - iterationCounter - 1; y <= startingPosition.y + iterationCounter + 1; y++) {
                GridWithData.CellData cellData;
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (grid.GetCellData(gridPosition, out cellData)) {
                    GridWithData.CellData tempCellData;
                    int score = 0;
                    int edgeScore = emptyTileEdgeScore;
                    int cornerScore = emptyTileCornerScore;

                    if (cellData.tile.type == Tile.Type.Wall) {
                        edgeScore = wallTileEdgeScore;
                        cornerScore = wallTileCornerScore;
                    }

                    if (edgeScore != 0) {
                        Vector2Int[] edgeNeighbourPositions = new Vector2Int[4];
                        edgeNeighbourPositions[0] = new Vector2Int(x - 1, y);
                        edgeNeighbourPositions[1] = new Vector2Int(x + 1, y);
                        edgeNeighbourPositions[2] = new Vector2Int(x, y - 1);
                        edgeNeighbourPositions[3] = new Vector2Int(x, y + 1);
                        foreach (var position in edgeNeighbourPositions) {
                            if (grid.GetCellData(position, out tempCellData)) {
                                if (tempCellData.tile.isCorrupted) {
                                    score += edgeScore;
                                }
                            }
                        }
                    }
                    if (cornerScore != 0) {
                        Vector2Int[] cornerNeighbourPositions = new Vector2Int[4];
                        cornerNeighbourPositions[0] = new Vector2Int(x - 1, y - 1);
                        cornerNeighbourPositions[1] = new Vector2Int(x + 1, y - 1);
                        cornerNeighbourPositions[2] = new Vector2Int(x - 1, y + 1);
                        cornerNeighbourPositions[3] = new Vector2Int(x + 1, y + 1);
                        foreach (var position in cornerNeighbourPositions) {
                            if (grid.GetCellData(position, out tempCellData)) {
                                if (tempCellData.tile.isCorrupted) {
                                    score += cornerScore;
                                }
                            }
                        }
                    }
                
                    cellData.corruptionScore = score;
                    grid.SetCellData(gridPosition, cellData);
                    
                }
                
            }
        }

        for (int x = startingPosition.x - iterationCounter - 1; x <= startingPosition.x + iterationCounter + 1; x++) {
            for (int y = startingPosition.y - iterationCounter - 1; y <= startingPosition.y + iterationCounter + 1; y++) {
                GridWithData.CellData cellData;
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (grid.GetCellData(gridPosition, out cellData)) {
                    int totalScore = 0;

                    if (cellData.tile.type == Tile.Type.Wall) {
                        continue;
                    }
                    if (cellData.tile.isCorrupted) {
                        continue;
                    }

                    Vector2Int[] neighbourPositions = new Vector2Int[8];
                    neighbourPositions[0] = new Vector2Int(x - 1, y);
                    neighbourPositions[1] = new Vector2Int(x + 1, y);
                    neighbourPositions[2] = new Vector2Int(x, y - 1);
                    neighbourPositions[3] = new Vector2Int(x, y + 1);
                    neighbourPositions[4] = new Vector2Int(x - 1, y - 1);
                    neighbourPositions[5] = new Vector2Int(x + 1, y - 1);
                    neighbourPositions[6] = new Vector2Int(x - 1, y + 1);
                    neighbourPositions[7] = new Vector2Int(x + 1, y + 1);

                    foreach (var position in neighbourPositions) {
                        if (grid.GetCellData(position, out cellData)) {
                            totalScore += cellData.corruptionScore;
                        }
                    }
                    
                    if (totalScore >= totalScoreThreshold) {
                        if (TryToTagAsDangerZone(gridPosition)) {
                            nextDangerZonePositions.Add(gridPosition);
                        }
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

        int maxIterations = 2000; // just in case...

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
                    inspectablePositions.Push(new Vector2Int(gridPosition.x - 1, gridPosition.y - 1));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x + 1, gridPosition.y - 1));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x - 1, gridPosition.y + 1));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x + 1, gridPosition.y + 1));
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
