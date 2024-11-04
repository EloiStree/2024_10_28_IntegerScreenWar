using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;

public class PushPlayerLed25AtPositionToFrontMapMono : MonoBehaviour
{

    public ScreenMapAsNativeColor32Mono m_screenMapFront;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectScreenPixelPosition m_playerCurrentPosition;
    public SNAM16K_ObjectLed25 m_playerLed25;

    public Color m_ledColor = Color.red;


    public NativeArray<bool> m_isPlayerPlayingRef;
    public NativeArray<STRUCT_PixelPosition> m_positionPlayerRef;
    public NativeArray<STRUCT_Led25PercentLRDT> m_ledPlayerRef;


    [ContextMenu("Execute")]
    public void Execute()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_positionPlayerRef = m_playerCurrentPosition.GetNativeArrayHolder().GetNativeArray();
        m_ledPlayerRef = m_playerLed25.GetNativeArrayHolder().GetNativeArray();
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        for (int i = 0; i < lenght; i++)
        {
            if (m_isPlayerPlayingRef[i] == false)
                continue;
            int index;

            STRUCT_PixelPosition pc = m_positionPlayerRef[i];
            index = ((int)pc.m_xLeftRight) + ((int)pc.m_yDownTop) * m_screenMapFront.GetScreenWidth();
            m_screenMapFront.SetColor(index, m_ledColor);
            int x = (int)pc.m_xLeftRight;
            int y = (int)pc.m_yDownTop;
            STRUCT_Led25PercentLRDT led25 = m_ledPlayerRef[i];
            for(int xx=0; xx<5; xx++)
            {
                for (int yy = 0; yy < 5; yy++)
                {
                    led25.GetValueLRDT(xx, yy, out bool isOn);
                    if(isOn)    
                        m_screenMapFront.SetColor(x+xx-2, y+yy-2, m_ledColor);
                }
            }
            

        }
    }
}
