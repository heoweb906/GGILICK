using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyClockWorkAssist : MonoBehaviour, IPartsOwner
{
    public GameObject TruckClockWork;   // Ȱ��ȭ �� Ʈ�� �¿�

    public void InsertOwnerFunc(GameObject soundPieceObj, int index)
    {
        // ��� �ڿ� ClockWork ������Ʈ�� �ٲ����
        StartCoroutine(ChangeLayerWithDelay(1.1f));
    }
    private IEnumerator ChangeLayerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TruckClockWork.SetActive(true);
        Destroy(gameObject);
    }



    public void RemoveOwnerFunc(int index)
    {
       
    }
}
