using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillState
{
    Ready,
    Active,
    Passive,
    Cooldown
}

public abstract class Skill 
{
    public string Name { get; protected set; }
    public float Duration { get; protected set; }
    public float Cooltime { get; protected set; }
    public int Level { get; protected set; } = 1;

    public float elapsedTime = 0f;      //경과 시간
    public SkillState state = SkillState.Ready;

    // 스킬 실행
    public virtual void UseSkill()
    {
        if (state != SkillState.Ready)
            return;
        state = SkillState.Active;
        elapsedTime = 0f;
    }

    // 스킬효과 제거
    public virtual void RemoveSkill()
    {
        state = SkillState.Cooldown;
        elapsedTime = 0f;
    }

    // 스킬 지속시간 업데이트
    public virtual void UpdateSkillDuration()
    {
        elapsedTime += Time.deltaTime;
        if (state == SkillState.Active && Duration > 0 && elapsedTime >= Duration)
        {
            RemoveSkill();
        }
        else if (state == SkillState.Cooldown && Cooltime > 0 && elapsedTime >= Cooltime)
        {
            state = SkillState.Ready;
            elapsedTime = 0f;
        }
    }

    // 스킬 레벨업
    public virtual void LevelUp()
    {
        Level++;
    }

    // 스킬의 쿨다운을 가져옵니다.
    public float GetCooldown()
    {
        return state == SkillState.Cooldown ? Cooltime - elapsedTime : 0f;
    }

}
