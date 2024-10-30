
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class MovePlayerFromIntInputMono : MonoBehaviour
{
    public ScreenMapAsNativeColor32Mono m_screenMap;
    public SNAM16K_ObjectGamepad2020 m_controllerState;
    public SNAM16K_ObjectScreenPixelPosition m_playerPreviousPosition;
    public SNAM16K_ObjectScreenPixelPosition m_playerCurrentPosition;
    public SNAM16K_ObjectScreenPixelPosition m_playerCursorPosition;
    public SNAM16K_ObjectBool m_isPlayerPlaying;

    public float m_pixelSpeed = 50;
    public float m_cursorRange = 3;
    public float m_joystickDeadZone = 0.1f;
    [ContextMenu("Execute")]
    public void Execute() {

        if (m_screenMap == null || !m_screenMap.IsInitialized())
            return;
        float deltaTime = Time.deltaTime;

        m_screenMap.GetScreenHeight(out int screenHeight);
        m_screenMap.GetScreenWidth(out int screenWidth);

        float deltaSpeed = m_pixelSpeed * deltaTime;
        STRUCTJOB_MovePlayerFromIntInput job = new STRUCTJOB_MovePlayerFromIntInput()
        {
            m_isPlayerPlaying= m_isPlayerPlaying.GetNativeArrayHolder().GetNativeArray(),
            m_controllerState = m_controllerState.GetNativeArrayHolder().GetNativeArray(),
            m_playerPositionPrevious = m_playerPreviousPosition.GetNativeArrayHolder().GetNativeArray(),
            m_playerPositionCurrent = m_playerCurrentPosition.GetNativeArrayHolder().GetNativeArray(),
            m_playerPositionCursor = m_playerCursorPosition.GetNativeArrayHolder().GetNativeArray(),
            m_pixelSpeedDelta = deltaSpeed,
            m_cursorRange = m_cursorRange,
            m_joystickDeadZone= m_joystickDeadZone,
            m_screenWidthMaxIndex = (short)(screenWidth - 1),
            m_screenHeightMaxIndex = (short)(screenHeight - 1)
        };

        JobHandle jobHandle = job.Schedule(SNAM16K.ARRAY_MAX_SIZE, 64);
        jobHandle.Complete();

    }
}
