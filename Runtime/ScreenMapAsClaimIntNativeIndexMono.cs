using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class ScreenMapAsClaimIntNativeIndexMono : MonoBehaviour
{
    public NativeArray<int> m_claimIntNativeIndex;
    public int m_screenWidth = 800;
    public int m_screenHeight=450;
    public int m_pixelCount;

    void Start()
    {
        m_pixelCount = m_screenWidth * m_screenHeight;
        m_claimIntNativeIndex = new NativeArray<int>(m_pixelCount, Allocator.Persistent);
    }

    public void Claim(int index, int playerIndex)
    {
        m_claimIntNativeIndex[index] = playerIndex;
    }
    
    public void Unclaim(int index)
    {
        m_claimIntNativeIndex[index] = 0;
    }
    public void SlowUnclaimAll()
    {
        for (int i = 0; i < m_pixelCount; i++)
        {
            m_claimIntNativeIndex[i] = 0;
        }
    }

    public int WhoOwns(int index)
    {
        return m_claimIntNativeIndex[index];
    }

    public bool IsClaimed(int index)
    {
        return m_claimIntNativeIndex[index] != 0;
    }

    void OnDestroy()
    {
        if(m_claimIntNativeIndex.IsCreated)
        m_claimIntNativeIndex.Dispose();
    }

    public void Claim(int x, int y, int playerIndex)
    {
        int index = y * m_screenWidth + x;
        m_claimIntNativeIndex[index] = playerIndex;
    }
}
