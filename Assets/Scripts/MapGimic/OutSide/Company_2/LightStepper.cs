using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightStepper : MonoBehaviour
{
    public bool bStep;
    private Renderer renderer;
    private Material matMine;
   
    public Material[] materials;

    private bool isCooldown; // ��ٿ� ���¸� üũ�ϴ� ����

    private void Awake()
    {
        // Renderer���� ���� Material�� ������ (���� Material �ν��Ͻ� ����)
        renderer = GetComponent<Renderer>();
        matMine = renderer.material; // �ʱ� Material ����

        if (bStep) renderer.material = materials[0]; // ù ��° Material�� ����
        else renderer.material = materials[1]; // �� ��° Material�� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldown)
        {
            Debug.Log("Step!");
            isCooldown = true;
            bStep = !bStep;

            renderer = GetComponent<Renderer>();
            if (bStep) renderer.material = materials[0]; // ù ��° Material�� ����
            else renderer.material = materials[1]; // �� ��° Material�� ����

            // ���� Material ������Ʈ
            matMine = renderer.material;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCooldown = false;
    }


}
