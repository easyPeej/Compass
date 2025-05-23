﻿namespace Compass.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels

type ReportCountUC() as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()

    do
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.FetchReportCount()
        viewModel.FetchReportCountByStatus("Open")
        viewModel.FetchReportCountByStatus("Resolved")
        viewModel.FetchReportCountByStatus("Under Review")
