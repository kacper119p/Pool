using Data;
using Logic;

namespace Presentation;

internal class PoolModel
{
    private ISimulationController _simulationController;
    private string _spawnAmountString = string.Empty;
    public string SpawnAmountString { get => _spawnAmountString; set => _spawnAmountString = value; }

    public PoolModel()
    {
        _simulationController = new PoolController(new PoolTable(100, 100));
    }
}
