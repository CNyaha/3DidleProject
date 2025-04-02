using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaivorMachine<T>
{
    private T monsterController;
    
    public IMonsterBehaivor<T> currentBehavior;

    public MonsterBehaivorMachine(T _monsterController, IMonsterBehaivor<T> currentBehavior)
    {
        monsterController = _monsterController;
        SetState(currentBehavior);
    }

    /// <summary>
    /// 상태 설정
    /// </summary>
    /// <param name="behavior"></param>
    public void SetState(IMonsterBehaivor<T> behavior)
    {
        if (monsterController == null)
        {
            return;
        }
        if (currentBehavior == behavior)
        {
            return;
        }
        if (currentBehavior != null)
        {
            currentBehavior.EnterBehaivor(monsterController);
        }

        currentBehavior = behavior;

        if (currentBehavior != null)
            currentBehavior.EnterBehaivor(monsterController);
    }
    
    /// <summary>
    /// 상태 업데이트 
    /// </summary>
    public void UpdateState()
    {
        if (monsterController == null)
            return;
        currentBehavior.UpdateBehavior(monsterController);
    }
}
