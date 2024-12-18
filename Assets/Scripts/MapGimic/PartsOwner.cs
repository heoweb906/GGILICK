using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartOwner
{
    GameObject Parts{ get; set; }                           // 파츠
    Transform PartsTransform { get; set; }                  // 파츠가 들어갈 위치   
    Transform PartsInteractTransform { get; set; }          // 파츠를 끼울 수 있는 위치


    // #. 파츠를 장착했을 때 실행시키는 함수
    void InsertParts(GameObject partsObj);

    // #. 파츠를 제거했을 때 실행시키는 함수
    public void RemoveParts();
  

}
