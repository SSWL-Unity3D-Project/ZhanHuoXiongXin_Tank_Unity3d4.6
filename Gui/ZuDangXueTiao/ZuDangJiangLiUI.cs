using UnityEngine;

public class ZuDangJiangLiUI : MonoBehaviour
{
    /// <summary>
    /// 动画事件回调.
    /// </summary>
    public void OnAnimationTrigger()
    {
        GameUICenterCtrl.GetInstance().RemoveZuDangJiangLiUI();
    }
}