﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using Avalonia.Controls;

using Nullinside.Api.Common;

using ReactiveUI;

using ApplicationNameUpperCamelCase.Models;

namespace ApplicationNameUpperCamelCase.ViewModels;

/// <summary>
///   The view model for the main UI.
/// </summary>
public class MainWindowViewModel : ViewModelBase {
  /// <summary>
  ///   True if the application is updating, false otherwise.
  /// </summary>
  private bool _isUpdating;
  /// <summary>
  ///   Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
  /// </summary>
  public MainWindowViewModel() {
    _isUpdating = Environment.GetCommandLineArgs().ToList().Contains("--update");
  }
  /// <summary>
  ///   True if the application is updating, false otherwise.
  /// </summary>
  public bool IsUpdating {
    get => _isUpdating;
    set => this.RaiseAndSetIfChanged(ref _isUpdating, value);
  }
}