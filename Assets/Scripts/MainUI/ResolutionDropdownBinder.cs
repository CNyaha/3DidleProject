using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// TMP_Dropdown UI에 해상도 옵션을 바인딩하고,
/// 사용자가 선택한 해상도를 ResolutionManager에 위임해서 적용하는 클래스.
/// </summary>
public class ResolutionDropdownBinder : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown; // 드롭다운 UI 참조
    private ResolutionManager resolutionManager;

    private void Start()
    {
        resolutionManager = new ResolutionManager(); // 로직 전담 객체 생성

        // 해상도 목록 받아오기
        var resolutions = resolutionManager.GetAvailableResolutions();

        // "1920 x 1080" 형식으로 변환
        var options = resolutions.Select(r => $"{r.width} x {r.height}").ToList();

        // 드롭다운 초기화 및 옵션 추가
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        // 현재 해상도에 해당하는 인덱스를 기본값으로 설정
        dropdown.value = resolutionManager.GetCurrentResolutionIndex();
        dropdown.RefreshShownValue();

        // 드롭다운 선택 시 이벤트 등록
        dropdown.onValueChanged.AddListener(SetResolution);
    }

    /// <summary>
    /// 드롭다운에서 선택한 값이 변경되면 호출되는 함수.
    /// 선택된 인덱스를 ResolutionManager에 전달함.
    /// </summary>
    private void SetResolution(int index)
    {
        resolutionManager.ApplyResolution(index);
    }
}
