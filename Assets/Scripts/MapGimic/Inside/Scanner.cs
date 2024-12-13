using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scanner : MonoBehaviour
{
    public List<ColorObj> colorObjList = new List<ColorObj>();


    // ��� Ȯ�ο����� �ִ� ��ũ��Ʈ, ���߿� ��������
    public GameObject testEffect;


    // #. ��ĳ�� ���� �ִ� ColorObjList�� ������ ������
    public List<ColorObj> GetColorObjList()
    {
        if (colorObjList != null) return colorObjList;
        return null;
    }



    // #. Ư���� �÷��� ������ ColorObj�� ��� ���������� �Լ�
    public void ThrowOtherColorObj(ColorType colorType = ColorType.None)
    {
        for (int i = colorObjList.Count - 1; i >= 0; i--)
        {
            if (colorObjList[i] != null && colorObjList[i].colorType != colorType)
            {
                Rigidbody objRigidbody = colorObjList[i].GetComponent<Rigidbody>();
                if (objRigidbody == null) objRigidbody = colorObjList[i].gameObject.AddComponent<Rigidbody>();
                
                Vector3 throwDirection = (transform.forward * -1f + transform.up * 3f).normalized;
                objRigidbody.AddForce(throwDirection * 40f, ForceMode.Impulse);
            }
        }

    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();

            colorObjList.Add(colorObj_);
            testEffect.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();

            if (colorObjList.Contains(colorObj_))
            {
                colorObjList.Remove(colorObj_);
            }

            if (colorObjList.Count == 0)
            {
                testEffect.SetActive(false);
            }
        }
    }




}
