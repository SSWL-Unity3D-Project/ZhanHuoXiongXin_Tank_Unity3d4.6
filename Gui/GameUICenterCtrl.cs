using UnityEngine;

/// <summary>
/// 游戏中心界面控制.
/// </summary>
public class GameUICenterCtrl : SSUiRoot
{
    Transform UIRootTr;
    /// <summary>
    /// 阻挡UI界面预制.
    /// </summary>
    public Object ZuDangUIPrefab;
    Object ZuDangUIObj;

    static GameUICenterCtrl _Instance;
    public static GameUICenterCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        _Instance = this;
        UIRootTr = transform.root;
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
}