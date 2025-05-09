﻿using Data;
using Presentation.Model;
using Presentation.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Data;
using System.Windows.Input;

namespace Presentation.ViewModel;

internal class PoolViewModel
{
    private readonly PoolModel _poolModel;
    private string _spawnAmountString = string.Empty;
    private readonly CreateBallsCommand _createBallsCommand;

    public ICommand CreateBallsCommand => _createBallsCommand;
    public ObservableCollection<BallRenderData> Balls => _poolModel.Balls;

    public string SpawnAmountText
    {
        get => _spawnAmountString;
        set
        {
            if (value.Equals("-"))
            {
                _spawnAmountString = value;
                return;
            }
            if (!int.TryParse(value, out int num) && value.Length != 0)
            {
                return;
            }

            if (num > 256)
            {
                _spawnAmountString = "256";
                return;
            }
            else if (num < -256)
            {
                _spawnAmountString = "-256";
                return;
            }

            _spawnAmountString = value;
        }
    }

    public PoolViewModel()
    {
        _poolModel = new PoolModel();
        _createBallsCommand = new CreateBallsCommand(_poolModel);
    }
}
