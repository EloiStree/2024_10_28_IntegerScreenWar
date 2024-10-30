using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

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


