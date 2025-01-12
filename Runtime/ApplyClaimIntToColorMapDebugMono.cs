using UnityEngine;

public class ApplyClaimIntToColorMapDebugMono : MonoBehaviour {

    public ScreenMapAsClaimIntNativeIndexMono m_claimMap;
    public Texture2D m_texture;
    public SNAM16K_ObjectColor32 m_playerColor;

    [ContextMenu("Refresh")]
    private void SlowRefresh()
    {
        if(m_claimMap.m_screenHeight != m_texture.height || m_claimMap.m_screenWidth != m_texture.width)
        {
            m_texture = new Texture2D(m_claimMap.m_screenWidth, m_claimMap.m_screenHeight);
            m_texture.filterMode = FilterMode.Point;
            m_texture.wrapMode = TextureWrapMode.Clamp;
        }
        for (int i = 0; i < m_claimMap.m_pixelCount; i++)
        {
            int owner= m_claimMap.WhoOwns(i);
            Color32 color = m_playerColor.Get(owner);
            int x = i % m_claimMap.m_screenWidth;
            int y = i / m_claimMap.m_screenWidth;
            m_texture.SetPixel(x, y, color);
        }
        m_texture.Apply();
    }
}
