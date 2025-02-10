using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
=======
using UnityEngine;
>>>>>>> Stashed changes

public class StampMacine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

<<<<<<< Updated upstream
    [Header("ì„œë¥˜ ì˜¤ë¸Œì íŠ¸")]
    public GameObject obj_Document;         // ì»¤ë‹¤ë‘ ìƒíƒœì˜ ì„œë¥˜
    public GameObject obj_SmallDocument;    // ìƒì„±í•  ColorObj
    public Transform transform_CreateDocument;

    [Header("ìŠ¤íƒ¬í”„ ì •ë³´")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // ì°ì„ ìŠ¤íƒ¬í”„ë“¤
    public Transform transforom_stamp;  // ìŠ¤íƒ¬í”„ ì°ì„ ìœ„ì¹˜
    private Queue<int> queueStamp = new Queue<int>(); // ìƒì„±ëœ ìŠ¤íƒ¬í”„ ê´€ë¦¬ìš© ìŠ¤íƒ
    
=======
    [Header("½ºÅÆÇÁ Á¤º¸")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // ÂïÀ» ½ºÅÆÇÁµé
    public Transform transforom_stamp;  // ½ºÅÆÇÁ ÂïÀ» À§Ä¡

    private Stack<GameObject> stampStack = new Stack<GameObject>(); // »ı¼ºµÈ ½ºÅÆÇÁ °ü¸®¿ë ½ºÅÃ
>>>>>>> Stashed changes


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(HitStamp());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
<<<<<<< Updated upstream

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        // ë„ì¥ì„ ì•Œë§ì€ ëª¨ì–‘ìœ¼ë¡œ ì°ì—ˆë‹¤ë©´
        if(IsQueueInOrder(queueStamp))
        {
            obj_Document.SetActive(false);
            GameObject instantiatedObject = Instantiate(obj_SmallDocument, transform_CreateDocument);
        }
    }

=======
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        Debug.Log(iStampNum);

    }
>>>>>>> Stashed changes


    private IEnumerator HitStamp()
    {
<<<<<<< Updated upstream
        // ë°°í„°ë¦¬ê°€ 3ë³´ë‹¤ ì‘ë‹¤ë©´
=======
        // ¹èÅÍ¸®°¡ 3º¸´Ù ÀÛ´Ù¸é
>>>>>>> Stashed changes
        if (fCurClockBattery < 3)
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
<<<<<<< Updated upstream
                yield return new WaitForSecondsRealtime(1.0f); // 1ì´ˆ ëŒ€ê¸°
            }
        }
        else // ë°°í„°ë¦¬ê°€ 3 ì´ìƒì´ë¼ë©´
=======
                yield return new WaitForSecondsRealtime(1.0f); // 1ÃÊ ´ë±â
            }
        }
        else // ¹èÅÍ¸®°¡ 3 ÀÌ»óÀÌ¶ó¸é
>>>>>>> Stashed changes
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
<<<<<<< Updated upstream
                yield return new WaitForSecondsRealtime(1.0f); // 1ì´ˆ ëŒ€ê¸°
=======
                yield return new WaitForSecondsRealtime(1.0f); // 1ÃÊ ´ë±â
>>>>>>> Stashed changes
            }

            if (iStampNum > 0)
            {
<<<<<<< Updated upstream
                queueStamp.Enqueue(iStampNum);
                CreateStackedStamps();      // ë„ì¥ ì°ê¸°
=======
                // YÃà ¿ÀÇÁ¼Â °è»ê: ½ºÅÃ¿¡ ÀÖ´Â ¿ä¼Ò ¼ö¿¡ µû¶ó ¼³Á¤
                float yOffset = stampStack.Count * 0.01f;

                // ½ºÅÆÇÁ ÇÁ¸®ÆÕ »ı¼º
                GameObject newStamp = Instantiate(
                    Stamps[iStampNum - 1],
                    transforom_stamp.position + new Vector3(0, yOffset, 0),
                    Quaternion.Euler(90f, 0f, 0f) // XÃàÀ¸·Î 90µµ È¸Àü
                );

                // ½ºÅÆÇÁ¸¦ ½ºÅÃ¿¡ Ãß°¡
                stampStack.Push(newStamp);

                // ºÎ¸ğ ¼³Á¤
                newStamp.transform.SetParent(transforom_stamp);
>>>>>>> Stashed changes
            }
        }

        TurnOffObj();
        yield break;
    }
