using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayerAnimationData
{
    [Header("State Group Parameter")]
    [SerializeField] private string groundParameterName = "Ground";
    [SerializeField] private string onAirParameterName = "OnAir";
    [SerializeField] private string moveStartParameterName = "MoveStart";
    [SerializeField] private string movingParameterName = "Move";
    [SerializeField] private string moveStopParameterName = "MoveStop";
    [SerializeField] private string landingParameterName = "Landing";


    [Header("Ground Group Parameter")]
    [SerializeField] private string idleParameterName = "isIdle";

    [SerializeField] private string walkStartParameterName = "isWalkStart";
    [SerializeField] private string runStartParameterName = "isRunStart";

    [SerializeField] private string walkingParameterName = "isWalking";
    [SerializeField] private string runningParameterName = "isRunning";

    [SerializeField] private string softStopParameterName = "isSoftStop";
    [SerializeField] private string hardStopParameterName = "isHardStop";

    [SerializeField] private string softLandingParameterName = "isSoftLanding";
    [SerializeField] private string hardLandingParameterName = "isHardLanding";
    [SerializeField] private string walkLandingParameterName = "isWalkLanding";
    [SerializeField] private string runLandingParameterName = "isRunLanding";

    [Header("Ground Group Parameter")]
    [SerializeField] private string jumpStartParameterName = "isJumpStart";
    [SerializeField] private string FallingParameterName = "isFalling";


    public int GroundParameterHash { get; private set; }
    public int OnAirParameterHash { get; private set; }
    public int MoveStartParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int MoveStopParameterHash { get; private set; }
    public int LandingParameterHash { get; private set; }

    public int IdleParameterHash { get; private set; }
    public int WalkStartParameterHash { get; private set; }
    public int RunStartParameterHash { get; private set; }
    public int WalkingParameterHash { get; private set; }
    public int RunningParameterHash { get; private set; }
    public int SoftStopParameterHash { get; private set; }
    public int HardStopParameterHash { get; private set; }
    public int SoftLandingParameterHash { get; private set; }
    public int HardLandingParameterHash { get; private set; }
    public int WalkLandingParameterHash { get; private set; }
    public int RunLandingParameterHash { get; private set; }

    public int JumpStartParameterHash { get; private set; }
    public int FallingParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        OnAirParameterHash = Animator.StringToHash(onAirParameterName);
        MoveStartParameterHash = Animator.StringToHash(moveStartParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        MoveStopParameterHash = Animator.StringToHash(moveStopParameterName);
        LandingParameterHash = Animator.StringToHash(landingParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        WalkStartParameterHash = Animator.StringToHash(walkStartParameterName);
        RunStartParameterHash = Animator.StringToHash(runStartParameterName);
        WalkingParameterHash = Animator.StringToHash(walkingParameterName);
        RunningParameterHash = Animator.StringToHash(runningParameterName);
        SoftStopParameterHash = Animator.StringToHash(softStopParameterName);
        HardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        SoftLandingParameterHash = Animator.StringToHash(softLandingParameterName);
        HardLandingParameterHash = Animator.StringToHash(hardLandingParameterName);
        WalkLandingParameterHash = Animator.StringToHash(walkLandingParameterName);
        RunLandingParameterHash = Animator.StringToHash(runLandingParameterName);
        JumpStartParameterHash = Animator.StringToHash(jumpStartParameterName);
        FallingParameterHash = Animator.StringToHash(FallingParameterName);
    }

}
