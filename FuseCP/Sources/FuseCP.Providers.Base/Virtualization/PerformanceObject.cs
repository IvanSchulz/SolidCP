// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuseCP.Providers.Virtualization
{
    public enum PerformanceType 
    { 
        Processor = 0,
        Memory = 1,
        Network = 2,
        DiskIO = 4
    }

    public partial class PerformanceDataValue : object, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private System.Nullable<double> SampleValueField;

        private System.DateTime TimeAddedField;

        private System.DateTime TimeSampledField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        public System.Nullable<double> SampleValue
        {
            get
            {
                return this.SampleValueField;
            }
            set
            {
                if ((this.SampleValueField.Equals(value) != true))
                {
                    this.SampleValueField = value;
                    this.RaisePropertyChanged("SampleValue");
                }
            }
        }

        public System.DateTime TimeAdded
        {
            get
            {
                return this.TimeAddedField;
            }
            set
            {
                if ((this.TimeAddedField.Equals(value) != true))
                {
                    this.TimeAddedField = value;
                    this.RaisePropertyChanged("TimeAdded");
                }
            }
        }

        public System.DateTime TimeSampled
        {
            get
            {
                return this.TimeSampledField;
            }
            set
            {
                if ((this.TimeSampledField.Equals(value) != true))
                {
                    this.TimeSampledField = value;
                    this.RaisePropertyChanged("TimeSampled");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
