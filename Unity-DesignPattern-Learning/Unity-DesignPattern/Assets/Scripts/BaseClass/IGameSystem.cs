using System;

public abstract class IGameSystem
{
    protected GameFacade mFacade;

    public virtual void Init() 
    {
        mFacade = GameFacade.Instance;
    }
    public virtual void Update() { }
    public virtual void Release() { }
}
