namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public class VisualIdentification
{
    public string GrowthHabit { get; set; }
    public Leaf Leaf { get; set; }
    public Flower Flower { get; set; }
    public Fruit Fruit { get; set; }

    // Constructor sin parámetros requerido por EF Core
    public VisualIdentification()
    {
        GrowthHabit = string.Empty;
        Leaf = new Leaf();
        Flower = new Flower();
        Fruit = new Fruit();
    }

    public VisualIdentification(
        string growthHabit,
        string leafShape,
        string leafRelativeSize,
        List<string> leafTexture,
        string leafEdge,
        string leafPattern,
        string leafMainColors,
        List<string> leafSecondaryColor,
        bool flowerPresent,
        List<string> flowerColor,
        string flowerShape,
        bool flowerFragance,
        bool fruitPresent)
    {
        GrowthHabit = growthHabit;
        Leaf = new Leaf(leafShape, leafRelativeSize, leafTexture, leafEdge, leafPattern, leafMainColors, leafSecondaryColor);
        Flower = new Flower(flowerPresent, flowerColor, flowerShape, flowerFragance);
        Fruit = new Fruit(fruitPresent);
    }

    public VisualIdentification(VisualIdentification original)
    {
        GrowthHabit = original.GrowthHabit;
        Leaf = original.Leaf;
        Flower = original.Flower;
        Fruit = original.Fruit;
    }
}