using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RoadCarOnTrack : CinemachineDollyCart
{

    private void FixedUpdate()
    {
        if(m_Path != null)
        {
            var trackLength = m_Path.PathLength;

            if (m_Path.Looped)
            {
                m_Position = m_Position % trackLength;
            }
            else if (m_Position >= trackLength)
            {
                Destroy(gameObject);
            }
        }
       
    }
    
}
