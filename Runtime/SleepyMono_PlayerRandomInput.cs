using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi.SNAM;
using UnityEngine.Events;
public class SleepyMono_PlayerRandomInput : MonoBehaviour
{

    public SNAM16K_ObjectInt m_playerInput;
    public int m_playerCount = 100;
    public UnityEvent<int> m_onPlayerCountChanged;


    private void Awake()
    {
        m_onPlayerCountChanged.Invoke(m_playerCount);
    }

    void Update()
    {
        for (int i = 0; i < m_playerCount; i++)
        {

            int value = Random.Range(0, 9)*100;
            value+= Random.Range(0, 9)*10;
            value+= Random.Range(0, 9);
            m_playerInput.Set(i, value);
        }
        
    }
}
