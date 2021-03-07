using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{ 
    Empty,
    Entrance,
    Exit,
    DeadEnd
}
public enum RoomPattern
{
    Up,
    UpDown,
    Down,
    Left,
    Right,
    LeftRight,
    UpDownLeftRight,
    RightDown,
    LeftDown,
    RightUp,
    LeftUp,
    LeftRightDown,
    LeftRightUp,
    UpRightDown,
    UpLeftDown,
    
    

    Closed // Closed is used only in intermediate stages during generation. A room should never be closed after generation is complete.
}
public class Room
{
    public RoomType type;
    public RoomPattern pattern;
    // Allowing each room to know its location on the grid will help out for dungeon generation.
    public int x;
    public int y;


    public GameObject roomObj = null;
    public Room()
    {
        type = RoomType.Empty;
        pattern = RoomPattern.Closed;
    }

    public void AddDownRoom()
    {
        if (pattern == RoomPattern.Up)
            pattern = RoomPattern.UpDown;

        else if (pattern == RoomPattern.Down)
            pattern = RoomPattern.UpDown;

        else if (pattern == RoomPattern.Right)
            pattern = RoomPattern.RightDown;

        else if (pattern == RoomPattern.Left)
            pattern = RoomPattern.LeftDown;

        else if (pattern == RoomPattern.Closed)
        {
            pattern = RoomPattern.Down;
        }
        else if (pattern == RoomPattern.LeftRight)
            pattern = RoomPattern.LeftRightDown;

        else if (pattern == RoomPattern.RightUp)
            pattern = RoomPattern.UpRightDown;

        else if (pattern == RoomPattern.LeftUp)
            pattern = RoomPattern.UpLeftDown;



        else
        {
            Debug.Log("Tried to add room but previous room did not match any valid type!");
        }
    }
    public void AddRightRoom()
    {
        if (pattern == RoomPattern.Up)
            pattern = RoomPattern.RightUp;

        else if (pattern == RoomPattern.Down)
            pattern = RoomPattern.RightDown;

        else if (pattern == RoomPattern.Right)
            pattern = RoomPattern.Right;

        else if (pattern == RoomPattern.Left)
            pattern = RoomPattern.LeftRight;

        else if (pattern == RoomPattern.Closed)
        {
            pattern = RoomPattern.Right;
        }

        else if (pattern == RoomPattern.UpDown)
            pattern = RoomPattern.UpRightDown;

        else if (pattern == RoomPattern.LeftUp)
            pattern = RoomPattern.LeftRightUp;

        else if (pattern == RoomPattern.LeftDown)
            pattern = RoomPattern.LeftRightDown;

        else
        {
            Debug.Log("Tried to add room but previous room did not match any valid type!");
        }
    }
    public void AddLeftRoom()
    {
        if (pattern == RoomPattern.Up)
            pattern = RoomPattern.LeftUp;

        else if (pattern == RoomPattern.Down)
            pattern = RoomPattern.LeftDown;

        else if (pattern == RoomPattern.Right)
            pattern = RoomPattern.LeftRight;

        else if (pattern == RoomPattern.Left)
            pattern = RoomPattern.Left;

        else if (pattern == RoomPattern.Closed)
        {
            pattern = RoomPattern.Left;
        }

        else if (pattern == RoomPattern.LeftRight)
            pattern = RoomPattern.LeftRightDown;

        else if (pattern == RoomPattern.RightUp)
            pattern = RoomPattern.LeftRightUp;

        else if (pattern == RoomPattern.RightDown)
            pattern = RoomPattern.LeftRightDown;

        else
        {
            Debug.Log("Tried to add room but previous room did not match any valid type!");
        }
    }

    public void AddUpRoom()
    {
        if (pattern == RoomPattern.RightDown)
            pattern = RoomPattern.UpRightDown;

        else if (pattern == RoomPattern.LeftRight)
            pattern = RoomPattern.LeftRightUp;

        else
        {
            Debug.Log("Tried to add room but previous room did not match any valid type!");
        }
    }

    public void SetRoomObj(GameObject newRoomObj)
    {
        this.roomObj = newRoomObj;
    }
}
