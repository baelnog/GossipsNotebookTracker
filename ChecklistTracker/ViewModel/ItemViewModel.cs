using ChecklistTracker.Config;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.ViewModel
{
    internal class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Item Item { get; set; }

        public ImageSource CurrentImage { get; }

        public int? IndexOverride { get; set; }

        public void Collect(int n)
        {

        }

        public void Uncollect(int n)
        {

        }
    }
}
