using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] Image curTimeImg;

    private void Update()
    {
        curTimeImg.fillAmount = GameManager.Instance.remainTime / GameManager.Instance.GameTime;
    }
}
