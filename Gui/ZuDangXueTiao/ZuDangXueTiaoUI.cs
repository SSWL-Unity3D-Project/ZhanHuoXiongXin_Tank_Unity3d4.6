using UnityEngine;

public class ZuDangXueTiaoUI : MonoBehaviour
{
    /// <summary>
    /// 阻挡血条.
    /// </summary>
    public UISprite mXuTiaoSprite;
    public XKNpcHealthCtrl mNpcHealth;
    public void Init(XKNpcHealthCtrl npcHealth)
    {
        if (npcHealth != null)
        {
            float xueLiang = npcHealth.GetHealthValue();
            mNpcHealth = npcHealth;
            ShowZuDangXuTiaoVal(xueLiang);
        }
    }

    void Update()
    {
        if (mNpcHealth != null && Time.frameCount % 2 == 0)
        {
            float xueLiang = mNpcHealth.GetHealthValue();
            if (xueLiang != mXuTiaoSprite.fillAmount)
            {
                ShowZuDangXuTiaoVal(xueLiang);
            }
        }
    }

    public void ShowZuDangXuTiaoVal(float xueLiang)
    {
        //Debug.Log("ShowZuDangXuTiaoVal -> xueLiang == " + xueLiang);
        mXuTiaoSprite.fillAmount = xueLiang;
    }
}