using System.Drawing;

ConfigHandler<ConfigData> configHandler = new("config.txt");
if (args.Length <= 0)
{
    configHandler.EditConfig();
    return;
}


for (int i = 0; i < args.Length; i++)
{
    string path = args[i];
    if (Directory.Exists(path))
        foreach (string filePath in Directory.GetFiles(path))
            HandleFile(filePath);
    else HandleFile(path);
}

Console.WriteLine("Done.");
Console.WriteLine("Press any key to close this window.");
Console.ReadKey();


void HandleFile(string path)
{
    if (!File.Exists(path)) return;

    Console.WriteLine("Currently processing file " + path);

    Bitmap? bmp = null;

    bool hasValidExtension = Path.GetExtension(path) switch
    {
        ".png" or ".bmp" or ".jpg" or ".tiff" or ".gif" or ".exif" => true,
        _ => false,
    };
    if (hasValidExtension) bmp = new(path);
    else Console.WriteLine("Cancelling because file is of invalid type: " + Path.GetExtension(path));
    
    if (bmp == null) return;

    //
    // Do magic here
    //

    string[] ascii = GenerateAscii(bmp, configHandler.data.chars, configHandler.data.inverted, configHandler.data.curveFilter);

    foreach (var line in ascii)
        Console.WriteLine(line);

    string newPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".txt";
    File.WriteAllLines(newPath, ascii);
}

static string[] GenerateAscii(Bitmap bmp, string values, bool invert = false, string curveFilter = "cubicOut")
{
    int asciiWidth = bmp.Width;
    int asciiHeight = (int)(bmp.Height * .5f);

    string[] lines = new string[asciiHeight];

    for (int y = 0; y < asciiHeight; y++)
    {
        string line = "";
        for (int x = 0; x < asciiWidth; x++)
        {
            float brightness = Sample(bmp, x, y);
            if (invert) brightness = 1f - brightness;
            brightness = ApplyCurve(brightness, curveFilter);

            line += values[(int)((values.Length - 1) * brightness)];
        }
        lines[y] = line;
    }

    return lines;

    float Sample(Bitmap bmp, int x, int y)
    {
        float sample1 = bmp.GetPixel(x, y * 2).GetBrightness();
        float sample2 = bmp.GetPixel(x, y * 2 + 1).GetBrightness();
        return (sample1 + sample2) * .5f;
    }

    float ApplyCurve(float v, string type) // Easing functions from https://easings.net/
    {
        switch (type)
        {
            case "cubicInOut":
                return v < 0.5f ? 4f * v * v * v : 1f - (float)Math.Pow(-2f * v + 2f, 3f) / 2f;

            case "cubicIn":
                return v * v * v;

            case "cubicOut":
                return 1f - (float)Math.Pow(1f - v, 3f);

            case "quintOut":
                return 1f - (float)Math.Pow(1f - v, 5f);

            case "quintIn":
                return v * v * v * v * v;

            case "bounceOut":
                float n1 = 7.5625f;
                float d1 = 2.75f;

                if (v < 1f / d1)
                    return n1 * v * v;
                else if (v < 2f / d1)
                    return n1 * (v -= 1.5f / d1) * v + 0.75f;
                else if (v < 2.5f / d1)
                    return n1 * (v -= 2.25f / d1) * v + 0.9375f;
                else
                    return n1 * (v -= 2.625f / d1) * v + 0.984375f;

            case "bounceIn":
                return 1f - ApplyCurve(1f - v, "bounceOut");

            case "bounceInOut":
                return v < 0.5f ? (1f - ApplyCurve(1f - 2f * v, "bounceOut")) / 2f : (1f + ApplyCurve(2f * v - 1f, "bounceOut")) / 2f;

            default: return v;
        }
    }
}

[Serializable]
public class ConfigData
{
    public string chars;
    public bool inverted;
    public string curveFilter;

    public ConfigData()
    {
        chars = " .:-=+*#%";
        inverted = true;
        curveFilter = "cubicOut";
    }
}
