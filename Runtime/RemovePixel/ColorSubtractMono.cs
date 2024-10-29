using UnityEngine;
using Unity.Collections;

public class ApplyComputeShader : MonoBehaviour
{
    public ComputeShader m_colorAdjustComputeShader;
    private ComputeBuffer m_pixelBuffer;

    [SerializeField] int width = 1920;
    [SerializeField] int height = 1080;
    [SerializeField] int totalePixel = 1920 * 1080;
    public int m_pixelPerSecondsToRemove = 50;
    Color32[] color32s;

    public void RemovePixelOf(NativeArray<Color32> pixelArray)
    {
        width = Screen.width;
        height = Screen.height;
        totalePixel = width * height;

        if(m_pixelBuffer == null)
            m_pixelBuffer = new ComputeBuffer(totalePixel, sizeof(uint) * 4);
        if(color32s == null)
            color32s = new Color32[totalePixel];
        m_pixelBuffer.SetData(pixelArray);

        int kernelHandle = m_colorAdjustComputeShader.FindKernel("CSMain");
        m_colorAdjustComputeShader.SetInt("width", width);
        m_colorAdjustComputeShader.SetInt("height", height);
        m_colorAdjustComputeShader.SetInt("toRemove", m_pixelPerSecondsToRemove);
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
