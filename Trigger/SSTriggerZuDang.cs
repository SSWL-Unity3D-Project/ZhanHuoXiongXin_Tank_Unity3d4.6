using UnityEngine;

public class SSTriggerZuDang : MonoBehaviour
{
    /// <summary>
    /// 阻挡类型.
    /// </summary>
    public enum ZuDangType
    {
        Null,     //无用的.
        PuTong,   //普通阻挡.
        KongXi,   //空袭阻挡.
    }
    public ZuDangType ZuDangState = ZuDangType.Null;

    bool IsActiveTrigger = false;
    /// <summary>
    /// 场景中阻挡NPC数组.
    /// </summary>
    public XKCannonCtrl[] ZuDangArray;
    public AiPathCtrl TestPlayerPath;

    void Start()
    {
        if (ZuDangState == ZuDangType.Null)
        {
            gameObject.SetActive(false);
        }
    }

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
                switch (ZuDangState)
                {
                    case ZuDangType.PuTong:
                        {
                            GameUICenterCtrl.GetInstance().RemoveZuDangUI();
                            break;
                        }
                    case ZuDangType.KongXi:
                        {
                            GameUICenterCtrl.GetInstance().RemoveKongXiZuDangUI();
                            break;
                        }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (ZuDangState == ZuDangType.Null)
        {
            gameObject.SetActive(false);
            return;
        }

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
            switch (ZuDangState)
            {
                case ZuDangType.PuTong:
                    {
                        GameUICenterCtrl.GetInstance().SpawnZuDangUI();
                        break;
                    }
                case ZuDangType.KongXi:
                    {
                        GameUICenterCtrl.GetInstance().SpawnKongXiZuDangUI();
                        break;
                    }
            }
        }
        else
        {
            gameObject.SetActive(false);
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