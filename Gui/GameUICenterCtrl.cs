using UnityEngine;

/// <summary>
/// 游戏中心界面控制.
/// </summary>
public class GameUICenterCtrl : SSUiRoot
{
    /// <summary>
    /// 阻挡UI界面预制.
    /// </summary>
    public Object ZuDangUIPrefab;
    Object ZuDangUIObj;

    /// <summary>
    /// 空袭阻挡UI界面预制.
    /// </summary>
    public Object KongXiZuDangUIPrefab;
    Object KongXiZuDangUIObj;

    /// <summary>
    /// 空袭倒计时UI界面预制.
    /// </summary>
    public Object KongXiDaoJiShiUIPrefab;
    /// <summary>
    /// 空袭倒计时UI.
    /// </summary>
    [HideInInspector]
    public KongXiDaoJiShiUI mKongXiDaoJiShiUI;

    /// <summary>
    /// 阻挡血条UI界面预制.
    /// </summary>
    public Object ZuDangXueTiaoUIPrefab;
    /// <summary>
    /// 阻挡血条UI.
    /// </summary>
    [HideInInspector]
    public ZuDangXueTiaoUI mZuDangXueTiaoUI;

    static GameUICenterCtrl _Instance;
    public static GameUICenterCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        _Instance = this;
    }
    
    /// <summary>
    /// 产生阻挡UI界面.
    /// </summary>
    public void SpawnZuDangUI()
    {
        if (ZuDangUIObj == null)
        {
            ZuDangUIObj = Instantiate((GameObject)ZuDangUIPrefab, transform);
        }
    }

    /// <summary>
    /// 删除阻挡UI界面.
    /// </summary>
    public void RemoveZuDangUI()
    {
        if (ZuDangUIObj != null)
        {
            Destroy(ZuDangUIObj);
        }
    }
    
    /// <summary>
    /// 产生空袭阻挡UI界面.
    /// </summary>
    public void SpawnKongXiZuDangUI()
    {
        if (KongXiZuDangUIObj == null)
        {
            KongXiZuDangUIObj = Instantiate((GameObject)KongXiZuDangUIPrefab, transform);
        }
    }

    /// <summary>
    /// 删除空袭阻挡UI界面.
    /// </summary>
    public void RemoveKongXiZuDangUI()
    {
        if (KongXiZuDangUIObj != null)
        {
            Destroy(KongXiZuDangUIObj);
        }
    }
    
    /// <summary>
    /// 产生空袭倒计时UI界面.
    /// </summary>
    public void SpawnKongXiDaoJiShiUI(int daoJiShiVal)
    {
        if (mKongXiDaoJiShiUI == null)
        {
            GameObject obj = (GameObject)Instantiate((GameObject)KongXiDaoJiShiUIPrefab, transform);
            mKongXiDaoJiShiUI = obj.GetComponent<KongXiDaoJiShiUI>();
            mKongXiDaoJiShiUI.ShwoTimeVal(daoJiShiVal);
        }
    }

    /// <summary>
    /// 删除空袭倒计时UI界面.
    /// </summary>
    public void RemoveKongXiDaoJishiUI()
    {
        if (mKongXiDaoJiShiUI != null)
        {
            Destroy(mKongXiDaoJiShiUI.gameObject);
        }
    }

    /// <summary>
    /// 产生阻挡血条UI界面.
    /// </summary>
    public void SpawnZuDangXueTiaoUI(float xueLiang)
    {
        if (mZuDangXueTiaoUI == null)
        {
            GameObject obj = (GameObject)Instantiate((GameObject)ZuDangXueTiaoUIPrefab, transform);
            mZuDangXueTiaoUI = obj.GetComponent<ZuDangXueTiaoUI>();
            mZuDangXueTiaoUI.Init(xueLiang);
        }
    }

    /// <summary>
    /// 删除阻挡血条UI界面.
    /// </summary>
    public void RemoveZuDangXueTiaoUI()
    {
        if (mZuDangXueTiaoUI != null)
        {
            Destroy(mZuDangXueTiaoUI.gameObject);
        }
    }
}