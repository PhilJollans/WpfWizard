using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControlDemo.Steps;
using MvvmWizard.Classes;

namespace UserControlDemo
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private StepOne   _stepOne   = new StepOne();
    private StepTwo   _stepTwo   = new StepTwo();
    private StepThree _stepThree = new StepThree();
    private StepFour  _stepFour  = new StepFour();

    public MainWindow()
    {
      WizardSettings.Instance.ViewResolver = ResolveType ;
      InitializeComponent();
    }

    public object ResolveType(Type stepType)
    {
      if (stepType == typeof(StepOne))
      {
        return _stepOne;
      }
      else if (stepType == typeof(StepTwo))
      {
        return _stepTwo;
      }
      else if (stepType == typeof(StepThree))
      {
        return _stepThree;
      }
      else if (stepType == typeof(StepFour))
      {
        return _stepFour;
      }
      else
      {
        return null;
      }
    }
  }
}
