using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static Ability GetAbilityByName(AbilityName abilityName)
    {
        switch (abilityName)
        {
            case AbilityName.Claws:
                return new ClawsAbility();
            case AbilityName.Fish:
                return new FishAbility();
            case AbilityName.WaterBottle:
                return new WaterbottleAbility();
            case AbilityName.Bone:
                return new BoneAbility();
            case AbilityName.Collar:
                return new CollarAbility();
            default:
                return new ClawsAbility();
        }
    }

    public void Setup()
    {
    }
}

public class CollarAbility : Ability
{
    public CollarAbility() : base()
    {
        Name = "Collar";
        Description = "Body block doggos";
    }
    public override void OnActivate()
    {
        base.OnActivate();
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableCollar(true);
    }

    public override void OnDeactivate()
    {
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableCollar(false);
    }
}

public class BoneAbility : Ability
{
    public BoneAbility() : base()
    {
        Name = "Bone";
        Description = "Attacks stun";
    }
    public override void OnActivate()
    {
        base.OnActivate();
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableBone(true);
    }

    public override void OnDeactivate()
    {
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableBone(false);
    }
}

public class WaterbottleAbility : Ability
{
    public WaterbottleAbility() : base()
    {
        Name = "Bottle";
        Description = "Attacks slow";
    }
    public override void OnActivate()
    {
        base.OnActivate();
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableWaterbottle(true);
    }

    public override void OnDeactivate()
    {
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableWaterbottle(false);
    }
}

public class ClawsAbility : Ability
{
    public ClawsAbility() : base()
    {
        Name = "Claws";
        Description = "Attacks hit 3 times";
    }
    public override void OnActivate()
    {
        base.OnActivate();
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableClaws(true);
    }

    public override void OnDeactivate()
    {
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableClaws(false);
    }
}

public class FishAbility : Ability
{
    public FishAbility() : base()
    {
        Name = "Fishy";
        Description = "Attack twice as fast";
    }
    public override void OnActivate()
    {
        base.OnActivate();
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableFish(true);
    }

    public override void OnDeactivate()
    {
        LevelController.Instance.CurrentPlayer.GetComponent<PlayerAttack>().EnableFish(false);
    }
}

public class Ability
{
    public bool Used { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Ability()
    {
        Used = false;
    }

    public virtual void OnActivate()
    {
        Used = true;
    }

    public virtual void OnDeactivate()
    {

    }

    public void Reset()
    {
        Used = false;
    }
}

[Serializable]
public enum AbilityName
{
    Claws,
    Fish,
    WaterBottle,
    Bone,
    Collar
}
