using Eloi.SNAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarMono_CheckCollisionOnMemoryMap : MonoBehaviour
{


    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectScreenPixelPosition m_playerPosition;
    public ScreenMapAsNativeColor32Mono m_memoryMap;
    public SNAM16K_ObjectBool m_isInCollisoin;

    public byte m_alphaThreshold = 10;
    [ContextMenu("Execute")]
    public void Execute() { 
    
        for(int i = 0; i < SNAM16K.ARRAY_MAX_SIZE; i++) {

            if (m_isPlayerPlaying.IsFalse(i))
                continue;

            STRUCT_PixelPosition pixel = m_playerPosition.Get(i);
            Color32 color = m_memoryMap.GetAsRound(pixel.m_xLeftRight, pixel.m_yDownTop);
            if (color.a > m_alphaThreshold)
            {
                m_isInCollisoin.Set(i, true);
            }
            else
            {
                m_isInCollisoin.Set(i, false);
            }
        }
    }
}
