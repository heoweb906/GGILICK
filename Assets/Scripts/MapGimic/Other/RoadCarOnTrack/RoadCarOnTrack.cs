using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCarOnTrack : CinemachineDollyCart
{
    void FixedUpdate()
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
