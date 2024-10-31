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
    [SerializeField] private string interactionParameterName = "Interaction";
    [SerializeField] private string climbingParameterName = "Climbing";
    [SerializeField] private string moveStartParameterName = "MoveStart";
    [SerializeField] private string movingParameterName = "Move";
    [SerializeField] private string moveStopParameterName = "MoveStop";
    [SerializeField] private string landingParameterName = "Landing";

    [SerializeField] private string jumpStartParameterName = "isJumpStart";
    [SerializeField] private string FallingParameterName = "isFalling";

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
    [SerializeField] private string moveLandingParameterName = "isMoveLanding";
    [SerializeField] private string runLandingParameterName = "isRunLanding";

    [Header("OnAir Group Parameter")]
    [SerializeField] private string jumpStartIdleParameterName = "isJumpStartIdle";
    [SerializeField] private string jumpStartMoveParameterName = "isJumpStartMove";
    [SerializeField] private string fallingIdleParameterName = "isFallingIdle";
    [SerializeField] private string fallingMoveParameterName = "isFallingMove";

    [Header("Climbing Group Parameter")]
    [SerializeField] private string HangingParameterName = "isHanging";
    [SerializeField] private string ClimbingToTopParameterName = "isClimbingToTop";

    [Header("Interaction Group Parameter")]
    [SerializeField] private string spinClockWorkParameterName = "isSpinClockWork";
    [SerializeField] private string pickUpParameterName = "isPickUp";
    [SerializeField] private string putDownParameterName = "isPutDown";


    public int GroundParameterHash { get; private set; }
    public int OnAirParameterHash { get; private set; }
    public int InteractionParameterHash { get; private set; }
    public int ClimbingParameterHash { get; private set; }
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
    public int MoveLandingParameterHash { get; private set; }
    public int RunLandingParameterHash { get; private set; }

    public int JumpStartParameterHash { get; private set; }
    public int FallingParameterHash { get; private set; }

    public int JumpStartIdleParameterHash { get; private set; }
    public int JumpStartMoveParameterHash { get; private set; }
    public int FallingIdleParameterHash { get; private set; }
    public int FallingMoveParameterHash { get; private set; }

    public int HangingParameterHash { get; private set; }
    public int ClimbingToTopParameterHash { get; private set; }

    public int SpinClockWorkParameterHash { get; private set; }
    public int PickUpParameterHash { get; private set; }
    public int PutDownParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        OnAirParameterHash = Animator.StringToHash(onAirParameterName);
        InteractionParameterHash = Animator.StringToHash(interactionParameterName);
        ClimbingParameterHash = Animator.StringToHash(climbingParameterName);
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
        MoveLandingParameterHash = Animator.StringToHash(moveLandingParameterName);
        RunLandingParameterHash = Animator.StringToHash(runLandingParameterName);
        JumpStartParameterHash = Animator.StringToHash(jumpStartParameterName);
        FallingParameterHash = Animator.StringToHash(FallingParameterName);
        JumpStartIdleParameterHash = Animator.StringToHash(jumpStartIdleParameterName);
        JumpStartMoveParameterHash = Animator.StringToHash(jumpStartMoveParameterName);
        FallingIdleParameterHash = Animator.StringToHash(fallingIdleParameterName);
        FallingMoveParameterHash = Animator.StringToHash(fallingMoveParameterName);

        HangingParameterHash = Animator.StringToHash(HangingParameterName);
        ClimbingToTopParameterHash = Animator.StringToHash(ClimbingToTopParameterName);

        SpinClockWorkParameterHash = Animator.StringToHash(spinClockWorkParameterName);
        PickUpParameterHash = Animator.StringToHash(pickUpParameterName);
        PutDownParameterHash = Animator.StringToHash(putDownParameterName);
    }

}