using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sensortest.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace sensortest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Content = new StackLayout
            {
                Padding = new Thickness(10, 0, 10, 0),
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button 
                        { 
                            VerticalOptions = LayoutOptions.Center, 
                            Text = "Login" 
                        }.BindCommand(nameof(LoginViewModel.LoginCommand))
                }
            };

            this.BindingContext = new LoginViewModel();
        }
    }
}