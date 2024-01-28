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

        // ��DistanceValue��-100��100�ķ�Χӳ�䵽0��1�ķ�Χ
        float normalizedDistanceValue = (GameManager.Instance.TargetDistance - distanceValue) / GameManager.Instance.TargetDistance;

        // ����Pin��DistanceBar�ϵ���λ��
        float newPositionY = Mathf.Lerp(balanceBar.rect.yMin * 0.75f, balanceBar.rect.yMax * 0.75f, normalizedDistanceValue);

        // ����Pin��λ��
        curPin.anchoredPosition = new Vector2(curPin.anchoredPosition.x, newPositionY);
    }
}
