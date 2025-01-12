using UnityEngine;
using Unity.Collections;
using UnityEngine.Events;

public class ApplyNativeColorToTextureMono : MonoBehaviour
{
    public ComputeShader m_computeShader;
    public RenderTexture m_outputTexture;

    private NativeArray<Color32> m_colors;
    private ComputeBuffer m_colorBuffer;

    public int m_screenWidth = 1920;
    public int m_screenHeight = 1080;

    public UnityEvent<RenderTexture> m_onTextureChanged;


    public bool m_useScreenSize = false;


    void Start()
    {

        m_outputTexture = new RenderTexture(m_screenWidth, m_screenHeight, 0, RenderTextureFormat.ARGB32);
        m_outputTexture.enableRandomWrite = true;
        m_outputTexture.Create();
        if(m_useScreenSize)
        {
        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;

        }
        m_colors = new NativeArray<Color32>(m_screenWidth * m_screenHeight, Allocator.Persistent);

        // Fill the NativeArray with some color data (example: all white)
        for (int i = 0; i < m_colors.Length; i++)
        {
            m_colors[i] = new Color32(0,0,0,0);  // Example color
        }

        // Create a ComputeBuffer from the NativeArray
        m_colorBuffer = new ComputeBuffer(m_colors.Length, sizeof(uint));
        m_colorBuffer.SetData(m_colors);

        ApplyColorsToTexture();
    }

    [ContextMenu("Apply")]
    public void ApplyColorsToTexture()
    {
        int kernel = m_computeShader.FindKernel("CSMain");

        m_computeShader.SetBuffer(kernel, "colorBuffer", m_colorBuffer);
        m_computeShader.SetTexture(kernel, "resultTexture", m_outputTexture);
        m_computeShader.SetInt("width", m_screenWidth);
        m_computeShader.SetInt("height", m_screenHeight);

        int threadGroupsX = Mathf.CeilToInt(m_outputTexture.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(m_outputTexture.height / 8.0f);
        m_computeShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);

        m_onTextureChanged.Invoke(m_outputTexture);
    }
    public void Update()
    {
        ApplyColorsToTexture();
    }

    void OnDestroy()
    {
        // Release resources
        if (m_colorBuffer != null)
            m_colorBuffer.Release();
        if (m_colors.IsCreated)
            m_colors.Dispose();
    }

    public void SetColorWithNativeArray(NativeArray<Color32> colors)
    {
        if (m_colorBuffer == null)
            return;
        if (m_colors.IsCreated)
            m_colors.Dispose();
        this.m_colors = colors;
        m_colorBuffer.SetData(colors);
        ApplyColorsToTexture();
    }
}
