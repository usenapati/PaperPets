using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWater : MonoBehaviour
{
    public void upgradeWater()
    {
        GameManager.Instance.getCurrentWorld().upgradeWaterLevel();
    }
}
