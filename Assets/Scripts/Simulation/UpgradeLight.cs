using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLight : MonoBehaviour
{
    public void upgradeLight()
    {
        GameManager.Instance.getCurrentWorld().upgradeLightLevel();
    }
}
