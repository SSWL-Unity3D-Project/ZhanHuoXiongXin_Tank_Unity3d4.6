using UnityEngine;

public class KongXiDaoJiShiUI : MonoBehaviour
{
    /// <summary>
    /// TimeArray[x] -> 0 秒十位, 1 秒个位.
    /// </summary>
    public UISprite[] TimeArray;
    public void ShwoTimeVal(int timeVal)
    {
        int miaoShiWei = Mathf.Clamp(timeVal / 10, 0, 9);
        int miaoGeWei = timeVal % 10;
        if (miaoShiWei > 0)
        {
            TimeArray[0].spriteName = miaoShiWei.ToString();
        }
        else
        {
            TimeArray[0].enabled = false;
        }

        TimeArray[1].spriteName = miaoGeWei.ToString();
    }
}