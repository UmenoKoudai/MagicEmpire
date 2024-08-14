public interface IStateMachine
{
    public abstract void Enter();
    public abstract void Exit(Player.MoveState change);
    public abstract void Update();
    public abstract void FixedUpdate();
}
