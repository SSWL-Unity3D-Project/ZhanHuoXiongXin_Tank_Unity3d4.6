using UnityEngine;

public class SSTriggerWuDi : MonoBehaviour
{
    public AiPathCtrl TestPlayerPath;
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

    void OnTriggerEnter(Collider other)
    {
        if (JiFenJieMianCtrl.GetInstance().GetIsShowFinishTask())
        {
            return;
        }

        if (Network.peerType == NetworkPeerType.Server)
        {
            return;
        }

        XkPlayerCtrl playerScript = other.GetComponent<XkPlayerCtrl>();
        if (playerScript == null || !playerScript.GetIsHandleRpc())
        {
            return;
        }
        gameObject.SetActive(false);
        XkGameCtrl.GetInstance().SetIsStopMovePlayer(false);
        XkGameCtrl.GetInstance().SetIsActiveWuDiState(true);
    }
}