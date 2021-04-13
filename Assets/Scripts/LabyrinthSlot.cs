using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabyrinthSlot : MonoBehaviour
{
    Labyrinth labyrinth;
    public types type;
    public enum types
    {
        NORMAL,
        WALL,
        INIT,
        END_OK,
        END_BAD
    }
    [SerializeField] private Image image;
    public Image wall;

    public void Init(Labyrinth labyrinth, int value)
    {
        this.labyrinth = labyrinth;
        if (value == 0) type = types.NORMAL;
        if (value == 1) type = types.WALL;
        if (value == 2) type = types.INIT;
        if (value == 3) type = types.END_OK;
        if (value == 4) type = types.END_BAD;

        wall.enabled = false;

        if (type == types.WALL)
        {
            wall.enabled = true;
            SetColor(labyrinth.slotWallColor);
        }
        else
            SetColor(labyrinth.slotInactiveColor);
    }
    public void OnOver()
    {
       labyrinth.OnOver(this);
    }
    public void SetColor(Color color)
    {
        image.color = color;
    }
}
