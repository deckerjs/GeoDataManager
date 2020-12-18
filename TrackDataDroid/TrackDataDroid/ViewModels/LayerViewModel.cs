using CoordinateDataModels;
using Mapsui.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackDataDroid.ViewModels
{
    public class LayerViewModel : BaseViewModel
    {

        private ILayer _layer;

        public ILayer Layer
        {
            get { return _layer; }
            set { SetProperty(ref _layer, value); }
        }


    }
}
