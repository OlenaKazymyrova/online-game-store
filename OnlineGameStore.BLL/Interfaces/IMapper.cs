namespace OnlineGameStore.BLL.Interfaces;

public interface IMapper<TSource, TDestination>
{
    static abstract TDestination Map(TSource source);

    static abstract TSource Map(TDestination source);

    static abstract IEnumerable<TDestination> Map(IEnumerable<TSource> source);

    static abstract IEnumerable<TSource> Map(IEnumerable<TDestination> source);
}