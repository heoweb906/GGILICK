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
using System.Linq;
using TMPro;

public class InGameUIController : MonoBehaviour
{
    public InGameButton nowPlayerButton; // 현재 선택되어 있는 버튼
    public InGameButton lastButton;
    public InGameButton[] ingameButtons;

    [SerializeField] private int nowPanelNum;

    private GameObject PanelNow;
    private bool bUIOnOff;
    // Panel Number = 643  / Panel Off 상태
    [Header("InGameUI Panel")]
    public GameObject Panel_InGameUI; // Panel Number = 0;

    [Header("Option Panel")]
    public GameObject Panel_Option; // Panel Number = 1;


    [Header("UI 애니메이션 부드러움 수치")]
    public Volume volume_global; // 전역 Volume 컴포넌트
    public GameObject image_FadeOut; // UI 활성화시에 사용할 이미지
    public GameObject image_FadeOut_ForReturn; // 씬 전환에 사용할 검은색 이미지
    public bool bIsUIDoing; // UI가 뭔가 기능 중임
    public float duration; // 애니메이션 지속 시간

    private void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (nowPlayerButton != null) nowPlayerButton.ImplementButton();
        }


        if (bUIOnOff)
        {
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
        }

        // #. ESC키는 따로 관리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (nowPanelNum == 643)
            {
                OnInGameUI();
            }
            else if (nowPanelNum == 0) OffInGameUI();

            else if (nowPanelNum == 1)
            {
                Panel_Option.SetActive(false);
                PanelChage(0);
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
        InGameButton closestButton = null;
        Vector2 currentPosition = nowPlayerButton.transform.position;

        foreach (InGameButton button in ingameButtons)
        {
            if (button == nowPlayerButton) continue;

            Vector2 directionToButton = (Vector2)button.transform.position - currentPosition;
            if (Vector2.Dot(directionToButton.normalized, direction) > 0.5f)
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
                Panel_InGameUI.SetActive(true);
                PanelNow = Panel_InGameUI;
                break;
            case 1: // Option Panel 켜기
                Panel_Option.SetActive(true);
                PanelNow = Panel_Option;
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
        List<InGameButton> foundButtons = new List<InGameButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // 하위 GameObject에서 MenuButton 스크립트를 상속받은 컴포넌트를 찾음
            InGameButton menuButton = childTransform.GetComponent<InGameButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List를 배열로 변환하여 menuButtons에 할당
        ingameButtons = foundButtons.ToArray();
        if (ingameButtons.Length != 0) lastButton = ingameButtons[0];
        bIsUIDoing = false;
    }



    public void PanelOff(int index)
    {
        PanelNow.SetActive(false);
        PanelChage(index);
    }
    public void PanelOn(GameObject ActivePanel)
    {
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

    // #. UI Off
    public void OffInGameUI()
    {
        Time.timeScale = 1f;
        if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();

        FadeInOutImage(0f);
        PanelNow.SetActive(false);
        nowPanelNum = 643;
        bUIOnOff = false;
    }


    // #. UI On
    public void OnInGameUI()
    {
        PanelChage(0);
        FadeInOutImage(0.9f);
        bUIOnOff = true;
        Time.timeScale = 0f;
    }


    // #. UI Panel 활성 여부 확인
    public bool GetbUIOnOff()
    {
        return bUIOnOff;
    }



    /// <summary>
    /// 다른 씬과의 연결, 혹은 기타 작업들
    /// </summary>

    // FadeInOutImage 알파값 조절 함수
    private void FadeInOutImage(float fTargetAlpha) // 매개변수는 목표 수치
    {
        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // 알파값을 fTargetAlpha까지 duration 동안 올리는 애니메이션
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, fTargetAlpha, duration)
        .SetEase(Ease.Linear)
        .SetUpdate(true); // Time.timeScale의 영향을 받지 않도록 SetUpdate(true) 설정
    }


    // #. Scene 전환 함수, main 화면으로 돌아가기
    public void ReturnMainMenu()
    {
        image_FadeOut_ForReturn.SetActive(true);
        Image fadeoutImage = image_FadeOut_ForReturn.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // 알파값을 서서히 1로, 마지막에 감속 후 씬 전환
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 1.5f)
        .SetEase(Ease.OutQuad).SetUpdate(true)
        .OnComplete(() => {
            // 씬 전환 직전에 Time.timeScale을 1로 돌림
            Time.timeScale = 1f;

            // 애니메이션이 끝난 후 'MainMenu' 씬으로 전환
            SceneManager.LoadScene("MainMenu");
        });
    }
}
