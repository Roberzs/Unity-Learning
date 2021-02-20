using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DP10Command : MonoBehaviour
{
    private void Start()
    {
        DPInvoker invoker = new DPInvoker();

        ConcreteCommand1 cmd1 = new ConcreteCommand1(new DPReceiver1());
        invoker.AddCommand(cmd1);

        invoker.ExecuteCommand();
    }
}

/** 命令管理者 */
public class DPInvoker
{
    private List<DPICommand> commands = new List<DPICommand>();

    public void AddCommand(DPICommand command)
    {
        commands.Add(command);
    }

    public void ExecuteCommand()
    {
        foreach (var cmd in commands)
        {
            cmd.Execute();
        }
        commands.Clear();
    }
}

/** 命令 */
public abstract class DPICommand
{
    public abstract void Execute();
}

public class ConcreteCommand1 : DPICommand
{
    DPIReceiver receiver;

    public ConcreteCommand1(DPIReceiver receiver)
    {
        this.receiver = receiver;
    }

    public override void Execute()
    {
        receiver.Action("Command1");
    }
}

/** 命令执行者 */

public abstract class DPIReceiver
{
    public abstract void Action(string cmd);
}

public class DPReceiver1: DPIReceiver
{

    public override void Action(string cmd)
    {
        Debug.Log("Receiver1执行了命令" + cmd);
    }
}