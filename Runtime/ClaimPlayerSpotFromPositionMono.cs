using Eloi.SNAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimPlayerSpotFromPositionMono : MonoBehaviour
{
    public ScreenMapAsClaimIntNativeIndexMono m_claimMap;
    public SNAM16K_ObjectBool m_isPlayerActive;
    public SNAM16K_ObjectScreenPixelPosition m_playerPosition;

    
    public void Execute()
    {
        for (int i = 0; i < SNAM16K.ARRAY_MAX_SIZE; i++)
        {
            if (m_isPlayerActive.Get(i) == false)
                continue;
            STRUCT_PixelPosition pos = m_playerPosition.Get(i);
            int x = (int)Mathf.Round(pos.m_xLeftRight);
            int y = (int)Mathf.Round(pos.m_yDownTop);
            m_claimMap.Claim(x, y, i);
        }
    }
}
