using System.Collections.Generic;

public class RegisterSystems
{
    public static List<ISystem> GetListOfSystems()
    {
        // determine order of systems to add
        List<ISystem> toRegister = new List<ISystem>();

        // AJOUTEZ VOS SYSTEMS ICI
        toRegister.Add(new InitializationSystem());
        toRegister.Add(new CoolDownSystem());

        CollisionSystem collisionSystem           = new CollisionSystem();
        PositionUpdateSystem positionUpdateSystem = new PositionUpdateSystem();
        TagSystem tagSystem                       = new TagSystem();
        ColoringSystem coloringSystem             = new ColoringSystem();
        SaveStateSystem saveStateSystem           = new SaveStateSystem();
        RestoreStateSystem restoreStateSystem     = new RestoreStateSystem();

        for (uint i = 0; i < 5; i++)
        {
            toRegister.Add(tagSystem);
            toRegister.Add(collisionSystem);
            toRegister.Add(positionUpdateSystem);
            toRegister.Add(coloringSystem);
            toRegister.Add(saveStateSystem);
            toRegister.Add(restoreStateSystem);
        }
        return toRegister;
    }
}