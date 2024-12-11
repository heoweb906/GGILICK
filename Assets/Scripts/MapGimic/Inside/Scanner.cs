using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scanner : MonoBehaviour
{
    public ColorType colorType;
    private ColorObj colorObj;


    // ��� Ȯ�ο����� �ִ� ��ũ��Ʈ, ���߿� ��������
    public GameObject testEffect;



    private Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    // #. ��ĳ�� ���� �ִ� ColorObj�� ������ ������
    public ColorObj GetColorObj()
    {
        if (colorObj != null) return colorObj;
        return null;
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();

            if(colorObj_.colorType == colorType)
            {
                testEffect.SetActive(true);
                colorObj = colorObj_;
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ColorObj>() != null)
        {
            ColorObj colorObj_ = other.GetComponent<ColorObj>();

            if (colorObj_.colorType == colorType)
            {
                testEffect.SetActive(false);
                colorObj = colorObj_;
            }
        }
    }




}
