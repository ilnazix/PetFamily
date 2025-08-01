using CSharpFunctionalExtensions;
using System.Collections;


namespace PetFamily.SharedKernel
{
    public class ValueObjectList<T> : ValueObject, IReadOnlyList<T>
        where T : ValueObject
    {
        public IReadOnlyList<T> Values { get; }

        public int Count => Values.Count;

        private ValueObjectList() { }
        public ValueObjectList(IEnumerable<T> values)
        {
            Values = values.ToList().AsReadOnly();
        }

        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int index] => Values[index];

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var item in Values)
            {
                yield return item;
            }
        }

        public static implicit operator List<T>(ValueObjectList<T> list)
            => list.Values.ToList();

        public static implicit operator ValueObjectList<T>(List<T> list)
            => new ValueObjectList<T>(list);
    }
}
