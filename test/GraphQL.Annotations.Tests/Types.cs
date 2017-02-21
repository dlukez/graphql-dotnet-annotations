namespace GraphQL.Annotations.Tests
{
    public class Counter
    {
        public int Value { get; set; }

        public int Increment()
        {
            return ++Value;
        }

        public bool HasParity(Parity parity)
        {
            return parity == Parity.Even
                ? Value % 2 == 0
                : Value % 2 == 1;
        }
    }

    public enum Parity
    {
        Even,
        Odd
    }
}