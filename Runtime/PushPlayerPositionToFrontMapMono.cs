using UnityEngine;
using Eloi.SNAM;
using Eloi.Led25;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

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
    private NativeArray<Color32> m_cursorPositionColor;

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
        m_cursorPositionColor = new NativeArray<Color32>(lenght, Allocator.TempJob);
        STRUCTJOB_PushPlayerPositionToFrontMapMono job = new STRUCTJOB_PushPlayerPositionToFrontMapMono()
        {
            m_isPlayerPlayingRef = m_isPlayerPlayingRef,
            m_positionPlayerRef = m_positionPlayerRef,
            m_cursorPlayerRef = m_cursorPlayerRef,
            m_cursorPositionColor = m_cursorPositionColor,
            m_screenMapFrontRef = m_screenMapFront.GetNativeArray(),
            m_screenWidth = m_screenMapFront.m_screenWidth,
            m_maxSize = lenght,
            m_playerPositionColor = m_playerPositionColor,
            m_playerCursorColor = m_playerCursorColor
        };
        Unity.Jobs.JobHandle jh = job.Schedule(lenght, 64);
        jh.Complete();




    }

    [BurstCompile(CompileSynchronously = true)]
    public struct STRUCTJOB_PushPlayerPositionToFrontMapMono : Unity.Jobs.IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<bool> m_isPlayerPlayingRef;
        [ReadOnly]
        public NativeArray<STRUCT_PixelPosition> m_positionPlayerRef;
        [ReadOnly]
        public NativeArray<STRUCT_PixelPosition> m_cursorPlayerRef;
        [WriteOnly]
        public NativeArray<Color32> m_cursorPositionColor;
        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<Color32> m_screenMapFrontRef;
        [ReadOnly]
        public int m_screenWidth;
        [ReadOnly]
        public int m_maxSize;
        [ReadOnly]
        public Color m_playerPositionColor;
        [ReadOnly]
        public Color m_playerCursorColor;
        public void Execute(int i)
        {
            if (m_isPlayerPlayingRef[i] == false)
                return;
            int index;
            STRUCT_PixelPosition pt = m_positionPlayerRef[i];
            index = ((int)pt.m_xLeftRight) + ((int)pt.m_yDownTop) * m_screenWidth;
            m_screenMapFrontRef[index] = m_playerCursorColor;
            STRUCT_PixelPosition pc = m_cursorPlayerRef[i];
            index = ((int)pc.m_xLeftRight) + ((int)pc.m_yDownTop) * m_screenWidth;
            m_screenMapFrontRef[index] = m_playerPositionColor;
        }
    }


}
