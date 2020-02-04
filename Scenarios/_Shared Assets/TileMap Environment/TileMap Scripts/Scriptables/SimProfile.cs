using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimProfile : ScriptableObject
{

    public float weektime = 1f;

    public float avgFishPrice = 16f;

    public float minBoatCapacity = .3f;
    public float maxBoatCapacity = 1f;

    public float minBoatsHarbor = 10;
    public float maxBoatsHarbor = 50;

    public float minDebt;
    public float maxDebt;

    public float debtRepaymentRate = .1f;

    public float efficiencyUpgradeCost;
    public float efficiencyUpgradeGain;

    public float capacityUpgradeCost;
    public float capacityUpgradeGain;

    public float startingCapital;

    public float growthRate = .2f;

    public float getMaxCarry()
    {
        return 12 * 15 * maxBoatCapacity * 3;
    }

    public float getMinCarry()
    {
        return 12 * 15 * minBoatCapacity * 3;
    }
}
