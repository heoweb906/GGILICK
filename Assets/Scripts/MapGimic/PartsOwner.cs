using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartOwner
{
    GameObject Parts{ get; set; }                           // ����
    Transform PartsTransform { get; set; }                  // ������ �� ��ġ   
    Transform PartsInteractTransform { get; set; }          // ������ ���� �� �ִ� ��ġ


    // #. ������ �������� �� �����Ű�� �Լ�
    void InsertParts(GameObject partsObj);

    // #. ������ �������� �� �����Ű�� �Լ�
    public void RemoveParts();
  

}
