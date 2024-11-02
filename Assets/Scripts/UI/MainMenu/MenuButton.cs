using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected MainMenuController mainMenuController;
    public TMP_Text textButton;
 
    public bool bSelect = false;

    protected void Awake()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (mainMenuController.nowPlayerButton != null)
        {
            mainMenuController.nowPlayerButton.SelectButtonOff();
        }

        if(mainMenuController.menuButtons != null)
        {
            foreach(var item in mainMenuController.menuButtons)
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

    // #. ��ư�� Ȱ��ȭ �Ǿ��� �� ���� �׼��� ������ ���� �Լ�
    // �� ��ư ���� �ٸ� ȿ���� �� �� �����Ƿ� ������ �ڽĿ��� �ۼ�
    public virtual void SelectButtonOn()
    {
        DOTween.Kill(gameObject);
        mainMenuController.nowPlayerButton = this;
    }

    // #. ��ư�� ��Ȱ��ȭ �Ǿ��� �� ���� �׼��� ������ ���� �Լ�
    // �� ��ư ���� �ٸ� ȿ���� �� �� �����Ƿ� ������ �ڽĿ��� �ۼ�
    public virtual void SelectButtonOff()
    {
        mainMenuController.nowPlayerButton = null;
        mainMenuController.lastButton = this;
    }



}
