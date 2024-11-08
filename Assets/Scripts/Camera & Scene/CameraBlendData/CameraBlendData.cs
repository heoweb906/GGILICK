using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "CameraBlendData", menuName = "ScriptableObjects/CameraBlendData", order = 1)]
public class CameraBlendData : ScriptableObject
{
    public CinemachineBlendDefinition.Style blendStyle; // 블렌드 스타일
    public float duration; // 지속 시간

    // 블렌드 정보를 출력하는 메서드 (선택 사항)
    public void PrintBlendInfo()
    {
        Debug.Log($"블렌드 스타일: {blendStyle}, 지속 시간: {duration}초");
    }
}