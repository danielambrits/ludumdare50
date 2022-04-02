using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridWithData : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    public Vector2Int size;

    [SerializeField]
    private GameObject groundTilePrefab;

    // TODO generate these
    [SerializeField]
    public Vector2Int[] housePositions;
    [SerializeField]
    public Vector2Int[] factoryPositions;
    [SerializeField]
    public Vector2Int[] helipadPositions;

    public struct CellData {
        public bool isInspected;
        public GroundTile tile;
    }

    private Dictionary<Vector2Int, CellData> gridMap = new Dictionary<Vector2Int, CellData>();

    void Start() {
        for (int i = -size.x/2; i <= size.x/2; i++) {
            for (int j = -size.y/2; j <= size.y/2; j++) {
                GameObject tile = Instantiate(groundTilePrefab, grid.CellToWorld(new Vector3Int(i, j, 0)), Quaternion.Euler(90, 0, 0));
                tile.transform.SetParent(transform);
                Vector2Int gridPosition = new Vector2Int(i, j);
                CellData cellData;
                cellData.isInspected = true;
                cellData.tile = tile.GetComponent<GroundTile>();
                gridMap.Add(gridPosition, cellData);
            }
        }

        foreach (var position in housePositions) {
            gridMap[position].tile.BuildHouse();
        }
        foreach (var position in factoryPositions) {
            gridMap[position].tile.BuildFactory();
        }
        foreach (var position in helipadPositions) {
            gridMap[position].tile.BuildHelipad();
        }
    }

    public bool GetCellData(Vector2Int gridPosition, out CellData cellData) {
        return gridMap.TryGetValue(gridPosition, out cellData);
    }

    public void SetCellData(Vector2Int gridPosition, CellData cellData) {
        gridMap[gridPosition] = cellData;
        if (cellData.tile.isCorrupted) {
            cellData.tile.Corrupt();
        }
    }

    public void ClearAllInspection() {
        foreach (var key in gridMap.Keys.ToList()) {
            CellData cellData = gridMap[key];
            cellData.isInspected = false;
            gridMap[key] = cellData;
        }
    }

}
