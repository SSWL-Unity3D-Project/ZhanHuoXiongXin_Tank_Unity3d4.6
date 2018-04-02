using UnityEngine;

public class CameraColorChange : MonoBehaviour
{
    /// <summary>
    /// 颜色初始值.
    /// </summary>
    public float ColorSaturationStart = 1f;
    /// <summary>
    /// 颜色变化速度.
    /// </summary>
    public float ColorSpeed = 3f;
    /// <summary>
    /// 镜头颜色变黑白控制.
    /// </summary>
    public ColorCorrectionCurves mColorCom;
    bool IsInitColor = false;
    void Awake()
    {
        mColorCom.enabled = false;
    }

    public void Init()
    {
        if (IsInitColor)
        {
            return;
        }
        mColorCom.saturation = ColorSaturationStart;
        mColorCom.enabled = true;
        enabled = true;
        IsInitColor = true;
    }

    public void Close()
    {
        if (!IsInitColor)
        {
            return;
        }
        IsInitColor = false;
        mColorCom.enabled = false;
        enabled = false;
    }

    void Update()
    {
        if (mColorCom.saturation > 0f && IsInitColor)
        {
            mColorCom.saturation -= (Time.deltaTime * ColorSpeed);
            if (mColorCom.saturation <= 0f)
            {
                mColorCom.saturation = 0f;
            }
        }
    }
}