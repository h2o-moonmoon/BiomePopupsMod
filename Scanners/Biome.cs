using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomePopupsMod.Scanners;

internal class Biome
{
    public string Name;
    public int Frames;

    public Biome(string name, int frames = 0)
    {
        Name = name;
        Frames = frames;
    }
}
