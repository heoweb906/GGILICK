public interface BaseState
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
    public abstract void OnAnimationEnterEvent();
    public abstract void OnAnimationExitEvent();
    public abstract void OnAnimationTransitionEvent();

}
