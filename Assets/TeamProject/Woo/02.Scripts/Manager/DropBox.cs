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
        resolutions.Clear(); // 기존 해상도 리스트 초기화
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }// 현재 모니터에 있는 해상도 를 불어와 for문으로 배열로 선언한뒤 리스트에 배열을 할당

        resolutionDropdown.options.Clear();// 드롭박스 초기화
        int optionNum = 0;// 배열에 담을 int 변수
        foreach (Resolution item in resolutions)// 리스트 있는걸 다시 Resolution 변수에 담음
        {
            Dropdown.OptionData option = new Dropdown.OptionData(); //드롭박스 옵션데이터 변수 초기화
            option.text = item.width + "X" + item.height; // 드롭박스 에 있는 텍스트에  넓이 값 높이 값 표시
            resolutionDropdown.options.Add(option); // 드롭박스 옵션에 변수 할당
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
            // 현재 모니터의 해상도로 설정하면서 풀스크린으로 전환
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
            screenMode = FullScreenMode.FullScreenWindow; // 상태 업데이트
        }
        else
        {
            // 창 모드로 전환 (원하는 해상도로 설정)
            Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, FullScreenMode.Windowed);
            screenMode = FullScreenMode.Windowed; // 상태 업데이트
        }
    }

    public void OkBtnClick()
    {
        // 해상도 변경
        Resolution selectedResolution = resolutions[resolutionNum];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenbtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        // 토글 상태 업데이트
        fullscreenbtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.MaximizedWindow);
    }
}
