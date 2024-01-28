using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform balanceBar;
    [SerializeField] private RectTransform curPin;

    private void Update()
    {
        float distanceValue = GameManager.Instance.remainDistance;

        // 将DistanceValue从-100到100的范围映射到0到1的范围
        float normalizedDistanceValue = (GameManager.Instance.TargetDistance - distanceValue) / GameManager.Instance.TargetDistance;

        // 计算Pin在DistanceBar上的新位置
        float newPositionY = Mathf.Lerp(balanceBar.rect.yMin * 0.75f, balanceBar.rect.yMax * 0.75f, normalizedDistanceValue);

        // 更新Pin的位置
        curPin.anchoredPosition = new Vector2(curPin.anchoredPosition.x, newPositionY);
    }
}
