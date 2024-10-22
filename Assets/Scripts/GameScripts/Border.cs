using UnityEngine;

public class Border
{
    private Vector3[] _points;
    private LineRenderer _renderer;

    public Border(Vector3 startPoint, Vector3 endPoint)
    {
        _points = new Vector3[2] { startPoint, endPoint };
        CreateLineObject();
        ConfigureRenderer();
    }

    private void CreateLineObject()
    {
        GameObject lineObject = new GameObject("Border");
        _renderer = lineObject.AddComponent<LineRenderer>();
    }

    private void ConfigureRenderer()
    {
        _renderer.material = new Material(Shader.Find("Sprites/Default"));
        _renderer.startColor = _renderer.endColor = Color.white;
        _renderer.widthMultiplier = 0.05f;
        _renderer.positionCount = _points.Length;
    }

    public void Render()
    {
        _renderer.SetPositions(_points);
    }
}