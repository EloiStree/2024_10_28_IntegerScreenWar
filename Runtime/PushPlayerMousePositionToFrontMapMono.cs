using UnityEngine;
using Unity.Collections;

public class PushPlayerMousePositionToFrontMapMono : MonoBehaviour
{

    public ScreenMapAsNativeColor32Mono m_screenMapFront;
    public bool m_useMemory = true;
    public ScreenMapAsNativeColor32Mono m_screenMapMemory;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_CursorPosition2020 m_playerCurrentPosition;
    public SNAM16K_CursorPosition2020 m_playerPreviousPosition;
    public Color m_playerCursorColor = Color.red;

    private NativeArray<bool> m_isPlayerPlayingRef;
    private NativeArray<STRUCT_CursorPosition2020> m_cursorPlayercurrentRef;
    private NativeArray<STRUCT_CursorPosition2020> m_cursorPlayerPreviousRef;


    public void SetUseOfMemoryMap( bool useMemoryMap)
    {
        m_useMemory = useMemoryMap;
    }
    


    [ContextMenu("Execute")]
    public void Execute()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_cursorPlayercurrentRef = m_playerCurrentPosition.GetNativeArrayHolder().GetNativeArray();
        m_cursorPlayerPreviousRef = m_playerPreviousPosition.GetNativeArrayHolder().GetNativeArray();
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlayingRef[i] == false)
                continue;
            STRUCT_CursorPosition2020 pt = m_cursorPlayercurrentRef[i];
            m_screenMapFront.SetColor(pt.m_leftRightPercent, pt.m_downTopPercent, m_playerCursorColor);
            if(m_useMemory && m_screenMapMemory)
            {
                m_screenMapMemory.SetColor(pt.m_leftRightPercent, pt.m_downTopPercent, m_playerCursorColor);
            }
            STRUCT_CursorPosition2020 pc = m_cursorPlayerPreviousRef[i];
            m_screenMapFront.SetColor(pt.m_leftRightPercent, pt.m_downTopPercent, m_playerCursorColor);



        }
    }
}
