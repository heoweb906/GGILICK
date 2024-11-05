using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class MainMenuController : MonoBehaviour
{

    public MenuButton nowPlayerButton; // 현재 선택되어 있는 버튼
    public MenuButton lastButton;
    public MenuButton[] menuButtons;

    [SerializeField] private int nowPanelNum;


    private GameObject PanelNow;
    [Header("Main Panel")]
    public GameObject Panel_Main; // Panel Number = 0;

    [Header("Option Panel")]
    public GameObject Panel_Option; // Panel Number = 1;

    [Header("Other Panel")]
    public GameObject Panel_Other; // Panel Numebr 5;
    public GameObject Panel_Resolution; // Panel Numer 8;
    public GameObject Panel_Other_Production; // Panel Number 6;
    public GameObject Panel_Other_Sources; // Panel Number 7;
    



    [Header("UI 애니메이션 부드러움 수치")]
    private Vignette vignette;
    public GameObject image_FadeOut; // FadeOut에 사용할 Image
    public bool bIsUIDoing; // UI가 뭔가 기능 중임
    public float duration; // 애니메이션 지속 시간
    public float maxScale; // 최대 크기 (커질 때의 크기)





    private void Awake()
    {
        PanelChage(0);
    }
    private void Update()
    {
        if(!bIsUIDoing)
        {
            InputKey();
        }
        
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nowPlayerButton != null) nowPlayerButton.ImplementButton();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FindClosestButton(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FindClosestButton(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FindClosestButton(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FindClosestButton(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && nowPanelNum != 0)
        {
            if (nowPanelNum == 1)
            {
                Panel_Option.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(0);
            }

            if (nowPanelNum == 5)
            {
                Panel_Other.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(0);
            }

            if (nowPanelNum == 6)
            {
                Panel_Other_Production.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(5);
            }
            if (nowPanelNum == 7)
            {
                Panel_Other_Sources.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(5);
            }
            if (nowPanelNum == 8)
            {
                Panel_Resolution.SetActive(false);
                if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();
                PanelChage(1);
            }


        }
    }

    // #. Arrow Key를 이용해서 현재 선택되어 있는 UI Button에서 가장 가까이에 있는 UI Button을 선택하도록 하는 함수
    private void FindClosestButton(Vector2 direction)
    {
        if (nowPlayerButton == null)
        {
            nowPlayerButton = lastButton;
            nowPlayerButton.SelectButtonOn();
            return;
        }

        float closestDistance = Mathf.Infinity;
        MenuButton closestButton = null;
        Vector2 currentPosition = nowPlayerButton.transform.position;

        foreach (MenuButton button in menuButtons)
        {
            if (button == nowPlayerButton) continue;

            Vector2 directionToButton = (Vector2)button.transform.position - currentPosition;

            // 정확한 방향 탐색: 지정된 방향과의 각도가 30도 이내일 때만 선택
            float angle = Vector2.Angle(direction, directionToButton);
            if (angle <= 45f) // 30도 범위로 제한
            {
                float distance = directionToButton.magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestButton = button;
                }
            }
        }

        if (closestButton != null)
        {
            nowPlayerButton.SelectButtonOff();
            nowPlayerButton = closestButton;
            nowPlayerButton.SelectButtonOn();
        }
    }



    // #. Panel이 활성화 될 때 하위 버튼 항목들을 변경해줌
    public void PanelChage(int index)
    {
        nowPlayerButton = null;

        switch (index)
        {
            case 0: // Main Panel 켜기
                Panel_Main.SetActive(true);
                PanelNow = Panel_Main;
                break;
            case 1: // Option Panel 켜기
                Panel_Option.SetActive(true);
                PanelNow = Panel_Option;
                break;

            case 5: 
                Panel_Other.SetActive(true);
                PanelNow = Panel_Other;
                break;

            case 6: 
                Panel_Other_Production.SetActive(true);
                PanelNow = Panel_Other_Production;
                break;

            case 7: 
                Panel_Other_Sources.SetActive(true);
                PanelNow = Panel_Other_Sources;
                break;

            case 8: 
                Panel_Resolution.SetActive(true);
                PanelNow = Panel_Resolution;
                break;

            case 999:
                return;
   
            default:
                break;
        }

        PanelOn(PanelNow);
        nowPanelNum = index;

        // Panel의 모든 하위 GameObject들을 가져옴
        Transform[] childTransforms = PanelNow.GetComponentsInChildren<Transform>(true);

        // MenuButton 스크립트를 상속받은 컴포넌트들을 찾아서 menuButtons 배열에 할당
        List<MenuButton> foundButtons = new List<MenuButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // 하위 GameObject에서 MenuButton 스크립트를 상속받은 컴포넌트를 찾음
            MenuButton menuButton = childTransform.GetComponent<MenuButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List를 배열로 변환하여 menuButtons에 할당
        menuButtons = foundButtons.ToArray();
        if (menuButtons.Length != 0) lastButton = menuButtons[0];
        lastButton = menuButtons[0];
        bIsUIDoing = false;
    }

    public void PanelOff(int index)
    {
        // # 연출 1번
        PanelNow.SetActive(false);
        PanelChage(index);

        // #.연출 2번
        //PanelNow.transform.DOScale(maxScale, duration / 2)
        //.SetEase(Ease.OutBack) // EaseOutBack 효과로 커짐
        //.OnComplete(() => // 커진 후에 크기를 0으로 줄임
        //{
        //    PanelNow.transform.DOScale(Vector3.zero, duration / 2)
        //        .SetEase(Ease.InBack)
        //        .OnComplete(() => // 크기가 0이 된 후에 SetActive(false) 실행
        //        {
        //            PanelNow.SetActive(false);
        //            PanelChage(index);
        //        });
        //});
    }
    public void PanelOn(GameObject ActivePanel)
    {
        // 연출 1번
        //ActivePanel.transform.localScale = Vector3.one * 0.5f;
        //ActivePanel.transform.DOScale(Vector3.one, duration / 2)
        //.SetEase(Ease.OutBack); // EaseOutBack 효과로 자연스럽게 커짐


        // 연출 2번
        Image[] images = ActivePanel.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] textMeshes = ActivePanel.GetComponentsInChildren<TextMeshProUGUI>();

        // 각 Image의 알파값을 0에서 1까지 서서히 변화
        foreach (Image img in images)
        {
            // Image의 초기 알파값을 0으로 설정
            Color tempColor = img.color;
            tempColor.a = 0f;
            img.color = tempColor;

            // 알파값을 0에서 1로 1초 동안 서서히 올림
            DOTween.To(() => img.color.a, x => {
                tempColor.a = x;
                img.color = tempColor;
            }, 1f, duration).SetEase(Ease.Linear).SetUpdate(true); // 1초 동안 알파값을 1로 만듦
        }

        // 각 TextMeshPro의 색상을 0에서 1까지 서서히 변화
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            // TextMeshPro의 초기 알파값을 0으로 설정
            Color textColor = textMesh.color;
            textColor.a = 0f;
            textMesh.color = textColor;

            // 텍스트 색상 알파값을 0에서 1로 1초 동안 서서히 올림
            DOTween.To(() => textMesh.color.a, x => {
                textColor.a = x;
                textMesh.color = textColor;
            }, 1f, duration).SetEase(Ease.Linear).SetUpdate(true);
        }



    }








    /// <summary>
    /// 다른 씬과의 연결, 혹은 기타 작업들
    /// </summary>

    // #. Scene 전환 함수, 게임 시작 버튼에서 사용하지만, 버튼에는 버튼 관련 기능만 넣기 위해서
    public void StartNewGame()
    {
        // Vignette의 intensity를 현재 값에서 1로 서서히 변화
        if (vignette != null)
        {
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 2.5f)
                .SetEase(Ease.OutQuad);

            // Vignette의 smoothness를 서서히 1로 변화, 마지막에 감속
            DOTween.To(() => vignette.smoothness.value, x => vignette.smoothness.value = x, 1f, 2.5f)
                .SetEase(Ease.OutQuad);
        }

        image_FadeOut.SetActive(true);
        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // 알파값을 서서히 1로, 마지막에 감속 후 씬 전환
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 2.5f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            // 애니메이션이 끝난 후 'Chapter_1' 씬으로 전환
            SceneManager.LoadScene("Chapter_1_City1");
        });
    }


}
