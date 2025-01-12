using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class PushPlayerPositionToMemoryMapMono : MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMapMemory;
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectColor32 m_playerColor;
    public SNAM16K_ObjectScreenPixelPosition m_playerPreviousPosition;

    public NativeArray<bool> m_isPlayerPlayingRef;
    public NativeArray<Color32> m_playerColorRef;
    public NativeArray<STRUCT_PixelPosition> m_playerPreviousPositionRef;
    public NativeArray<Color32> m_screenMapMemoryRef;

    [ContextMenu("Execute")]
    public void Execute()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_playerColorRef = m_playerColor.GetNativeArrayHolder().GetNativeArray();
        m_playerPreviousPositionRef = m_playerPreviousPosition.GetNativeArrayHolder().GetNativeArray();
        m_screenMapMemoryRef = m_screenMapMemory.GetNativeArray();
        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        int width = m_screenMapMemory.GetScreenWidth();
        STRUCTJOB_PushPlayerPositionToMemoryMapMono job = new STRUCTJOB_PushPlayerPositionToMemoryMapMono()
        {
            m_isPlayerPlayingRef = m_isPlayerPlayingRef,
            m_playerColorRef = m_playerColorRef,
            m_playerPreviousPositionRef = m_playerPreviousPositionRef,
            m_screenWidth = width,
            m_screenMapMemoryRef = m_screenMapMemoryRef
            ,m_maxSize = lenght


        };
        JobHandle jh = job.Schedule(lenght, 64);
        jh.Complete();

    }
}


[BurstCompile(CompileSynchronously = true)]
public struct STRUCTJOB_PushPlayerPositionToMemoryMapMono : IJobParallelFor
{

    [ReadOnly]
    public NativeArray<bool> m_isPlayerPlayingRef;
    [ReadOnly]
    public NativeArray<Color32> m_playerColorRef;
    [ReadOnly]
    public NativeArray<STRUCT_PixelPosition> m_playerPreviousPositionRef;
    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<Color32> m_screenMapMemoryRef;
    [ReadOnly]
    public int m_screenWidth;
    [ReadOnly]
    public int m_maxSize;

    public void Execute(int i)
    {
      
            if (m_isPlayerPlayingRef[i] == false)
                return;
            Color32 c = m_playerColorRef[i];
            STRUCT_PixelPosition pp = m_playerPreviousPositionRef[i];
            int index = ((int)pp.m_xLeftRight) + ((int)pp.m_yDownTop) * m_screenWidth;
            if(index>=0 && index<m_screenMapMemoryRef.Length)
                m_screenMapMemoryRef[index] = c;
        
    }
}
