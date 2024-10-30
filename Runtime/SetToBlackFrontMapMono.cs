using UnityEngine;

public class SetToBlackFrontMapMono : MonoBehaviour {


    public ScreenMapAsNativeColor32Mono m_screenMapFront;

    [ContextMenu("Execute")]
    public void Execute() { 
        m_screenMapFront.FlushToBlackTransparent();
    }
}
