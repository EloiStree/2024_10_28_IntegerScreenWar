using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;

public class PushPlayerPositionToMemoryMapMono : MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMapMemory;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectColor32 m_playerColor;
    public SNAM16K_ObjectScreenPixelPosition m_playerPreviousPosition;

    public NativeArray<bool> m_isPlayerPlayingRef;
    public NativeArray<Color32> m_playerColorRef;
    public NativeArray<STRUCT_PixelPosition> m_playerPreviousPositionRef;

    [ContextMenu("Execute")]
    public void Execute()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_playerColorRef = m_playerColor.GetNativeArrayHolder().GetNativeArray();
        m_playerPreviousPositionRef = m_playerPreviousPosition.GetNativeArrayHolder().GetNativeArray();
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlayingRef[(i)] == false)
                continue;
            Color32 c = m_playerColorRef[(i)];
            STRUCT_PixelPosition pp = m_playerPreviousPositionRef[(i)];
            int index = ((int)pp.m_xLeftRight) + ((int)pp.m_yDownTop) * m_screenMapMemory.GetScreenWidth();
            m_screenMapMemory.SetColor(index, c);
        }
    }
}
