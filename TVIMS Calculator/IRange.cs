namespace TVIMS_Calculator
{
    interface IRange<T>
    {
        Pair<T, int>[] Frequencies { get; }
        int FrequenciesSum { get; }
        int this[int index] { get; }
    }
}
