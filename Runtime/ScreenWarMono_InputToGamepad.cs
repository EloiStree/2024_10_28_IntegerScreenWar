using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWarMono_InputToGamepad : MonoBehaviour
{
    public SNAM16K_ObjectBool m_isPlayerPlaying;
    public SNAM16K_ObjectGamepad2020 m_player;
    public int m_index=2501;
    public GamepadByteId2020Percent11 m_gamepad;


    public void SetLeftHorizontal(float value)=> m_gamepad.m_joystickLeftHorizontal = (value);
    public void SetLeftVertical(float value) => m_gamepad.m_joystickLeftVertical = (value);
    public void SetRightHorizontal(float value) => m_gamepad.m_joystickRightHorizontal = (value);
    public void SetRightVertical(float value) => m_gamepad.m_joystickRightVertical=(value);


    public void Update()
    {
        m_isPlayerPlaying.Set(m_index, true);
        m_player.Set(m_index, m_gamepad);
        
    }

}


