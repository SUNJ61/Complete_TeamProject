using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour, IItem
{
    MeshCollider DoorCol;
    AudioClip DoorOpen;
    void Start()
    {
        DoorOpen = Resources.Load<AudioClip>("Sound/Object/EndDoorOpen");

        DoorCol = GetComponent<MeshCollider>();
        DoorCol.enabled = false;
    }

    public void OpenCololider()
    {
        DoorCol.enabled = true;
    }
    public void CatchItem()
    {
        StartCoroutine(EndingimgStart());
        InGameSoundManager.instance.ActiveSound(gameObject, DoorOpen, 10, true, false, false, 1);

    }
    IEnumerator EndingimgStart()
    {
        DoorCol.enabled = false;
        InGameUIManager.instance.EndingUI();
        yield return null;
    }
    public void Use()
    {
        //�κ��丮���� ����ϴ� �����۾ƴ� ���� x
    }

    public void ItemUIOn()
    {
        InGameUIManager.instance.SetPlayerUI_Text("Ż���ϱ� [G]");
    }
}
