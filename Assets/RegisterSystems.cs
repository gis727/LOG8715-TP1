using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        List<ISystem> toRegister = new List<ISystem>();

        // AJOUTEZ VOS SYSTEMS ICI
        toRegister.Add(new InitializationSystem());
        toRegister.Add(new CollisionSystem());
        toRegister.Add(new PositionUpdateSystem());
        toRegister.Add(new TagSystem());
        toRegister.Add(new ColoringSystem());
        toRegister.Add(new SaveStateSystem());
        toRegister.Add(new CoolDownSystem());
        toRegister.Add(new RestoreStateSystem());

        return toRegister;
    }
}