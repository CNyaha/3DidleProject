using Unity.VisualScripting;

public interface IMonsterBehaivor<T>
{
    void EnterBehaivor(T monsterController);
    void UpdateBehavior(T monsterController);
    void ExitBehaivor(T monsterController);
        
}
