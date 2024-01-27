using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BalanceDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform balanceBar;
    [SerializeField] private RectTransform curPin;

    private void Update()
    {
        float balanceValue = Motorcycle.Instance.BalanceValue;

        // 将BalanceValue从-100到100的范围映射到0到1的范围
        float normalizedBalanceValue = (balanceValue + 100) / 200;

        // 计算Pin在BalanceBar上的新位置
        float newPositionX = Mathf.Lerp(balanceBar.rect.xMin, balanceBar.rect.xMax, normalizedBalanceValue);

        // 更新Pin的位置
        curPin.anchoredPosition = new Vector2(newPositionX, curPin.anchoredPosition.y);
    }
}
