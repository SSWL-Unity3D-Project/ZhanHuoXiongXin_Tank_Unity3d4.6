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
    /// 空袭阻挡数据.
    /// </summary>
    [System.Serializable]
    public class KongXiZuDangDate
    {
        /// <summary>
        /// 导弹预制.
        /// </summary>
        public GameObject DaoDanPrefab;
        /// <summary>
        /// 导弹产生点.
        /// </summary>
        public Transform[] AmmoPointTr;
        /// <summary>
        /// 空袭提示的场景3d对象.
        /// </summary>
        public GameObject KongXiTiShiObj;
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
        [HideInInspector]
        public float TimeLastKongXi = 0f;
        [HideInInspector]
        public bool IsCreatKongXiDaoDan = false;
        [HideInInspector]
        public bool IsRemoveKongXiZuDang = false;
        [HideInInspector]
        public bool IsMovePlayer = false;
    }
    public KongXiZuDangDate KongXiDt = new KongXiZuDangDate();
    XkPlayerCtrl mPlayerScript;
    public AiPathCtrl TestPlayerPath;

    void Start()
    {
        if (ZuDangState == ZuDangType.Null)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < KongXiDt.AmmoPointTr.Length; i++)
        {
            if (KongXiDt.AmmoPointTr[i] != null)
            {
                KongXiDt.AmmoPointTr[i].gameObject.SetActive(false);
            }
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
                if (!KongXiDt.IsCreatKongXiDaoDan && Time.time - KongXiDt.TimeLastKongXi >= KongXiDt.TimeSpawnDaoDan)
                {
                    KongXiDt.IsCreatKongXiDaoDan = true;
                    KongXiDt.TimeLastKongXi = Time.time;
                    SpawnKongXiDaoDan(KongXiDt.DaoDanPrefab);
                }

                if (KongXiDt.IsCreatKongXiDaoDan && !KongXiDt.IsRemoveKongXiZuDang && Time.time - KongXiDt.TimeLastKongXi >= KongXiDt.TimeRemoveZuDang)
                {
                    KongXiDt.IsRemoveKongXiZuDang = true;
                    if (!CheckIsMovePlayer())
                    {
                        //删除空袭阻挡.
                        //RemoveAllZuDang();
                        //使玩家可以继续移动,当敌人空袭完成后.
                        KongXiDt.IsMovePlayer = true;
                        if (KongXiDt.KongXiTiShiObj != null)
                        {
                            Destroy(KongXiDt.KongXiTiShiObj);
                        }
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
            KongXiDt.TimeLastKongXi = Time.time;
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
        if (ZuDangState == ZuDangType.KongXi)
        {
            return KongXiDt.IsMovePlayer;
        }

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

        int max = KongXiDt.AmmoPointTr.Length;
        for (int i = 0; i < max; i++)
        {
            if (mPlayerScript != null && KongXiDt.AmmoPointTr[i] != null)
            {
                KongXiDt.AmmoPointTr[i].gameObject.SetActive(false);
                mPlayerScript.SpawnPlayerDaoDan(KongXiDt.AmmoPointTr[i], playerDaoDan);
            }
        }
    }
}