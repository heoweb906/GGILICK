using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "CameraBlendData", menuName = "ScriptableObjects/CameraBlendData", order = 1)]
public class CameraBlendData : ScriptableObject
{
    public CinemachineBlendDefinition.Style blendStyle; // ���� ��Ÿ��
    public float duration; // ���� �ð�

    // ���� ������ ����ϴ� �޼��� (���� ����)
    public void PrintBlendInfo()
    {
        Debug.Log($"���� ��Ÿ��: {blendStyle}, ���� �ð�: {duration}��");
    }
}