using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject squaresManagerObject;
    
    private static Field _field;
    private static SquaresManager _squaresManager;
    private static Camera _camera;

    [SerializeField] private float timeToIteration;
    [SerializeField] private int width;
    [SerializeField] private int height;  
    [SerializeField] private float cellSize;
    [SerializeField] private int brushSize;

    private static bool _isRunning;

    private void Awake()
    {
        SetupGame();
    }

    private void Update()
    {
        HandleInput();
        _field.Render();
        _squaresManager.UpdateOnTrigger();
    }

    private void SetupGame()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _isRunning = false;

        Utils.Instantiate(width, height, cellSize);
        
        _field = new Field(width, height);

        _squaresManager = squaresManagerObject.GetComponent<SquaresManager>();
        _camera = cameraObject.GetComponent<Camera>();

        CenterCamera();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isRunning = !_isRunning;
        }

        for (var i = 0; i <= 9; ++i)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                brushSize = i;
            }
        }
    }

    private void CenterCamera()
    {
        cameraObject.transform.position = new Vector3(
            width * cellSize / 2f, 
            height * cellSize / 2f, 
            -10
        );
    }

    public static bool IsRunning()
    {
        return _isRunning;
    }

    public static Camera GetCamera()
    {
        return _camera;
    }

    public int GetWidth() => width;

    public int GetHeight() => height;

    public float GetCellSize() => cellSize;

    public int GetBrushSize() => brushSize;

    public float GetTimeToIteration() => timeToIteration;

    public List<Vector2Int> GetSquaresByBrush(Vector3 center)
    {
        var squares = new List<Vector2Int>();

        var cellCoordinates = Utils.GetCellCoordinates(center);
        int x = cellCoordinates.x;
        int y = cellCoordinates.y;

        for (var dx = -brushSize; dx <= brushSize; dx++)
        {
            for (var dy = -brushSize; dy <= brushSize; dy++)
            {
                if (Mathf.Abs(dx) + Mathf.Abs(dy) <= brushSize)
                {
                    squares.Add(new Vector2Int(x + dx, y + dy));
                }
            }
        }

        return squares;
    }
}
