using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearobjEffect : MonoBehaviour
{
    public float DissolveSpeed = 0.01f;  // Dissolve �ӵ�
    public float FadeSpeed = 0.05f;      // ���� �� ���� �ӵ�
    public float DissolveYield = 0.1f;   // �ڷ�ƾ ��� �ð�

    public ParticleSystem Particle = null; // ��ƼŬ �ý���

    private MeshRenderer meshRenderer;
    private Material material;

    private float dissolveStart = -0.2f;
    private float dissolveEnd = 1.2f;

    private void Awake()
    {
        // ���� ������Ʈ���� MeshRenderer ������Ʈ ��������
        meshRenderer = GetComponent<MeshRenderer>();

        // MeshRenderer���� Material ��������
        if (meshRenderer != null)
        {
            material = meshRenderer.material;
        }
    }

    private void Update()
    {
        // �׽�Ʈ��: T Ű�� ������ Dissolve ȿ�� ����
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(DissolveCoroutine());
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        if (Particle != null)
        {
            Particle.Play();
        }

        float dissolveAmount = dissolveStart;
        float speedMultiplier = 1f;
        Color color = material.color; // ���� ���׸��� ���� ��������

        while (dissolveAmount < dissolveEnd)
        {
            dissolveAmount += DissolveSpeed * speedMultiplier;
            speedMultiplier += 0.1f;

            if (material != null)
            {
                // ���׸����� ���� ���� ����
                color.a -= FadeSpeed;
                color.a = Mathf.Clamp01(color.a); // ���� ���� 0�� 1 ���̷� �����ǵ��� ����
                material.color = color;
            }

            yield return new WaitForSeconds(DissolveYield);
        }

        Destroy(gameObject);
    }
}
