
public abstract class BaseSate<T>
{
    public abstract void EnterState(T script);
    public abstract void IdleState(T script);
    public abstract void ExitState(T script);
}
