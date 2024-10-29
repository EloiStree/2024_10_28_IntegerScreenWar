
using Eloi.SNAM;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class MovePlayerFromIntInputMono : MonoBehaviour
{
    public SNAM16K_ObjectInt m_controllerState;
    public SNAM16K_ObjectColor32 m_playerColor;
    public SNAM16K_ObjectPixelPositionShort m_playerPosition;
    public int m_screenWidth = 1920;
    public int m_screenHeight = 1080;
    public int m_playerCount = 100;
    public float m_deltaTime = 0.1f;

    public Color m_defaultColor = Color.red;
    public UnityEvent<NativeArray<Color32>> m_onMapChanged;
    public NativeArray<Color32> m_colorNativeArray;

    public float m_pixelSpeed = 50;
    public void SetPlayerCount(int count)
    {
        m_playerCount = count;
    }

    public void OnEnable()
    {
        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;
        m_colorNativeArray = new NativeArray<Color32>(m_screenWidth * m_screenHeight, Allocator.Persistent);
        for (int i = 0; i < m_colorNativeArray.Length; i++)
        {
            m_colorNativeArray[i] = m_defaultColor;
        }
        m_onMapChanged.Invoke(m_colorNativeArray);

    }
    public void OnDisable()
    {
        if(m_colorNativeArray.IsCreated)
            m_colorNativeArray.Dispose();
        
    }

    [ContextMenu("Apply Turn")]
    public void ApplyTurn() { 
        m_deltaTime = Time.deltaTime;
        STRUCTJOB_MovePlayerFromIntInput job = new STRUCTJOB_MovePlayerFromIntInput()
        {
            m_controllerState = m_controllerState.GetNativeArrayHolder().GetNativeArray(),
            m_playerPositionCurrent = m_playerPosition.GetNativeArrayHolder().GetNativeArray(),
            m_pixelSpeed = m_pixelSpeed,
            m_timePassed = m_deltaTime,
            m_screenWidthMaxIndex = (short)(m_screenWidth - 1),
            m_screenHeightMaxIndex = (short)(m_screenHeight - 1)
        };

        JobHandle jobHandle = job.Schedule(m_playerCount, 64);
        jobHandle.Complete();

        STRUCTJOB_PixelPositionToColor32 job2 = new STRUCTJOB_PixelPositionToColor32()
        {
            m_playerPositionCurrent = m_playerPosition.GetNativeArrayHolder().GetNativeArray(),
            m_playerColor = m_playerColor.GetNativeArrayHolder().GetNativeArray(),
            m_textureColors = m_colorNativeArray,
            m_textureWidth = m_screenWidth,
            m_textureHeight = m_screenHeight,
            m_playerCount = m_playerCount
        };

        JobHandle jobHandle2 = job2.Schedule(m_playerCount, 64);
        jobHandle2.Complete();
    }
}
