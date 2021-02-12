using CoordinateDataModels;
using Mapsui.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackDataDroid.ViewModels
{
    public class LayerViewModel<T> : BaseViewModel
    {
        private T _layerData;
        public T LayerData
        {
            get { return _layerData; }
            set { SetProperty(ref _layerData, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private ILayer _layer;
        public ILayer Layer
        {
            get { return _layer; }
            set { SetProperty(ref _layer, value); }
        }

    }
}
