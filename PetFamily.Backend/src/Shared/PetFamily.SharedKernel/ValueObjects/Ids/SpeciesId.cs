﻿using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects.Ids;

public class SpeciesId : ComparableValueObject
{
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    private SpeciesId()
    {
    }

    public Guid Value { get; }

    public static SpeciesId NewSpeciesId() => new SpeciesId(Guid.NewGuid());
    public static SpeciesId Empty => new SpeciesId(Guid.Empty);
    public static SpeciesId Create(Guid value) => new SpeciesId(value);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(SpeciesId id) => id.Value;
}