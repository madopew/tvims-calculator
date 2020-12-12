namespace TVIMS_Calculator.Interfaces
{
    interface ISampleRange<T>
    {
        public Pair<T, int>[] Frequencies { get; }
        public int FrequenciesSum { get; }
        public int this[int index] { get; }
    }
}
