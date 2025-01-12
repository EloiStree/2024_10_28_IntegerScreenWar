using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

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
    public NativeArray<Color32> m_screenMapMemoryRef;


    [ContextMenu("Execute")]
    public void Execute()
    {

        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_isPlayerFiringRef = m_isPlayerFiring.GetNativeArrayHolder().GetNativeArray();
        m_cursorPositionRef = m_cursorPosition.GetNativeArrayHolder().GetNativeArray();
        m_screenMapMemoryRef = m_screenMapMemory.GetNativeArray();

        int lenght = SNAM16K.ARRAY_MAX_SIZE;
        int width = m_screenMapMemory.GetScreenWidth();
        STRUCTJOB_PushPlayerCursorPositionToMemoryMapMono job = new STRUCTJOB_PushPlayerCursorPositionToMemoryMapMono()
        {
            m_isPlayerPlayingRef = m_isPlayerPlayingRef,
            m_isPlayerFiringRef = m_isPlayerFiringRef,
            m_cursorPositionRef = m_cursorPositionRef,
            m_screenWidth = width,
            m_screenMapMemoryRef = m_screenMapMemoryRef
            ,
            m_maxSize = lenght
            ,
            m_cursorColor = m_cursorColor
        };
        JobHandle jh = job.Schedule(lenght, 64);
        jh.Complete();
    }
}
[BurstCompile(CompileSynchronously = true)]
public struct STRUCTJOB_PushPlayerCursorPositionToMemoryMapMono : IJobParallelFor
{

    [ReadOnly]
    public NativeArray<bool> m_isPlayerPlayingRef;
    [ReadOnly]
    public NativeArray<bool> m_isPlayerFiringRef;
    [ReadOnly]
    public NativeArray<STRUCT_PixelPosition> m_cursorPositionRef;
    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<Color32> m_screenMapMemoryRef;
    [ReadOnly]
    public int m_screenWidth;
    [ReadOnly]
    public int m_maxSize;
    [ReadOnly]
    public Color m_cursorColor;

    public void Execute(int index)
    {
        if (m_isPlayerPlayingRef[(index)] == false)
            return;
        if (m_isPlayerFiringRef[(index)] == false)
            return;

        STRUCT_PixelPosition pp = m_cursorPositionRef[(index)];
        int index2 = ((int)pp.m_xLeftRight) + ((int)pp.m_yDownTop) * m_screenWidth;
        m_screenMapMemoryRef[index2] = m_cursorColor;
    }
}
