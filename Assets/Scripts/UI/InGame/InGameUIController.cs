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
    public static InGameUIController Instance { get; private set; }

    public InGameButton nowPlayerButton; // ���� ���õǾ� �ִ� ��ư
    public InGameButton lastButton;
    public InGameButton[] ingameButtons;

    [SerializeField] private int nowPanelNum;

    private GameObject PanelNow;
    private bool bUIOnOff;
    // Panel Number = 643  / Panel Off ����
    [Header("InGameUI Panel")]
    public GameObject Panel_InGameUI; // Panel Number = 0;

    [Header("Option Panel")]
    public GameObject Panel_Option; // Panel Number = 1;
    public GameObject Panel_Resolution; // Panel Number = 8;


    [Header("UI �ִϸ��̼� �ε巯�� ��ġ")]
    public Volume volume_global; // ���� Volume ������Ʈ
    public GameObject image_FadeOut; // UI Ȱ��ȭ�ÿ� ����� �̹���
    public GameObject image_FadeOut_ForReturn; // �� ��ȯ�� ����� ������ �̹���
    public bool bIsUIDoing; // UI�� ���� ��� ����
    public float duration; // �ִϸ��̼� ���� �ð�


    private void Start()
    {
        FadeOutImageEffect();
        Instance = this;
    }

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


        // #. ESCŰ�� ���� ����
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
            else if (nowPanelNum == 8)
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
        InGameButton closestButton = null;
        Vector2 currentPosition = nowPlayerButton.transform.position;

        foreach (InGameButton button in ingameButtons)
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
                Panel_InGameUI.SetActive(true);
                PanelNow = Panel_InGameUI;
                break;
            case 1: // Option Panel �ѱ�
                Panel_Option.SetActive(true);
                PanelNow = Panel_Option;
                break;
            case 8: // Option Panel �ѱ�
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
        List<InGameButton> foundButtons = new List<InGameButton>();
        foreach (Transform childTransform in childTransforms)
        {
            // ���� GameObject���� MenuButton ��ũ��Ʈ�� ��ӹ��� ������Ʈ�� ã��
            InGameButton menuButton = childTransform.GetComponent<InGameButton>();
            if (menuButton != null)
            {
                foundButtons.Add(menuButton);
            }
        }

        // List�� �迭�� ��ȯ�Ͽ� menuButtons�� �Ҵ�
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

    // #. UI Off
    public void OffInGameUI()
    {
        Time.timeScale = 1f;
        if (nowPlayerButton != null) nowPlayerButton.SelectButtonOff();

   

        FadeInOutImage(0f, 0.2f);
        PanelNow.SetActive(false);
        nowPanelNum = 643;
        bUIOnOff = false;
    }


    // #. UI On
    public void OnInGameUI()
    {
        if (bUIOnOff) return;

        DOTween.KillAll();

        PanelChage(0);
        FadeInOutImage(0.9f, 0.2f);
        bUIOnOff = true;
        Time.timeScale = 0f;
    }


    // #. Scene ���� ���� �� ȭ��
    private void FadeOutImageEffect()
    {
        bUIOnOff = true;

        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // ���İ��� fTargetAlpha���� duration ���� �ø��� �ִϸ��̼�
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 0f)
        .SetEase(Ease.Linear)
        .SetUpdate(true).OnComplete(() => {

            DOTween.To(() => fadeColor.a, x => {
                fadeColor.a = x;
                fadeoutImage.color = fadeColor;
            }, 0f, 2.5f)
                .SetEase(Ease.Linear)
                  .SetUpdate(true);


            bUIOnOff = false;
            
        });
    }



    // #. UI Panel Ȱ�� ���� Ȯ��
    public bool GetbUIOnOff()
    {
        return bUIOnOff;
    }



    /// <summary>
    /// �ٸ� ������ ����, Ȥ�� ��Ÿ �۾���
    /// </summary>

    // FadeInOutImage ���İ� ���� �Լ�
    public void FadeInOutImage(float fTargetAlpha, float fFadeDuration) // �Ű������� ��ǥ ��ġ, �ɸ��� �ð�
    {
        Image fadeoutImage = image_FadeOut.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // ���İ��� fTargetAlpha���� duration ���� �ø��� �ִϸ��̼�
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, fTargetAlpha, fFadeDuration)
        .SetEase(Ease.Linear)
        .SetUpdate(true); // Time.timeScale�� ������ ���� �ʵ��� SetUpdate(true) ����
    }




    // #. Scene ��ȯ �Լ�, main ȭ������ ���ư���
    public void ChangeScene(string SceneName)
    {
        bUIOnOff = true;

        image_FadeOut_ForReturn.SetActive(true);
        Image fadeoutImage = image_FadeOut_ForReturn.GetComponent<Image>();
        Color fadeColor = fadeoutImage.color;

        // ���İ��� ������ 1��, �������� ���� �� �� ��ȯ
        DOTween.To(() => fadeColor.a, x => {
            fadeColor.a = x;
            fadeoutImage.color = fadeColor;
        }, 1f, 2.5f)
        .SetEase(Ease.OutQuad).SetUpdate(true)
        .OnComplete(() => {
            // �� ��ȯ ������ Time.timeScale�� 1�� ����
            Time.timeScale = 1f;

            bUIOnOff = false;
            SceneManager.LoadScene(SceneName);
        });
    }
}
