using UnityEngine;

/// <summary>
/// 玩家被空袭击中UI提示.
/// </summary>
public class PlayerBeiKongXiJiZhongUI : MonoBehaviour
{
    /// <summary>
    /// 被空袭击中UI提示动画结束事件响应.
    /// </summary>
    public void OnAnimationEnvent()
    {
        if (GameUICenterCtrl.GetInstance() != null)
        {
            GameUICenterCtrl.GetInstance().RemoveKongXiJiZhongUI();
        }
        
        if (!JiFenJieMianCtrl.GetInstance().GetIsShowFinishTask())
        {
            DaoJiShiCtrl.GetInstance().StartPlayDaoJiShi();
        }
        XkGameCtrl.SetActivePlayerOne(false);
        XkGameCtrl.SetActivePlayerTwo(false);
    }
}