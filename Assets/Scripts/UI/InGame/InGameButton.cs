using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected InGameUIController ingameUIController;
    public TMP_Text textButton;

    protected float fButtonAnimtionDelay = 0.3f;

    public bool bSelect = false;

    private void Awake()
    {
        ingameUIController = FindObjectOfType<InGameUIController>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (ingameUIController.nowPlayerButton != null)
        {
            ingameUIController.nowPlayerButton.SelectButtonOff();
        }

        if (ingameUIController.ingameButtons != null)
        {
            foreach (var item in ingameUIController.ingameButtons)
            {
                item.SelectButtonOff();
            }
        }

        SelectButtonOn();
    }


    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SelectButtonOff();
    }

    // #. MenuButton�� ��� ���� ��ư���� ���� ����� ���⿡ �� �����ϴ� ����
    public virtual void ImplementButton()
    {
        SelectButtonOff();
    }

    public virtual void SelectButtonOn()
    {
        DOTween.Kill(gameObject, true); 

        ingameUIController.nowPlayerButton = this;
    }

    // #. ��ư�� ��Ȱ��ȭ �Ǿ��� �� ���� �׼��� ������ ���� �Լ�
    // �� ��ư ���� �ٸ� ȿ���� �� �� �����Ƿ� ������ �ڽĿ��� �ۼ�
    public virtual void SelectButtonOff()
    {
        ingameUIController.nowPlayerButton = null;
        ingameUIController.lastButton = this;
    }


}
