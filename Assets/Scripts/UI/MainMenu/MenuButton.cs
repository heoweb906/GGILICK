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

    private void Awake()
    {
        mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (mainMenuController.nowPlayerButton != null)
        {
            mainMenuController.nowPlayerButton.SelectButtonOff();
        }
        SelectButtonOn();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SelectButtonOff();
    }

    // #. MenuButton을 상속 받은 버튼들의 실행 기능을 여기에 다 구현하는 거임
    public virtual void ImplementButton()
    {
        SelectButtonOff();
    }

    // #. 버튼이 활성화 되었을 때 취할 액션의 내용을 담을 함수
    // 각 버튼 별로 다른 효과를 줄 수 있으므로 내용은 자식에서 작성
    public virtual void SelectButtonOn()
    {
        mainMenuController.nowPlayerButton = this;
    }

    // #. 버튼이 비활성화 되었을 때 취할 액션의 내용을 담을 함수
    // 각 버튼 별로 다른 효과를 줄 수 있으므로 내용은 자식에서 작성
    public virtual void SelectButtonOff()
    {
        mainMenuController.nowPlayerButton = null;
        mainMenuController.lastButton = this;
    }



}
