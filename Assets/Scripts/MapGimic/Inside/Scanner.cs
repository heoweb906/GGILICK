using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scanner : MonoBehaviour
{
    public ColorType colorType;
    private ColorObj colorObj;


    // 기능 확인용으로 있는 스크립트, 나중에 지워야함
    public GameObject testEffect;



    private Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    // #. 스캐너 위에 있는 ColorObj의 정보를 가져옴
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
