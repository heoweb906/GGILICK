using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_PlayerCheck : MonoBehaviour
{
    public bool bPlayerNearby = false;

<<<<<<< Updated upstream
    // íŠ¸ë¦¬ê±°ì— ë‹¤ë¥¸ Colliderê°€ ë“¤ì–´ì˜¬ ë•Œ í˜¸ì¶œë¨
=======
    // Æ®¸®°Å¿¡ ´Ù¸¥ Collider°¡ µé¾î¿Ã ¶§ È£ÃâµÊ
>>>>>>> Stashed changes
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = true;
            Debug.Log("Player entered the area.");
        }
    }

<<<<<<< Updated upstream
    // íŠ¸ë¦¬ê±°ì—ì„œ ë‹¤ë¥¸ Colliderê°€ ë‚˜ê°ˆ ë•Œ í˜¸ì¶œë¨
=======
    // Æ®¸®°Å¿¡¼­ ´Ù¸¥ Collider°¡ ³ª°¥ ¶§ È£ÃâµÊ
>>>>>>> Stashed changes
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = false;
            Debug.Log("Player left the area.");
        }
    }
}
