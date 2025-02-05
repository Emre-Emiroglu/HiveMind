using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories.CrossScene
{
    public sealed class CurrencyTrailFactory : PlaceholderFactory<CurrencyTrailData, CurrencyTrailMediator> { }
    public sealed class CurrencyTrailPool : MonoPoolableMemoryPool<CurrencyTrailData, IMemoryPool, CurrencyTrailMediator> { }
}
