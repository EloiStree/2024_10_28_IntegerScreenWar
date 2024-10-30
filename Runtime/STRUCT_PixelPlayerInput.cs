
using Eloi.SNAM;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

//00997766551

public struct STRUCTJOB_MovePlayerFromIntInput : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<bool> m_isPlayerPlaying;
    [ReadOnly]
    public NativeArray<GamepadByteId2020Percent11> m_controllerState;
    [WriteOnly]
    public NativeArray<STRUCT_PixelPosition> m_playerPositionPrevious;
    public NativeArray<STRUCT_PixelPosition> m_playerPositionCurrent;
    [WriteOnly]
    public NativeArray<STRUCT_PixelPosition> m_playerPositionCursor;
    public float m_pixelSpeedDelta;
    public float m_cursorRange;
    public short m_screenWidthMaxIndex;
    public short m_screenHeightMaxIndex;
    public float m_joystickDeadZone;

    public void Execute(int index)
    {
        if(m_isPlayerPlaying[index] == false)
            return;
        GamepadByteId2020Percent11 pad = m_controllerState[index];
        STRUCT_PixelPosition point = m_playerPositionCurrent[index];
        m_playerPositionPrevious[index] = new STRUCT_PixelPosition()
        {
            m_xLeftRight = point.m_xLeftRight,
            m_yDownTop = point.m_yDownTop
        };

        if( Mathf.Abs(pad.m_joystickLeftHorizontal) < m_joystickDeadZone)
            pad.m_joystickLeftHorizontal = 0;
        if( Mathf.Abs(pad.m_joystickLeftVertical) < m_joystickDeadZone)
            pad.m_joystickLeftVertical = 0;
        if( Mathf.Abs(pad.m_joystickRightHorizontal) < m_joystickDeadZone)
            pad.m_joystickRightHorizontal = 0;
        if( Mathf.Abs(pad.m_joystickRightVertical) < m_joystickDeadZone)
            pad.m_joystickRightVertical = 0;

        point.m_xLeftRight += pad.m_joystickLeftHorizontal* m_pixelSpeedDelta;
        point.m_yDownTop += pad.m_joystickLeftVertical* m_pixelSpeedDelta;

        STRUCT_PixelPosition cursor = point;
        cursor.m_xLeftRight += pad.m_joystickRightHorizontal * m_cursorRange;
        cursor.m_yDownTop += pad.m_joystickRightVertical * m_cursorRange;

       
        
        
        if(point.m_xLeftRight < 0) point.m_xLeftRight = 0;
        else if(point.m_xLeftRight > m_screenWidthMaxIndex) point.m_xLeftRight = m_screenWidthMaxIndex;
        if(point.m_yDownTop < 0) point.m_yDownTop = 0;
        else if(point.m_yDownTop > m_screenHeightMaxIndex) point.m_yDownTop = m_screenHeightMaxIndex;

        if(cursor.m_xLeftRight < 0) cursor.m_xLeftRight = 0;
        else if(cursor.m_xLeftRight > m_screenWidthMaxIndex) cursor.m_xLeftRight = m_screenWidthMaxIndex;
        if(cursor.m_yDownTop < 0) cursor.m_yDownTop = 0;
        else if(cursor.m_yDownTop > m_screenHeightMaxIndex) cursor.m_yDownTop = m_screenHeightMaxIndex;

        m_playerPositionCurrent[index] = point;
        m_playerPositionCursor[index] = cursor;
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

[System.Serializable]
public struct STRUCT_PixelPosition { 

    public float m_xLeftRight;
    public float m_yDownTop;
}


