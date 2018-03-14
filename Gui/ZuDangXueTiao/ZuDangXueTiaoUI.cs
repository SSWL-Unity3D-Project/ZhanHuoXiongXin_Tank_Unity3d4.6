using UnityEngine;

public class ZuDangXueTiaoUI : MonoBehaviour
{
    /// <summary>
    /// 阻挡血条.
    /// </summary>
    public UISprite mXuTiaoSprite;
    public void Init(float xueLiang)
    {
        mXuTiaoSprite.fillAmount = xueLiang;
    }

    public void ShowZuDangXuTiaoVal(float xueLiang)
    {
        mXuTiaoSprite.fillAmount = xueLiang;
    }
}