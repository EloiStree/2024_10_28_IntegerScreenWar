using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;

public class PushPlayerCursorPositionToMemoryMapMono : MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMapMemory;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectBool m_isPlayerFiring;
    public SNAM16K_ObjectScreenPixelPosition m_cursorPosition;

    public Color m_cursorColor = Color.red;

    public NativeArray<bool> m_isPlayerPlayingRef;
    public NativeArray<bool> m_isPlayerFiringRef;
    public NativeArray<STRUCT_PixelPosition> m_cursorPositionRef;


    [ContextMenu("Execute")]
    public void Execute()
    {

        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_isPlayerFiringRef = m_isPlayerFiring.GetNativeArrayHolder().GetNativeArray();
        m_cursorPositionRef = m_cursorPosition.GetNativeArrayHolder().GetNativeArray();

        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlayingRef[(i)] == false)
                continue;
            if(m_isPlayerFiringRef[(i)] == false)
                continue;
            
            STRUCT_PixelPosition pp = m_cursorPositionRef[(i)];
            int index = ((int)pp.m_xLeftRight) + ((int)pp.m_yDownTop) * m_screenMapMemory.GetScreenWidth();
            m_screenMapMemory.SetColor(index, m_cursorColor);
        }
    }
}
