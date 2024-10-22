using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static readonly int[,] Directions = 
    {
        { -1, -1 }, { -1, 1 }, { -1, 0 },
        { 1, -1 }, { 1, 1 }, { 1, 0 },
        { 0, 1 }, { 0, -1 }
    };

    private static Vector2Int _gridSize;
    private static float _cellSize;

    public static void Instantiate(int width, int height, float cellSize)
    {
        _gridSize = new Vector2Int(width, height);
        _cellSize = cellSize;
    }

    public static Vector3 GetWorldCellPosition(int x, int y)
    {
        return new Vector3(x * _cellSize, y * _cellSize, 0);
    }

    public static Vector2Int GetCellCoordinates(Vector3 position)
    {
        int cellX = Mathf.FloorToInt(position.x / _cellSize);
        int cellY = Mathf.FloorToInt(position.y / _cellSize);
        return new Vector2Int(cellX, cellY);
    }

    public static bool IsAllowableSquare(Vector3 position)
    {
        Vector2Int cell = GetCellCoordinates(position);
        return IsAllowableSquare(cell.x, cell.y);
    }

    public static bool IsAllowableSquare(int x, int y)
    {
        return x >= 0 && x < _gridSize.x && y >= 0 && y < _gridSize.y;
    }

    public static List<Vector2Int> GetNeighborCells(Vector2Int cell)
    {
        var neighbors = new List<Vector2Int>();

        for (int i = 0; i < Directions.GetLength(0); i++)
        {
            int dx = Directions[i, 0];
            int dy = Directions[i, 1];

            Vector2Int neighbor = new Vector2Int(cell.x + dx, cell.y + dy);
            if (IsAllowableSquare(neighbor.x, neighbor.y))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
