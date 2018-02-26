using UnityEngine;

public class SSTriggerZuDang : MonoBehaviour
{
    bool IsActiveTrigger = false;
    /// <summary>
    /// 场景中阻挡NPC数组.
    /// </summary>
    public XKCannonCtrl[] ZuDangArray;
    public AiPathCtrl TestPlayerPath;

    void OnDrawGizmosSelected()
    {
        if (!XkGameCtrl.IsDrawGizmosObj)
        {
            return;
        }

        if (!enabled)
        {
            return;
        }

        if (TestPlayerPath != null)
        {
            TestPlayerPath.DrawPath();
        }
    }

    void Update()
    {
        if (IsActiveTrigger)
        {
            if (CheckIsMovePlayer())
            {
                gameObject.SetActive(false);
                XkGameCtrl.GetInstance().SetIsActiveZuDangTrigger(false);
                XkGameCtrl.GetInstance().SetIsStopMovePlayer(false);
                //关闭提示框UI.
                GameUICenterCtrl.GetInstance().RemoveZuDangUI();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (JiFenJieMianCtrl.GetInstance().GetIsShowFinishTask())
        {
            return;
        }

        if (Network.peerType == NetworkPeerType.Server)
        {
            return;
        }

        XkPlayerCtrl script = other.GetComponent<XkPlayerCtrl>();
        if (script == null || !script.GetIsHandleRpc())
        {
            return;
        }

        if (!CheckIsMovePlayer())
        {
            IsActiveTrigger = true;
            XkGameCtrl.GetInstance().SetIsStopMovePlayer(true);
            XkGameCtrl.GetInstance().SetIsActiveZuDangTrigger(true);
            //打开提示框UI.
            GameUICenterCtrl.GetInstance().SpawnZuDangUI();
        }
    }

    bool CheckIsMovePlayer()
    {
        bool isMovePlayer = true;
        for (int i = 0; i < ZuDangArray.Length; i++)
        {
            if (ZuDangArray[i] != null && !ZuDangArray[i].IsDeathNpc)
            {
                isMovePlayer = false;
                break;
            }
        }
        return isMovePlayer;
    }
}