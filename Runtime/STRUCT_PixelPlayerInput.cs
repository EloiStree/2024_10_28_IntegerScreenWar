
using Eloi.SNAM;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;


public struct STRUCTJOB_PixelPositionToColor32 : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<STRUCT_PixelPosition> m_playerPositionCurrent;
    [ReadOnly]
    public NativeArray<Color32> m_playerColor;

    [NativeDisableParallelForRestriction]
    public NativeArray<Color32> m_textureColors;
    public int m_textureWidth;
    public int m_textureHeight;
    public int m_playerCount;

    public void Execute(int index)
    {
        if (index >= m_playerCount)
            return;
        STRUCT_PixelPosition point = m_playerPositionCurrent[index];
        Color32 color = m_playerColor[index];
        int indexInTexture = ((int)point.m_xLeftRight) + ((int)point.m_yDownTop) * m_textureWidth;
        m_textureColors[indexInTexture] = color;
    }
}


public struct STRUCTJOB_MovePlayerFromIntInput : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<int> m_controllerState;
    public NativeArray<STRUCT_PixelPosition> m_playerPositionCurrent;
    public float m_pixelSpeed;
    public float m_timePassed;
    public short m_screenWidthMaxIndex;
    public short m_screenHeightMaxIndex;

    public void Execute(int index)
    {
        int intValue = m_controllerState[index];
        PixelPlayerUtility.GetPlayerMoveDirection(intValue, out PixelDirection moveDigit);
        if (moveDigit == PixelDirection.None)
        {
            return;
        }
        STRUCT_PixelPosition point = m_playerPositionCurrent[index];
        short d = (short)(1f + m_pixelSpeed * m_timePassed);
        switch (moveDigit) {
                case PixelDirection.Up: point.m_yDownTop += d; break;
                case PixelDirection.Down: point.m_yDownTop -= d; break;
                case PixelDirection.Left: point.m_xLeftRight -= d; break;
                case PixelDirection.Right: point.m_xLeftRight += d; break;
                case PixelDirection.UpLeft: point.m_xLeftRight -= d; point.m_yDownTop += d; break;
                case PixelDirection.UpRight: point.m_xLeftRight += d; point.m_yDownTop += d; break;
                case PixelDirection.DownLeft: point.m_xLeftRight -= d; point.m_yDownTop -= d; break;
                case PixelDirection.DownRight: point.m_xLeftRight += d; point.m_yDownTop -= d; break;

        }
        
        if(point.m_xLeftRight < 0) point.m_xLeftRight = 0;
        else if(point.m_xLeftRight > m_screenWidthMaxIndex) point.m_xLeftRight = m_screenWidthMaxIndex;
        if(point.m_yDownTop < 0) point.m_yDownTop = 0;
        else if(point.m_yDownTop > m_screenHeightMaxIndex) point.m_yDownTop = m_screenHeightMaxIndex;

        m_playerPositionCurrent[index] = point;
    }
}

public enum PixelDirection : byte
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
    UpLeft = 5,
    UpRight = 6,
    DownLeft = 7,
    DownRight = 8
}
public static class PixelPlayerUtility { 

    public static void GetPlayerMoveDirection(int intValue, out byte moveDigit) => moveDigit = (byte)(intValue % 10);
    public static void GetPlayerCursorDirection(int intValue, out byte cursorDigit) => cursorDigit = (byte)(intValue / 10 % 10);
    public static void GetPlayerSelectAction(int intValue, out byte selectAction) => selectAction = (byte)(intValue / 100 % 10);
 
    public static void GetPlayerMoveDirection(int intValue, out PixelDirection moveDigit) => moveDigit = (PixelDirection)(intValue % 10);
    public static void GetPlayerCursorDirection(int intValue, out PixelDirection cursorDigit) => cursorDigit = (PixelDirection)(intValue / 10 % 10);
    public static void GetPlayerSelectAction(int intValue, out PixelDirection selectAction) => selectAction = (PixelDirection)(intValue / 100 % 10);

}

[System.Serializable]
public struct STRUCT_PixelPosition { 

    public float m_xLeftRight;
    public float m_yDownTop;
}
