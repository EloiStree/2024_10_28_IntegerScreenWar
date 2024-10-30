using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;

public class RemoveAlphaColor32FromMapMono : MonoBehaviour
{

    public ScreenMapAsNativeColor32Mono m_mapToAffect;
    public ComputeShader m_colorAdjustComputeShader;
    private ComputeBuffer m_pixelBuffer;

    public bool m_useCoroutine = true;
    public int m_pixelPerSecondsToRemove = 50;
    Color32[] color32s;
    public float m_removePixelPerUpdate;


    [ContextMenu("Execute")]
    public void Execute()
    {
        NativeArray<Color32> pixelArray = m_mapToAffect.GetNativeArray();
        int pixelCount = m_mapToAffect.GetPixelCount();
        int width = m_mapToAffect.GetScreenWidth();
        int height = m_mapToAffect.GetScreenHeight();
        if(m_pixelBuffer == null)
            m_pixelBuffer = new ComputeBuffer(pixelCount, sizeof(byte) * 4);
        if(color32s == null)
            color32s = new Color32[pixelCount];
        m_pixelBuffer.SetData(pixelArray);


        m_removePixelPerUpdate = (int) (m_pixelPerSecondsToRemove* Time.deltaTime);
        if(m_removePixelPerUpdate <= 1)
            m_removePixelPerUpdate = 1;
        int kernelHandle = m_colorAdjustComputeShader.FindKernel("CSMain");
        m_colorAdjustComputeShader.SetInt("width", width);
        m_colorAdjustComputeShader.SetInt("height", height);
        m_colorAdjustComputeShader.SetInt("toRemove",(int) m_removePixelPerUpdate);
        m_colorAdjustComputeShader.SetBuffer(kernelHandle, "pixels", m_pixelBuffer);
        m_colorAdjustComputeShader.Dispatch(kernelHandle, width / 8, height / 8, 1);

        m_pixelBuffer.GetData(color32s);
        pixelArray.CopyFrom(color32s);
  
    }

    private void OnDestroy()
    {
        if(m_pixelBuffer != null)
        m_pixelBuffer.Release();
    }
}
