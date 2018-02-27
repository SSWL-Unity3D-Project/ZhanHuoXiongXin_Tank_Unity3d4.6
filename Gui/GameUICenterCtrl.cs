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
        ZuDangUIObj = Instantiate((GameObject)ZuDangUIPrefab, transform);
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
        KongXiZuDangUIObj = Instantiate((GameObject)KongXiZuDangUIPrefab, transform);
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
}