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

    public MenuButton nowPlayerButton; // ���� ���õǾ� �ִ� ��ư
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
    



    [Header("UI �ִϸ��̼� �ε巯�� ��ġ")]
    private Vignette vignette;
    public GameObject image_FadeOut; // FadeOut�� ����� Image
    public bool bIsUIDoing; // UI�� ���� ��� ����
    public float duration; // �ִϸ��̼� ���� �ð�
    public float maxScale; // �ִ� ũ�� (Ŀ�� ���� ũ��)





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

    // #. Arrow Key�� �̿��ؼ� ���� ���õǾ� �ִ� UI Button���� ���� �����̿� �ִ� UI Button�� �����ϵ��� �ϴ� �Լ�
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

            // ��Ȯ�� ���� Ž��: ������ ������� ������ 30�� �̳��� ���� ����
            float angle = Vector2.Angle(direction, directionToButton);
            if (angle <= 45f) // 30�� ������ ����
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



    // #. Panel�� Ȱ��ȭ �� �� ���� ��ư �׸���� ��������
    public void PanelChage(int index)
    {
        nowPlayerButton = null;

        switch (index)
        {
            case 0: // Main Panel �ѱ�
                Panel_Main.SetActive(true);
                PanelNow = Panel_Main;
                break;
            case 1: // Option Panel �ѱ�
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

        // Panel�� ��� ���� GameObject���� ������
        Transform[] childTransforms = PanelNow.GetComponentsInChildren<Transform>(true);

        // MenuButton ��ũ��Ʈ�� ��ӹ��� ������Ʈ���� ã�Ƽ� menuButtons �迭�� �Ҵ�
        List<MenuButton> foundButtons = new List<MenuButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // ���� GameObject���� MenuButton ��ũ��Ʈ�� ��ӹ��� ������Ʈ�� ã��
            MenuButton menuButton = childTransform.GetComponent<MenuButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List�� �迭�� ��ȯ�Ͽ� menuButtons�� �Ҵ�
        menuButtons = foundButtons.ToArray();
        if (menuButtons.Length != 0) lastButton = menuButtons[0];
        lastButton = menuButtons[0];
        bIsUIDoing = false;
    }

    public void PanelOff(int index)
    {
        // # ���� 1��
        PanelNow.SetActive(false);
        PanelChage(index);

        // #.���� 2��
        //PanelNow.transform.DOScale(maxScale, duration / 2)
        //.SetEase(Ease.OutBack) // EaseOutBack ȿ���� Ŀ��
        //.OnComplete(() => // Ŀ�� �Ŀ� ũ�⸦ 0���� ����
        //{
        //    PanelNow.transform.DOScale(Vector3.zero, duration / 2)
        //        .SetEase(Ease.InBack)
        //        .OnComplete(() => // ũ�Ⱑ 0�� �� �Ŀ� SetActive(false) ����
        //        {
        //            PanelNow.SetActive(false);
        //            PanelChage(index);
        //        });
        //});
    }
    public void PanelOn(GameObject ActivePanel)
    {
        // ���� 1��
        //ActivePanel.transform.localScale = Vector3.one * 0.5f;
        //ActivePanel.transform.DOScale(Vector3.one, duration / 2)
        //.SetEase(Ease.OutBack); // EaseOutBack ȿ���� �ڿ������� Ŀ��


        // ���� 2��
        Image[] images = ActivePanel.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] textMeshes = ActivePanel.GetComponentsInChildren<TextMeshProUGUI>();

        // �� Image�� ���İ��� 0���� 1���� ������ ��ȭ
        foreach (Image img in images)
        {
            // Image�� �ʱ� ���İ��� 0���� ����
            Color tempColor = img.color;
            tempColor.a = 0f;
            img.color = tempColor;

            // ���İ��� 0���� 1�� 1�� ���� ������ �ø�
            DOTween.To(() => img.color.a, x => {
                tempColor.a = x;
                img.color = tempColor;
            }, 1f, duration).SetEase(Ease.Linear).SetUpdate(true); // 1�� ���� ���İ��� 1�� ����
        }

        // �� TextMeshPro�� ������ 0���� 1���� ������ ��ȭ
        foreach (TextMeshProUGUI textMesh in textMeshes)
        {
            // TextMeshPro�� �ʱ� ���İ��� 0���� ����
            Color textColor = textMesh.color;
            textColor.a = 0f;
            textMesh.color = textColor;

            // �ؽ�Ʈ ���� ���İ��� 0���� 1�� 1�� ���� ������ �ø�
            DOTween.To(() => textMesh.color.a, x => {
                textColor.a = x;
                textMesh.color = textColor;
            }, 1f, duration).SetEase(Ease.Linear).SetUpdate(true);
        }



    }








    /// <summary>
    /// �ٸ� ������ ����, Ȥ�� ��Ÿ �۾���
    /// </summary>

    // #. Scene ��ȯ �Լ�, ���� ���� ��ư���� ���������, ��ư���� ��ư ���� ��ɸ� �ֱ� ���ؼ�
    public void StartNewGame()
    {
        // Vignette�� intensity�� ���� ������ 1�� ������ ��ȭ
        if (vignette != null)
        {
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 2.5f)
                .SetEase(Ease.OutQuad);

            // Vignette�� smoothness�� ������ 1�� ��ȭ, �������� ����
            DOTween.To(() => vignette.smoothness.value, x => vignette.smoothness.value = x, 1f, 2.5f)
                .SetEase(Ease.OutQuad);
        }

        image_FadeOut.SetActive(true);
        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // ���İ��� ������ 1��, �������� ���� �� �� ��ȯ
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 2.5f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => {
            // �ִϸ��̼��� ���� �� 'Chapter_1' ������ ��ȯ
            SceneManager.LoadScene("Chapter_1_City1");
        });
    }


}
