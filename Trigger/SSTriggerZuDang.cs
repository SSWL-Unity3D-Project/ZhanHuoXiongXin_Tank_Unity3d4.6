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

    /// <summary>
    /// 导弹预制.
    /// </summary>
    public GameObject DaoDanPrefab;
    /// <summary>
    /// 导弹产生点.
    /// </summary>
    public Transform[] AmmoPointTr;
    /// <summary>
    /// 产生导弹的延迟时间.
    /// </summary>
    [Range(0f, 600f)]
    public float TimeSpawnDaoDan = 15f;
    /// <summary>
    /// 空袭阻挡删除的延迟时间.
    /// </summary>
    [Range(0f, 600f)]
    public float TimeRemoveZuDang = 3f;
    float TimeLastKongXi = 0f;
    bool IsCreatKongXiDaoDan = false;
    bool IsRemoveKongXiZuDang = false;
    XkPlayerCtrl mPlayerScript;
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
            if (ZuDangState == ZuDangType.KongXi)
            {
                if (!IsCreatKongXiDaoDan && Time.time - TimeLastKongXi >= TimeSpawnDaoDan)
                {
                    IsCreatKongXiDaoDan = true;
                    TimeLastKongXi = Time.time;
                    SpawnKongXiDaoDan(DaoDanPrefab);
                }

                if (IsCreatKongXiDaoDan && !IsRemoveKongXiZuDang && Time.time - TimeLastKongXi >= TimeRemoveZuDang)
                {
                    IsRemoveKongXiZuDang = true;
                    if (!CheckIsMovePlayer())
                    {
                        //删除空袭阻挡.
                        RemoveAllZuDang();
                    }
                }
            }

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

        mPlayerScript = other.GetComponent<XkPlayerCtrl>();
        if (mPlayerScript == null || !mPlayerScript.GetIsHandleRpc())
        {
            return;
        }

        if (!CheckIsMovePlayer())
        {
            IsActiveTrigger = true;
            TimeLastKongXi = Time.time;
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

    /// <summary>
    /// 检测是否可以使玩家继续运动.
    /// </summary>
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

    /// <summary>
    /// 删除阻挡.
    /// </summary>
    void RemoveAllZuDang()
    {
        for (int i = 0; i < ZuDangArray.Length; i++)
        {
            if (ZuDangArray[i] != null && !ZuDangArray[i].IsDeathNpc)
            {
                ZuDangArray[i].OnRemoveCannon(PlayerEnum.Null, 1);
            }
        }
    }

    /// <summary>
    /// 产生空袭导弹.
    /// </summary>
    void SpawnKongXiDaoDan(GameObject playerDaoDan)
    {
        if (playerDaoDan == null)
        {
            Debug.LogWarning("SpawnPlayerDaoDan -> playerDaoDan was null");
            return;
        }

        int max = AmmoPointTr.Length;
        for (int i = 0; i < max; i++)
        {
            if (mPlayerScript != null && AmmoPointTr[i] != null)
            {
                AmmoPointTr[i].gameObject.SetActive(false);
                mPlayerScript.SpawnPlayerDaoDan(AmmoPointTr[i], playerDaoDan);
            }
        }
    }
}