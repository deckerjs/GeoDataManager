using System.ComponentModel;

namespace XamlFreeDroidUI.ViewModels
{
    public interface IBaseViewModel
    {
        bool IsLoading { get; set; }
        string Title { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}