using UnityEngine;
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

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
    public NativeArray<Color32> m_screenMapFrontRef;


    [ContextMenu("Execute")]
    public void Execute()
    {
        m_isPlayerPlayingRef = m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray();
        m_positionPlayerRef = m_playerCurrentPosition.GetNativeArrayHolder().GetNativeArray();
        m_ledPlayerRef = m_playerLed25.GetNativeArrayHolder().GetNativeArray();
        m_screenMapFrontRef = m_screenMapFront.GetNativeArray();
        int lenght = SNAM16K.ARRAY_MAX_SIZE;

        STRUCTJOB_PushPlayerPositionToFrontMapMono job = new STRUCTJOB_PushPlayerPositionToFrontMapMono()
        {
            m_isPlayerPlayingRef = m_isPlayerPlayingRef,
            m_positionPlayerRef = m_positionPlayerRef,
            m_ledPlayerRef = m_ledPlayerRef,
            m_screenMapFrontRef = m_screenMapFrontRef,
            m_screenWidth = m_screenMapFront.m_screenWidth,
            m_ledColor = m_ledColor,
            m_maxSize = m_screenMapFrontRef.Length
        };
        Unity.Jobs.JobHandle jh = job.Schedule(lenght, 64);
        jh.Complete();
    }

    [BurstCompile(CompileSynchronously = true)]
    public struct STRUCTJOB_PushPlayerPositionToFrontMapMono : IJobParallelFor
    {
        
        [ReadOnly]
        public NativeArray<bool> m_isPlayerPlayingRef;
        [ReadOnly]
        public NativeArray<STRUCT_PixelPosition> m_positionPlayerRef;
        [ReadOnly]
        public NativeArray<STRUCT_Led25PercentLRDT> m_ledPlayerRef;

        [NativeDisableParallelForRestriction]
        [WriteOnly]
        public NativeArray<Color32> m_screenMapFrontRef;
        
        [ReadOnly]
        public int m_screenWidth;

        [ReadOnly]
        public int m_maxSize;
        [ReadOnly]
        public Color m_ledColor;


        public void Execute(int i)
        {
            if (m_isPlayerPlayingRef[i] == false)
                return;
            int index;

            STRUCT_PixelPosition pc = m_positionPlayerRef[i];
            index = ((int)pc.m_xLeftRight) + ((int)pc.m_yDownTop) * m_screenWidth;
            m_screenMapFrontRef[index] = m_ledColor;
            int x = (int)pc.m_xLeftRight;
            int y = (int)pc.m_yDownTop;
            STRUCT_Led25PercentLRDT led25 = m_ledPlayerRef[i];
            for (int xx = 0; xx < 5; xx++)
            {
                for (int yy = 0; yy < 5; yy++)
                {
                    led25.GetValueLRDT(xx, yy, out bool isOn);
                    if (isOn) { 
                    
                        int x2 = x + xx - 2;
                        int y2 = y + yy - 2;
                        int index2 = x2 + y2 * m_screenWidth;
                        if (x2 >= 0 && x2 < m_screenWidth && y2 >= 0 && y2 < m_screenMapFrontRef.Length && index2<m_maxSize)
                        {

                            m_screenMapFrontRef[x2 + y2 * m_screenWidth] = m_ledColor;
                        }

                    }
                }
            }

        }
    }
}
