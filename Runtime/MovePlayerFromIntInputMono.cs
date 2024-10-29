
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class MovePlayerFromIntInputMono : MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMap;
    public SNAM16K_ObjectInt m_controllerState;
    public SNAM16K_ObjectColor32 m_playerColor;
    public SNAM16K_ObjectPixelPositionShort m_playerPosition;
    public int m_screenWidth = 1920;
    public int m_screenHeight = 1080;
    public int m_playerCount = 100;
    public float m_deltaTime = 0.1f;

    public float m_pixelSpeed = 50;

    public UnityEvent m_playersMoved;

    public void SetPlayerCount(int count)
    {
        m_playerCount = count;
    }


    [ContextMenu("Apply Turn")]
    public void ApplyTurn() {

        if (m_screenMap == null || !m_screenMap.IsInitialized())
            return;
        m_deltaTime = Time.deltaTime;

        m_screenMap.GetScreenHeight(out int screenHeight);
        m_screenMap.GetScreenWidth(out int screenWidth);

        STRUCTJOB_MovePlayerFromIntInput job = new STRUCTJOB_MovePlayerFromIntInput()
        {
            m_controllerState = m_controllerState.GetNativeArrayHolder().GetNativeArray(),
            m_playerPositionCurrent = m_playerPosition.GetNativeArrayHolder().GetNativeArray(),
            m_pixelSpeed = m_pixelSpeed,
            m_timePassed = m_deltaTime,
            m_screenWidthMaxIndex = (short)(screenWidth - 1),
            m_screenHeightMaxIndex = (short)(screenHeight - 1)
        };

        JobHandle jobHandle = job.Schedule(m_playerCount, 64);
        jobHandle.Complete();


        STRUCTJOB_PixelPositionToColor32 job2 = new STRUCTJOB_PixelPositionToColor32()
        {
            m_playerPositionCurrent = m_playerPosition.GetNativeArrayHolder().GetNativeArray(),
            m_playerColor = m_playerColor.GetNativeArrayHolder().GetNativeArray(),
            m_textureColors = m_screenMap.GetNativeArray(),
            m_playerCount = m_playerCount,
            m_textureWidth = screenWidth,
            m_textureHeight = screenHeight,
            
        };

        JobHandle jobHandle2 = job2.Schedule(m_playerCount, 64);
        jobHandle2.Complete();

        m_playersMoved.Invoke();
    }
}
