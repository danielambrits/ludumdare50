using UnityEngine;
using System.Collections.Generic;

public class CorruptionHandler : MonoBehaviour
{
    [SerializeField]
    private GridWithData grid;
    [SerializeField]
    private Vector2Int startingPosition;
    [SerializeField]
    private int spreadPeriod;
    [SerializeField]
    private int spreadDelayLength;

    [Header("UI")]
    [SerializeField]
    private OvertextHandler uiHandler;

    private int spreadDelayCounter;
    private int iterationCounter;

    private Stack<Vector2Int> inspectablePositions = new Stack<Vector2Int>();
    private List<Vector2Int> gridCorners = new List<Vector2Int>();

    void Start() {
        iterationCounter = 0;
        spreadDelayCounter = spreadPeriod;

        gridCorners.Add(new Vector2Int(grid.size.x/2, grid.size.y/2));
        gridCorners.Add(new Vector2Int(-grid.size.x/2, grid.size.y/2));
        gridCorners.Add(new Vector2Int(grid.size.x/2, -grid.size.y/2));
        gridCorners.Add(new Vector2Int(-grid.size.x/2, -grid.size.y/2));
    }

    public void Apply() {
        spreadDelayCounter--;
        uiHandler.SetValue(spreadDelayCounter);
        if (spreadDelayCounter > 0) {
            Debug.Log("Nuclear spread in " + spreadDelayCounter);
            return;
        }

        spreadDelayCounter = spreadPeriod;
        iterationCounter++;

        for (int x = startingPosition.x - iterationCounter; x <= startingPosition.x + iterationCounter; x++) {
            for (int y = startingPosition.y - iterationCounter; y <= startingPosition.y + iterationCounter; y++) {
                if ((x - startingPosition.x) * (x - startingPosition.x) + (y - startingPosition.y) * (y - startingPosition.y) < iterationCounter * iterationCounter) {
                    Vector2Int gridPosition = new Vector2Int(x,y);
                    GridWithData.CellData cellData;
                    if (grid.GetCellData(gridPosition, out cellData)) {
                        if (!cellData.tile.isCorrupted) {
                            cellData.tile.Corrupt();
                        }
                    }
                }
            }
        }
    }

    public void AddDelay() {
        spreadDelayCounter += spreadDelayLength;
    }

    public bool CheckWallDone() {
        // Implements a Flood Fill algorithm from the starting position.
        grid.ClearAllInspection();

        inspectablePositions.Clear();
        inspectablePositions.Push(startingPosition);

        int maxIterations = 1000; // just in case...

        while (inspectablePositions.Count > 0) {
            if (maxIterations-- < 0) {
                Debug.LogWarning("Flood Fill took too may iterations.");
                return false;
            }

            Vector2Int gridPosition = inspectablePositions.Pop();

            // If we reached any of the corners, it is impossible that the wall is built in a closed shape
            foreach (var corner in gridCorners) {
                if (corner == gridPosition) {
                    inspectablePositions.Clear();
                    return false;
                }
            }

            GridWithData.CellData cellData;
            if (grid.GetCellData(gridPosition, out cellData)) {
                if (cellData.isInspected) {
                    continue;
                }
                cellData.isInspected = true;
                grid.SetCellData(gridPosition, cellData);
                if (cellData.tile.type != GroundTile.Type.Wall) {
                    inspectablePositions.Push(new Vector2Int(gridPosition.x - 1, gridPosition.y));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x + 1, gridPosition.y));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x, gridPosition.y - 1));
                    inspectablePositions.Push(new Vector2Int(gridPosition.x, gridPosition.y + 1));
                }
            }
        }

        // Corners were not reached -> the wall is done
        inspectablePositions.Clear();
        return true;     
    }

}
