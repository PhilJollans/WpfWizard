using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineXamlDemo
{
  internal class vmMainWindow : ObservableObject
  {
    private bool _enableNextStepOne = true ;

    public bool EnableNextStepOne { get => _enableNextStepOne ; set => SetProperty ( ref _enableNextStepOne, value ) ; }
  }
}
