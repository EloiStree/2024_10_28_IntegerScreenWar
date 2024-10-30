using UnityEngine;
using Eloi.SNAM;

public class PushPlayerPositionToMemoryMapMono: MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMapMemory;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectColor32 m_playerColor;
    public SNAM16K_ObjectScreenPixelPosition m_playerPreviousPosition;


    [ContextMenu("Execute")]
    public void Execute()
    {
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlaying.Get(i) == false)
                continue;
            Color32 c = m_playerColor.Get(i);
            STRUCT_PixelPosition pp = m_playerPreviousPosition.Get(i);
            int index = ((int)pp.m_xLeftRight) + ((int)pp.m_yDownTop) * m_screenMapMemory.GetScreenWidth();
            m_screenMapMemory.SetColor(index, c);
        }
    }
}
