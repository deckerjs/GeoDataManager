﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sensortest.Models;
using sensortest.Services;
using sensortest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
namespace sensortest.Views
{
    public class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            _viewModel = viewModel;
            Content = GetContent();

        }

        private View GetContent()
        {
            return new StackLayout { Children = { new Label { Text = "Main Page Content" } } };
        }
    }
}
