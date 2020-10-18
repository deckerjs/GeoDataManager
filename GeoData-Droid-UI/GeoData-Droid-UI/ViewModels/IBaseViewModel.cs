using System.ComponentModel;

namespace sensortest.ViewModels
{
    public interface IBaseViewModel
    {
        bool IsLoading { get; set; }
        string Title { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}