using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class SetToBlackFrontMapMono : MonoBehaviour {


    public ScreenMapAsNativeColor32Mono m_screenMapFront;
    //public bool m_useComplete=true;

    [ContextMenu("Execute")]
    public void Execute() { 


        m_screenMapFront.FlushToBlackTransparent();


        //18 milliseconds vs 0.5 milliseconds
        //NativeArray<Color32> screenMapFrontRef = m_screenMapFront.GetNativeArray();
        //STRUCTJOB_SetToBlack job = new STRUCTJOB_SetToBlack()
        //{
        //    m_screenMapFrontRef = screenMapFrontRef
        //};
        //JobHandle jh = job.Schedule(screenMapFrontRef.Length, 64);
        //if(m_useComplete)
        //    jh.Complete();
    }


    //public struct STRUCTJOB_SetToBlack : IJobParallelFor
    //{
    //    [WriteOnly]
    //    public Unity.Collections.NativeArray<Color32> m_screenMapFrontRef;
    //    public void Execute(int index)
    //    {
    //        m_screenMapFrontRef[index] = Color.black;
    //    }
    //}
}
