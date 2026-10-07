using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public static class WorldVersions
    {
        public static List<WorldBase> CreateAll() => new()
        {
            new WorldVersion_20100616(),
            new WorldVersion_20100617_to_alpha(),
            new WorldVersion_Alpha_1_2_2(),
            new WorldVersion_1_0_0_rc1(),
            new WorldVersion_1_8(),
            new WorldVersion_1_15(),
            new WorldVersion_Modern(),
        };

        public static string[] GetNames()
        {
            var list = CreateAll();
            var names = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                names[i] = list[i].VersionName;
            return names;
        }
    }
}