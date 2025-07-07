namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public record Leaf
{
    public string Shape { get; set; }
    public string RelativeSize { get; set; }
    public List<string> Texture { get; set; } = new List<string>();
    public string Edge { get; set; }
    public string Pattern { get; set; }
    public string MainColors { get; set; }
    public List<string> SecondaryColor { get; set; } = new List<string>();

    // Constructor sin parámetros requerido por EF Core
    public Leaf()
    {
        Shape = string.Empty;
        RelativeSize = string.Empty;
        Texture = new List<string>();
        Edge = string.Empty;
        Pattern = string.Empty;
        MainColors = string.Empty;
        SecondaryColor = new List<string>();
    }

    public Leaf(
        string shape,
        string relativeSize,
        List<string> texture,
        string edge,
        string pattern,
        string mainColors,
        List<string> secondaryColor)
    {
        Shape = shape;
        RelativeSize = relativeSize;
        Texture = texture;
        Edge = edge;
        Pattern = pattern;
        MainColors = mainColors;
        SecondaryColor = secondaryColor;
    }

    public Leaf(Leaf original)
    {
        Shape = original.Shape;
        RelativeSize = original.RelativeSize;
        Texture = original.Texture;
        Edge = original.Edge;
        Pattern = original.Pattern;
        MainColors = original.MainColors;
        SecondaryColor = original.SecondaryColor;
    }
}