using UnityEngine;
using System.Collections.Generic;

public class SquaresManager : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject gameManagerObject;
    
    private GameManager _gameManager;
    private Square[,] _squaresArray;
    private List<Square> _targetedSquaresList;
    private int _width, _height;
    private float _squareSize;
    private static float _timer;

    private void Start()
    {
        _gameManager = gameManagerObject.GetComponent<GameManager>();

        _width = _gameManager.GetWidth();
        _height = _gameManager.GetHeight();
        _squareSize = _gameManager.GetCellSize();

        _squaresArray = new Square[_width, _height];
        _targetedSquaresList = new List<Square>();

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var position = Utils.GetWorldCellPosition(i, j) + new Vector3(_squareSize, _squareSize) / 2f;
                _squaresArray[i, j] = new Square(SpawnSquare(position, _squareSize));
            }
        }
    }

    private void UpdateSquares()
    {
        for (var x = 0; x < _width; ++x)
        {
            for (var y = 0; y < _height; ++y)
            {
                var neighbours = CountOfNeighbours(x, y);
                if (IsActiveSquare(x, y) && (neighbours != 2 && neighbours != 3))
                {
                    _squaresArray[x, y].Deactivate();
                }

                if (!IsActiveSquare(x, y) && neighbours == 3)
                {
                    _squaresArray[x, y].Activate();
                }
            }
        }

        foreach (var square in _squaresArray)
        {
            square.Update(true);
        }
    }

    private void UpdateSquareOnClick(List<Vector2Int> squaresToUpdatePositions, bool activate)
    {
        foreach (var squarePositions in squaresToUpdatePositions)
        {
            if (!Utils.IsAllowableSquare(squarePositions.x, squarePositions.y)) continue;

            if (activate)
                _squaresArray[squarePositions.x, squarePositions.y].Activate();
            else
                _squaresArray[squarePositions.x, squarePositions.y].Deactivate();
        }

        // Обновляем после всех изменений
        foreach (var square in squaresToUpdatePositions)
        {
            _squaresArray[square.x, square.y].Update();
        }
    }

    private void ClearGrid()
    {
        for (var x = 0; x < _width; ++x)
        {
            for (var y = 0; y < _height; ++y)
            {
                _squaresArray[x, y].Deactivate();
                _squaresArray[x, y].Update();
            }
        }
    }

    private void RandomFillGrid()
    {
        var random = new System.Random();
        for (var x = 0; x < _width; ++x)
        {
            for (var y = 0; y < _height; ++y)
            {
                if (random.Next(3) == 0)
                    _squaresArray[x, y].Activate();
                else
                    _squaresArray[x, y].Deactivate();

                _squaresArray[x, y].Update();
            }
        }
    }

    private void UpdateTargetSquare(List<Vector2Int> squaresToTargetPositions)
    {
        foreach (var square in _targetedSquaresList)
        {
            square.Untarget();
        }
        _targetedSquaresList.Clear();

        foreach (var squarePosition in squaresToTargetPositions)
        {
            if (!Utils.IsAllowableSquare(squarePosition.x, squarePosition.y)) continue;

            _squaresArray[squarePosition.x, squarePosition.y].Target();
            _targetedSquaresList.Add(_squaresArray[squarePosition.x, squarePosition.y]);
        }
    }

    private int CountOfNeighbours(int x, int y)
    {
        var neighbours = 0;
        for (var i = 0; i < Utils.Directions.GetLength(0); i++)
        {
            neighbours += IsActiveSquare(x + Utils.Directions[i, 0], y + Utils.Directions[i, 1]) ? 1 : 0;
        }
        return neighbours;
    }

    private bool IsActiveSquare(int x, int y)
    {
        return Utils.IsAllowableSquare(x, y) && _squaresArray[x, y].IsActive();
    }

    public void UpdateOnTrigger()
    {
        if (!GameManager.IsRunning())
        {
            var curMousePosition = GameManager.GetCamera().ScreenToWorldPoint(Input.mousePosition);

            UpdateTargetSquare(GetSquaresByBrush(curMousePosition));

            if (Input.GetMouseButton(0))
            {
                UpdateSquareOnClick(GetSquaresByBrush(curMousePosition), true);
            }

            if (Input.GetMouseButton(1))
            {
                UpdateSquareOnClick(GetSquaresByBrush(curMousePosition), false);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RandomFillGrid();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearGrid();
            }

            return;
        }

        if (_timer >= _gameManager.GetTimeToIteration())
        {
            _timer = 0f;
            UpdateSquares();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    private List<Vector2Int> GetSquaresByBrush(Vector3 center)
    {
        var squares = new List<Vector2Int>();

        var x = Utils.GetCellCoordinates(center).x;
        var y = Utils.GetCellCoordinates(center).y;

        for (var dx = -_gameManager.GetBrushSize(); dx <= _gameManager.GetBrushSize(); dx++)
        {
            for (var dy = -_gameManager.GetBrushSize(); dy <= _gameManager.GetBrushSize(); dy++)
            {
                if (Mathf.Abs(dy) + Mathf.Abs(dx) <= _gameManager.GetBrushSize())
                {
                    squares.Add(new Vector2Int(x + dx, y + dy));
                }
            }
        }

        return squares;
    }

    private GameObject SpawnSquare(Vector3 position, float squareSize)
    {
        var square = Instantiate(squarePrefab, position, Quaternion.identity);
        square.transform.localScale = new Vector3(squareSize, squareSize, 1);
        return square;
    }
}
