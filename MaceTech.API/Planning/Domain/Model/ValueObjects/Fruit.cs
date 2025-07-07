namespace MaceTech.API.Planning.Domain.Model.ValueObjects;

public record Fruit(bool Present)
    {
        public Fruit() : this(false)
        {}
    }