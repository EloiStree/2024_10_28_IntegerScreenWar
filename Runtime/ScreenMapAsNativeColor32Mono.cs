using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenMapAsNativeColor32Mono : MonoBehaviour { 

    public NativeArray<Color32> m_colorNativeArray;
    public Texture2D m_textureOfMap;
    
    public int m_screenWidth = 1920;
    public int m_screenHeight = 1080;
    public int m_pixelCount;

    public UnityEvent<Texture2D> m_onTextureApplyed;

    public void SetColor(in int index, in Color32 color)
    {
        m_colorNativeArray[index] = color;
    }

    public void GetScreenWidth(out int width)
    {
        width = m_screenWidth;
    }
    public void GetScreenHeight(out int height)
    {
        height = m_screenHeight;
    }
    public void GetPixelCount(out int count)
    {
        count = m_pixelCount;
    }
    public void GetNativeArray(out NativeArray<Color32> array)
    {
        array = m_colorNativeArray;
    }

    [ContextMenu("Apply")]
    public void ApplyColor32ToTexture() { 
    
        m_textureOfMap.SetPixelData(m_colorNativeArray, 0);
        m_textureOfMap.Apply();
        m_onTextureApplyed.Invoke(m_textureOfMap);
    }

    public bool IsInitialized() { 
         return m_colorNativeArray.IsCreated;
    }

    public bool mipmap;
    public bool linear;
    public void Awake()
    {
        m_screenWidth = Screen.width;   
        m_screenHeight = Screen.height;
        m_pixelCount = m_screenWidth * m_screenHeight;
        m_colorNativeArray = new NativeArray<Color32>(m_screenWidth * m_screenHeight, Allocator.Persistent);
        m_emptyMap = new Color32[m_screenWidth * m_screenHeight];
        for(int i = 0; i < m_emptyMap.Length; i++)
        {
            m_emptyMap[i] = new Color32(0, 0, 0, 0);
        }
        for (int i = 0; i < m_colorNativeArray.Length; i++)
        {
            m_colorNativeArray[i] = new Color32(0, 0, 0, 0);
        }
        m_textureOfMap = new Texture2D(m_screenWidth, m_screenHeight, TextureFormat.RGBA32, mipmap,linear);
        m_textureOfMap.filterMode = FilterMode.Point;
        m_textureOfMap.wrapMode = TextureWrapMode.Clamp;
        ApplyColor32ToTexture();
    }

    public void OnDestroy()
    {
        if (m_colorNativeArray.IsCreated)
            m_colorNativeArray.Dispose();
    }

    public NativeArray<Color32> GetNativeArray()
    {
        return m_colorNativeArray;
    }

    public int GetScreenWidth()
    {
        return m_screenWidth;
    }

    public int GetScreenHeight()
    {
        return m_screenHeight;
    }

    public int GetPixelCount()
    {
        return m_pixelCount;
    }

    public bool m_applyAtLateUpdate = true;
    public void LateUpdate()
    {
        if (m_applyAtLateUpdate)
            ApplyColor32ToTexture();
    }

    Color32 [] m_emptyMap;
    public void FlushToBlackTransparent()
    {
      
        m_colorNativeArray.CopyFrom(m_emptyMap);
    }

    public Color32 GetAsInt(int xLeftRight, int yDownTop)
    {
        return m_colorNativeArray[yDownTop * m_screenWidth + xLeftRight];
    }
    public Color32 GetAsRound(float xLeftRight, float yDownTop)
    {
        return m_colorNativeArray[Mathf.RoundToInt(yDownTop) * m_screenWidth + Mathf.RoundToInt(xLeftRight)];
    }
    public Color32 GetAsInt(float xLeftRight, float yDownTop)
    {
        return GetAsInt((int)xLeftRight, (int)yDownTop);
    }

    public void SetColor(int xLeftRight, int yDownTop, Color m_ledColor)
    {
        if(xLeftRight>=0 && xLeftRight < m_screenWidth && yDownTop >= 0 && yDownTop < m_screenHeight)
            m_colorNativeArray[yDownTop * m_screenWidth + xLeftRight] = m_ledColor;
    }
} 
