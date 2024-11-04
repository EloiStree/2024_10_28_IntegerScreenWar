using UnityEngine;
using Eloi.SNAM;
using Eloi.Led25;
using Unity.Collections;

public class PushPlayerPositionToFrontMapMono : MonoBehaviour
{

    public ScreenMapAsNativeColor32Mono m_screenMapFront;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectScreenPixelPosition m_playerCurrentPosition;
    public SNAM16K_ObjectScreenPixelPosition m_playerCursorPosition;
    public Color m_playerPositionColor = Color.yellow;
    public Color m_playerCursorColor = Color.red;

    private NativeArray<bool> m_isPlayerPlayingRef;
    private NativeArray<STRUCT_PixelPosition> m_positionPlayerRef;
    private NativeArray<STRUCT_PixelPosition> m_cursorPlayerRef;

    private void Awake()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_positionPlayerRef = m_playerCurrentPosition.GetNativeArrayHolder().GetNativeArray();
        m_cursorPlayerRef = m_playerCursorPosition.GetNativeArrayHolder().GetNativeArray();
    }

    [ContextMenu("Execute")]
    public void Execute()
    {
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlayingRef[i]== false)
                continue;
            int index;
            STRUCT_PixelPosition pt = m_positionPlayerRef[i];
            index = ((int)pt.m_xLeftRight) + ((int)pt.m_yDownTop) * m_screenMapFront.GetScreenWidth();
            m_screenMapFront.SetColor(index, m_playerCursorColor);
            STRUCT_PixelPosition pc = m_cursorPlayerRef[i];
            index = ((int)pc.m_xLeftRight) + ((int)pc.m_yDownTop) * m_screenMapFront.GetScreenWidth();
            m_screenMapFront.SetColor(index, m_playerPositionColor);



        }
    }
}
