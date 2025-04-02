using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Player_OrbitBlades : Skill
{
    [Header("스킬 세팅")]
    public int bladeCount = 5;      // 생성될 프리팹의 갯수
    public float orbitRadius = 3f;  // 플레이어부터의 거리
    public float rotateSpeed = 180f;    //회전 속도
    public float selfSpineSpeed = 720f; // 무기 자체의 회전 속도
    public float damageInterval = 0.5f; //적에게 피해를 주는 주기
    public float baseDamage = 0.6f;
    public float manaCost = 7f;

    public float damageTimer = 0f;      
    private List<GameObject> blades = new List<GameObject>();

    public Skill_Player_OrbitBlades()
    {
        Name = "OrbitBlades";
        Duration = 5f;
        Cooltime = 2f;
    }

    

    // 비어있는 오브젝트를 가져와서 balde를 baldeCount 만큼 배치하고 플레이어 주변을 회전하게 합니다.
    public override void UseSkill()
    {
        if (state != SkillState.Ready) return;
        if (PlayerManager.Instance.Player.CurrentMp < manaCost) return;
        base.UseSkill();

        PlayerManager.Instance.Player.SubstractMana(manaCost);

        int bladeCount = GetBladeCountByLevel();

        for (int i = 0; i < bladeCount; i++)
        {
            float angle = (360f / bladeCount) * i;

            GameObject blade = OrbitBladePool.Instance.GetBlade();

            if (blade == null || blade.GetComponent<OrbitBlade>() == null)
            {
                Debug.LogError("OribitBlade 프리팹에 문재발생");
                continue;
            }

            blade.GetComponent<OrbitBlade>().Init(PlayerManager.Instance.PlayerTransform,
                orbitRadius, rotateSpeed, selfSpineSpeed, angle);

            blades.Add(blade);
        }


        // 플레이어 주변을 지속시간동안 돌아다니는 스킬
    }

    // 지속시간이 지나면 스킬들을 없애줍니다.
    public override void RemoveSkill()
    {
        base.RemoveSkill();
        foreach (GameObject blade in blades)
        {
            if (blade != null)
            {
                OrbitBladePool.Instance.ReturnBlade(blade);
            }
        }
        blades.Clear();
    }

    // 스킬이 사용중이라면 지속시간을 계산
    public override void UpdateSkillDuration()
    {
        base.UpdateSkillDuration();

        if (state == SkillState.Active)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                DealDamage();
            }
        }

    }

    private void DealDamage()
    {
        float damage = PlayerManager.Instance.Player.Damage * baseDamage;
        float range = 1f;

        foreach (var blade in blades)
        {
            OrbitBlade orbitBlade = blade.GetComponent<OrbitBlade>();
            if (orbitBlade != null)
            {
                orbitBlade.TriggerHit(damage);
            }
        }

    }

    private int GetBladeCountByLevel()
    {
        return bladeCount + Mathf.FloorToInt(Level / 5);
    }

    public override void LevelUp()
    {
        base.LevelUp();

    }

}
