using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWarTexture2DMono : MonoBehaviour
{
    public UnityEvent<Texture2D> m_onMapChanged;
    public Texture2D m_createdMap;
    public bool m_mipmap;
    public bool m_linear;


    public int m_screenWidth;
    public int m_screenHeight;
    public NativeArray<Color32> m_colorNativeArray;

    public void GetCreatedTexture(out Texture2D texture) {
        texture = m_createdMap;
    }
    public bool m_useScreenSize = false;

    public void OnEnable()
    {
        if (m_useScreenSize)
        {
            m_screenWidth = Screen.width;
            m_screenHeight = Screen.height;
        }

        m_createdMap = new Texture2D(Screen.width, Screen.height,TextureFormat.RGBA32,m_mipmap, m_linear);
        m_createdMap.filterMode = FilterMode.Point;
        m_createdMap.wrapMode = TextureWrapMode.Clamp;
        SlowCleanMap(m_createdMap);
        AddRandomYellowPoints();
        m_createdMap.Apply();
        m_onMapChanged.Invoke(m_createdMap);
    }

    public void SetColor(NativeArray<Color32> colors)
    {
        m_colorNativeArray = colors;
        Refresh();
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if(m_createdMap==null)
            return;
        m_createdMap.SetPixelData(m_colorNativeArray, 0, 0);
        m_createdMap.Apply();
        m_onMapChanged.Invoke(m_createdMap);
    }

    [ContextMenu("Slow Clean with points for testing")]
    public void SlowCleanMapWithAddRandomPoints()
    {
        SlowCleanMap(m_createdMap);
        AddRandomYellowPoints();
        m_createdMap.Apply();
    }

    private void AddRandomYellowPoints()
    {
        for (int i = 0; i < 100; i++)
        {
            int x = UnityEngine.Random.Range(0, m_createdMap.width);
            int y = UnityEngine.Random.Range(0, m_createdMap.height);
            m_createdMap.SetPixel(x, y, Color.yellow);
        }
    }

    public void OnDisable()
    {
        SlowCleanMap(m_createdMap);
        m_createdMap.Apply();
        m_onMapChanged.Invoke(m_createdMap);
    }

    private void SlowCleanMap(Texture2D createdMap)
    {
        Color32[] colors  = createdMap.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color32(0, 0, 0, 0);
        }
        createdMap.SetPixels32(colors);
    }
}
