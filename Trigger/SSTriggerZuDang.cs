using System.Collections;
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
        /// 空袭点.
        /// </summary>
        public GameObject KongXiDianObj;
        /// <summary>
        /// 空袭隐藏cube.
        /// </summary>
        public GameObject[] HiddenObjArray;
        /// <summary>
        /// 导弹对玩家的伤害距离.
        /// </summary>
        [Range(0f, 100f)]
        public float DamageDis = 25f;
        /// <summary>
        /// 导弹对玩家的伤害.
        /// </summary>
        [Range(0f, 1000f)]
        public float PlayerDamage = 15f;
        /// <summary>
        /// 产生导弹的延迟时间.
        /// </summary>
        [Range(0f, 600f)]
        public float TimeSpawnDaoDan = 15f;
        /// <summary>
        /// 循环产生导弹的时间.
        /// </summary>
        [Range(0.1f, 600f)]
        public float TimeLoopDaoDan = 1f;
        /// <summary>
        /// 空袭阻挡删除的延迟时间.
        /// </summary>
        [Range(0f, 600f)]
        public float TimeRemoveZuDang = 3f;
        [HideInInspector]
        public float TimeLastKongXi = 0f;
        [HideInInspector]
        public float TimeLastKongXiDaoJiShi = 0f;
        /// <summary>
        /// 倒计时数据.
        /// </summary>
        [HideInInspector]
        public int DaoJiShiVal = 0;
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
        switch (ZuDangState)
        {
            case ZuDangType.Null:
                {
                    gameObject.SetActive(false);
                    break;
                }
            case ZuDangType.KongXi:
                {
                    if (KongXiDt.HiddenObjArray.Length > 0)
                    {
                        for (int i = 0; i < KongXiDt.HiddenObjArray.Length; i++)
                        {
                            if (KongXiDt.HiddenObjArray[i] != null)
                            {
                                KongXiDt.HiddenObjArray[i].SetActive(false);
                            }
                        }
                    }
                    KongXiDt.TimeSpawnDaoDan = Mathf.CeilToInt(KongXiDt.TimeSpawnDaoDan);
                    KongXiDt.DaoJiShiVal = (int)KongXiDt.TimeSpawnDaoDan;
                    break;
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

        if (ZuDangState == ZuDangType.KongXi)
        {
            if (KongXiDt.KongXiDianObj != null && KongXiDt.DamageDis > 0f)
            {
                Gizmos.DrawWireSphere(KongXiDt.KongXiDianObj.transform.position, KongXiDt.DamageDis);
            }
        }
    }

    void Update()
    {
        if (IsActiveTrigger)
        {
            if (ZuDangState == ZuDangType.KongXi)
            {
                if (!KongXiDt.IsCreatKongXiDaoDan)
                {
                    if (Time.time - KongXiDt.TimeLastKongXi >= KongXiDt.TimeSpawnDaoDan)
                    {
                        if (GameUICenterCtrl.GetInstance() != null)
                        {
                            GameUICenterCtrl.GetInstance().RemoveKongXiDaoJishiUI();
                        }
                        KongXiDt.IsCreatKongXiDaoDan = true;
                        KongXiDt.TimeLastKongXi = Time.time;
                        //产生空袭导弹.
                        SpawnKongXiDaoDan(KongXiDt.DaoDanPrefab);
                    }
                    else
                    {
                        if (Time.time - KongXiDt.TimeLastKongXiDaoJiShi >= 1f)
                        {
                            KongXiDt.TimeLastKongXiDaoJiShi = Time.time;
                            KongXiDt.DaoJiShiVal--;
                            if (GameUICenterCtrl.GetInstance() != null)
                            {
                                GameUICenterCtrl.GetInstance().mKongXiDaoJiShiUI.ShwoTimeVal(KongXiDt.DaoJiShiVal);
                            }
                        }
                    }
                }

                if (KongXiDt.IsCreatKongXiDaoDan && !KongXiDt.IsRemoveKongXiZuDang && Time.time - KongXiDt.TimeLastKongXi >= KongXiDt.TimeRemoveZuDang)
                {
                    KongXiDt.IsRemoveKongXiZuDang = true;
                    Debug.Log("removeKongXiZuDang -> time " + Time.time);
                    if (!CheckIsMovePlayer())
                    {
                        //删除空袭阻挡.
                        //RemoveAllZuDang();
                        //删除空袭点.
                        if (KongXiDt.KongXiDianObj != null)
                        {
                            Destroy(KongXiDt.KongXiDianObj);
                        }
                        //使玩家可以继续移动,当敌人空袭完成后.
                        KongXiDt.IsMovePlayer = true;
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
                            if (GameUICenterCtrl.GetInstance() != null)
                            {
                                GameUICenterCtrl.GetInstance().RemoveZuDangUI();
                                GameUICenterCtrl.GetInstance().RemoveZuDangXueTiaoUI();
                            }
                            break;
                        }
                    case ZuDangType.KongXi:
                        {
                            if (GameUICenterCtrl.GetInstance() != null)
                            {
                                GameUICenterCtrl.GetInstance().RemoveKongXiZuDangUI();
                            }
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
            KongXiDt.TimeLastKongXi = KongXiDt.TimeLastKongXiDaoJiShi = Time.time;
            //打开提示框UI.
            switch (ZuDangState)
            {
                case ZuDangType.PuTong:
                    {
                        XkGameCtrl.GetInstance().SetIsStopMovePlayer(true);
                        XkGameCtrl.GetInstance().SetIsActiveZuDangTrigger(true);
                        if (GameUICenterCtrl.GetInstance() != null)
                        {
                            GameUICenterCtrl.GetInstance().SpawnZuDangUI();
                            if (ZuDangArray[0] != null)
                            {
                                XKNpcHealthCtrl npcHealth = ZuDangArray[0].GetComponent<XKNpcHealthCtrl>();
                                if (npcHealth != null)
                                {
                                    GameUICenterCtrl.GetInstance().SpawnZuDangXueTiaoUI(npcHealth);
                                }
                            }
                        }
                        break;
                    }
                case ZuDangType.KongXi:
                    {
                        if (GameUICenterCtrl.GetInstance() != null)
                        {
                            GameUICenterCtrl.GetInstance().SpawnKongXiZuDangUI();
                            GameUICenterCtrl.GetInstance().SpawnKongXiDaoJiShiUI((int)KongXiDt.TimeSpawnDaoDan);
                        }
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
        StartCoroutine(LoopSpawnKongXiDaoDan(playerDaoDan));
    }

    /// <summary>
    /// 循环产生空袭导弹.
    /// </summary>
    IEnumerator LoopSpawnKongXiDaoDan(GameObject playerDaoDan)
    {
        bool isDamagePlayer = false;
        do
        {
            if (KongXiDt.IsRemoveKongXiZuDang)
            {
                Debug.Log("LoopSpawnKongXiDaoDan -> stop!");
                yield break;
            }

            Debug.Log("LoopSpawnKongXiDaoDan -> time " + Time.time);
            int max = KongXiDt.AmmoPointTr.Length;
            for (int i = 0; i < max; i++)
            {
                if (mPlayerScript != null && KongXiDt.AmmoPointTr[i] != null)
                {
                    KongXiDt.AmmoPointTr[i].gameObject.SetActive(false);
                    mPlayerScript.SpawnPlayerDaoDan(KongXiDt.AmmoPointTr[i], playerDaoDan);
                }
            }

            if (!isDamagePlayer)
            {
                Vector3 pos1 = KongXiDt.KongXiDianObj.transform.position;
                Vector3 pos2 = XkPlayerCtrl.GetInstanceTanKe().transform.position;
                pos1.y = pos2.y = 0f;
                if (Vector3.Distance(pos1, pos2) <= KongXiDt.DamageDis)
                {
                    isDamagePlayer = true;
                    XkGameCtrl.GetInstance().SubPlayerYouLiang(PlayerEnum.PlayerOne, KongXiDt.PlayerDamage);
                    XkGameCtrl.GetInstance().SubPlayerYouLiang(PlayerEnum.PlayerTwo, KongXiDt.PlayerDamage);
                }
            }
            yield return new WaitForSeconds(KongXiDt.TimeLoopDaoDan);
        }
        while (!KongXiDt.IsRemoveKongXiZuDang);
    }
}