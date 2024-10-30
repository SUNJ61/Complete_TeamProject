using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBox : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenbtn;
    [SerializeField] List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum = 0;

    void Start()
    {
        fullscreenbtn = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<Toggle>();
        resolutionDropdown = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Dropdown>();
        Init();
    }

    void Init()
    {
        resolutions.Clear(); // ���� �ػ� ����Ʈ �ʱ�ȭ
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }// ���� ����Ϳ� �ִ� �ػ� �� �Ҿ�� for������ �迭�� �����ѵ� ����Ʈ�� �迭�� �Ҵ�

        resolutionDropdown.options.Clear();// ��ӹڽ� �ʱ�ȭ
        int optionNum = 0;// �迭�� ���� int ����
        foreach (Resolution item in resolutions)// ����Ʈ �ִ°� �ٽ� Resolution ������ ����
        {
            Dropdown.OptionData option = new Dropdown.OptionData(); //��ӹڽ� �ɼǵ����� ���� �ʱ�ȭ
            option.text = item.width + "X" + item.height; // ��ӹڽ� �� �ִ� �ؽ�Ʈ��  ���� �� ���� �� ǥ��
            resolutionDropdown.options.Add(option); // ��ӹڽ� �ɼǿ� ���� �Ҵ�
            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum; //
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
        fullscreenbtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.MaximizedWindow);
    }

    public void DropOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        if (isFull)
        {
            // ���� ������� �ػ󵵷� �����ϸ鼭 Ǯ��ũ������ ��ȯ
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
            screenMode = FullScreenMode.FullScreenWindow; // ���� ������Ʈ
        }
        else
        {
            // â ���� ��ȯ (���ϴ� �ػ󵵷� ����)
            Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, FullScreenMode.Windowed);
            screenMode = FullScreenMode.Windowed; // ���� ������Ʈ
        }
    }

    public void OkBtnClick()
    {
        // �ػ� ����
        Resolution selectedResolution = resolutions[resolutionNum];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenbtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        // ��� ���� ������Ʈ
        fullscreenbtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.MaximizedWindow);
    }
}
