using System.Collections.Generic;

namespace MinecraftLavaEdition
{
    public static class Simulation
    {
        public static List<Snapshot> Run(WorldBase world)
        {
            if (world is WorldVersion_1_15)
                return Simulation115.Run(world);

            return SimulationOld.Run(world);
        }
    }
}