<<<<<<< Updated upstream
    // #. ë„ì¥ ìƒì„±í•˜ê¸°
    public void CreateStackedStamps()
    {
        // ì´ë¯¸ ìƒì„±ë˜ì–´ ìˆëŠ” ë„ì¥ë“¤ì„ ì§€ì›€
        foreach (Transform child in transforom_stamp)
        {
            Destroy(child.gameObject);  
        }


        // ìì‹  ì•„ë˜ì˜ ë„ì¥ë“¤ì„ ì§€ì›€
        Queue<int> tempQueue = new Queue<int>();
        while (queueStamp.Count > 0)
        {
            int element = queueStamp.Dequeue();
            if (element <= iStampNum) tempQueue.Enqueue(element);
        }
        // ì›ë˜ íì— ì¡°ê±´ì„ ë§Œì¡±í•˜ëŠ” ìš”ì†Œë§Œ ë‹¤ì‹œ ë„£ê¸°
        while (tempQueue.Count > 0)
        {
            queueStamp.Enqueue(tempQueue.Dequeue());
        }



        if (queueStamp.Count > 0)
        {
            // ìŠ¤íƒì—ì„œ êº¼ë‚¸ ìš”ì†Œë¥¼ ì €ì¥í•  ë¦¬ìŠ¤íŠ¸
            List<int> poppedElements = new List<int>();

            int count = queueStamp.Count;
            for (int i = 0; i < count; i++)
            {
                // íì—ì„œ Popì„ ì‚¬ìš©í•˜ì—¬ ìš”ì†Œë¥¼ êº¼ëƒ„
                int stampIndex = queueStamp.Dequeue();  // Popì„ ì‚¬ìš©í•˜ì—¬ íì—ì„œ ê°’ì„ ì œê±°í•˜ë©´ì„œ êº¼ëƒ„

                poppedElements.Add(stampIndex);

                if (stampIndex >= 0)
                {
                    Vector3 spawnPosition = transforom_stamp.position + new Vector3(0, 0.01f * i, 0);
                    GameObject newStamp = Instantiate(Stamps[stampIndex - 1], spawnPosition, Quaternion.Euler(90, 0, 0));
                    newStamp.transform.SetParent(transforom_stamp);
                }
                else
                {
                    Debug.LogWarning("Invalid stamp index in the stack");
                }
            }

            // êº¼ë‚¸ ìš”ì†Œë“¤ì„ ë‹¤ì‹œ ì›ë˜ëŒ€ë¡œ ìŠ¤íƒì— ë³µì›
            foreach (int stampIndex in poppedElements)
            {
                queueStamp.Enqueue(stampIndex);
            }
        }
    }




    private bool IsQueueInOrder(Queue<int> queue)
    {
        // ì˜¬ë°”ë¥¸ ìˆœì„œë¥¼ ë¯¸ë¦¬ ì •ì˜í•©ë‹ˆë‹¤.
        int[] correctOrder = { 1, 2, 3, 4 };

        // ìš”ì†Œ ê°œìˆ˜ê°€ ë‹¤ë¥´ë©´ ìˆœì„œê°€ ë§ì„ ìˆ˜ ì—†ìŠµë‹ˆë‹¤.
        if (queue.Count != correctOrder.Length)
            return false;

        // Queueë¥¼ ë°°ì—´ë¡œ ë³€í™˜í•˜ì—¬ ìˆœì„œë¥¼ ë¹„êµí•©ë‹ˆë‹¤.
        int[] queueArray = queue.ToArray();

        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (queueArray[i] != correctOrder[i])
                return false;
        }

        return true;
    }
=======
>>>>>>> Stashed changes








<<<<<<< Updated upstream
    // #. IPartOwner ì¸í„°í˜ì´ìŠ¤
=======






    // #. IPartOwner ÀÎÅÍÆäÀÌ½º
>>>>>>> Stashed changes
    #region

    public void InsertOwnerFunc(GameObject stampParts, int index)
    {
        StampParts stampParts_ = stampParts.GetComponent<StampParts>();
<<<<<<< Updated upstream
        if (stampParts_ == null)
        {
            Debug.LogWarning("ìŠ¤íƒ¬í”„ íŒŒì¸ ì— StampParts ì»´í¬ë„ŒíŠ¸ê°€ ì—†ìŠµë‹ˆë‹¤.");
            return;
        }
=======
>>>>>>> Stashed changes
        iStampNum = stampParts_.iStampeNum;
    }

    public void RemoveOwnerFunc(int index)
    {
        iStampNum = 0;
    }



    #endregion
}
