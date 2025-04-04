using System;

namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums
{
    [Flags]
    public enum FollowTypes
    {
        None = 0,
        Position = 1,
        Rotation = 2,
        Everything = 3,
    }

    public enum LerpTypes
    {
        Lerp,
        NonLerp
    }
}
