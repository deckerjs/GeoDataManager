using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackDataDroid.ViewModels
{
    public class TrackSummaryViewModel: BaseViewModel
    {
        private CoordinateDataSummary _coordinateData;
        public CoordinateDataSummary CoordinateData
        {
            get { return _coordinateData; }
            set { SetProperty(ref _coordinateData, value); }
        }

        private bool _showOnMap;
        public bool ShowOnMap
        {
            get { return _showOnMap; }
            set { SetProperty(ref _showOnMap, value); }
        }
    }
}
