
//abstract class for enemy state machine
public abstract class EnemyBaseState
{
    public abstract void EnterState(Enemy enemy);
    public abstract void OnUpdate(Enemy enemy);
}
