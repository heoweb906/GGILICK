using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SetIdle();
        else if (Input.GetKeyDown(KeyCode.E))
            SetUnControllable_Idle();
    }

    public void SetIdle()
    {
        player.machine.OnStateChange(player.machine.IdleState);
    }

    public void SetUnControllable_Idle()
    {
        player.machine.OnStateChange(player.machine.UC_IdleState);
    }
}
