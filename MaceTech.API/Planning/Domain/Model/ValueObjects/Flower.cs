namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public record Flower
{
    public bool Present { get; set; }
    public List<string> Color { get; set; } = new List<string>();
    public string Shape { get; set; }
    public bool Fragance { get; set; }

    public Flower()
    {
        Present = false;
        Color = new List<string>();
        Shape = string.Empty;
        Fragance = false;
    }

    public Flower(bool present, List<string> color, string shape, bool fragance)
    {
        Present = present;
        Color = color;
        Shape = shape;
        Fragance = fragance;
    }

    public Flower(Flower original)
    {
        Present = original.Present;
        Color = original.Color;
        Shape = original.Shape;
        Fragance = original.Fragance;
    }
}