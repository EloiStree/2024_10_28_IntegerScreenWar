
using System.Runtime.Remoting.Contexts;
using UnityEngine;

namespace Eloi.SNAM
{

    [System.Serializable]
    public class SNAM16K_ObjectScreenPixelPosition: SNAM_Generic16KMono<STRUCT_PixelPosition>
    {

        [ContextMenu("Random Position")]
        public void RandomPosition() {

            int lenght = SNAM16K.ARRAY_MAX_SIZE;
            for (int i = 0; i < lenght; i++)
            {
                Set(i, new STRUCT_PixelPosition()
                {
                    m_xLeftRight = (short)Random.Range(0, Screen.width),
                    m_yDownTop = (short)Random.Range(0, Screen.height)
                });
            }
        }
    }
}
