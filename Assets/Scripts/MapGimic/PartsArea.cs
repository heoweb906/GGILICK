using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOwnerType
{
    Nothing,
    TrafficLightClockWork,      // ½ÅÈ£µî 
    SoundPiece,                 // »ç¿îµå ºí·Ï
    ToyTruckClockWork,          // Àå³­°¨ Æ®·°¿¡ ÀåÂøÇÒ ÅÂ¿± 
    StampMachine,               // µµÀå Âï´Â ±â°è
    GameMachine                 // °ÔÀÓ ±â°è
}

public enum PartsAreaType
{
    Wall,
    Floor
}

public class PartsArea : MonoBehaviour
{
    public GameObject Parts;                         // ?Œì¸ 
    public Transform PartsTransform;                 // ?Œì¸ ê°€ ?¤ì–´ê°??„ì¹˜   
    public Transform PartsInteractTransform;         // ?Œì¸ ë¥??¼ìš¸ ???ˆëŠ” ?„ì¹˜
    public bool BCanInteract;                        // ?Œì¸ ë¥??£ì„ ???ˆëŠ” ?íƒœ?¸ì?
    public int iIndex;

    public PartOwnerType PartOwnertype;              // ?Œì¸  ?€??êµ¬ë¶„
    public PartsAreaType PartsAreaType;              // ?Œì¸  ?„ì¹˜ êµ¬ë¶„
    public GameObject[] partsOwnerObjects;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;
    }

    // #. ?Œì¸ ë¥??¥ì°©?ˆì„ ???¤í–‰?œí‚¤???¨ìˆ˜
    public virtual void InsertParts(GameObject partsObj)
    {
        OffCanInteract();
        Parts = partsObj;


        foreach (var owner in partsOwnerObjects)
        {
            partsOwner = owner.GetComponent<IPartsOwner>();
            if (partsOwner != null) partsOwner.InsertOwnerFunc(partsObj, iIndex);
        }
    }

    // #. ?Œì¸ ë¥??œê±°?ˆì„ ???¤í–‰?œí‚¤???¨ìˆ˜
    public virtual void RemoveParts()
    {
        OffCanInteract();
        Parts = null;


        foreach (var owner in partsOwnerObjects)
        {
            partsOwner = owner.GetComponent<IPartsOwner>();
            if (partsOwner != null) partsOwner.RemoveOwnerFunc(iIndex);
        }
    }


    private void OffCanInteract()
    {
        if (PartOwnertype == PartOwnerType.TrafficLightClockWork)
        {
            BCanInteract = false;
        }
    }







}
