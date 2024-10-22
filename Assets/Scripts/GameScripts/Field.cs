using System.Collections.Generic;
using UnityEngine;

public class Field
{
    private readonly List<Border> _horizontalLines;
    private readonly List<Border> _verticalLines;

    public Field(int width, int height)
    {
        _horizontalLines = CreateLines(width, height+1, isHorizontal: true);
        _verticalLines = CreateLines(width+1, height, isHorizontal: false);
    }

    private List<Border> CreateLines(int width, int height, bool isHorizontal)
    {
        var lines = new List<Border>();

        if (isHorizontal)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 start = Utils.GetWorldCellPosition(0, y);
                Vector2 end = Utils.GetWorldCellPosition(width, y);
                lines.Add(new Border(start, end));
            }
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 start = Utils.GetWorldCellPosition(x, 0);
                Vector2 end = Utils.GetWorldCellPosition(x, height);
                lines.Add(new Border(start, end));
            }
        }

        return lines;
    }

    public void Render()
    {
        _horizontalLines.ForEach(border => border.Render());
        _verticalLines.ForEach(border => border.Render());
    }
}