using UnityEngine;

/// <summary>
/// 해상도 관련 데이터를 관리하고, 설정을 적용하는 클래스.
/// UI와 무관하게 로직만 담당함.
/// </summary>
public class ResolutionManager
{
    // 사용 가능한 모든 해상도 목록을 Unity에서 가져옴
    private Resolution[] resolutions = Screen.resolutions;

    /// <summary>
    /// 사용 가능한 해상도 배열을 반환함.
    /// </summary>
    public Resolution[] GetAvailableResolutions() => resolutions;

    /// <summary>
    /// 현재 설정된 해상도가 해상도 리스트의 몇 번째인지 찾음.
    /// 드롭다운에서 기본 선택값으로 사용함.
    /// </summary>
    public int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0; // 찾지 못했을 경우 기본값
    }

    /// <summary>
    /// 선택된 인덱스의 해상도를 실제 화면에 적용함.
    /// </summary>
    public void ApplyResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
