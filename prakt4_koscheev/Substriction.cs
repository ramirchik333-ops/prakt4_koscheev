using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace prakt4_koscheev
{
    public class Subscription : INotifyPropertyChanged
    {
        private int _id;
        private string _owner = "";
        private decimal _monthlyPrice;
        private int _monthsCount;
        private decimal _discount; 

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MonthlyPrice
        {
            get => _monthlyPrice;
            set
            {
                if (_monthlyPrice != value)
                {
                    _monthlyPrice = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalCost)); 
                }
            }
        }

        public int MonthsCount
        {
            get => _monthsCount;
            set
            {
                if (_monthsCount != value)
                {
                    _monthsCount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalCost));
                }
            }
        }

        public decimal Discount
        {
            get => _discount;
            set
            {
                if (_discount != value)
                {
                    _discount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalCost));
                }
            }
        }

        
        public decimal TotalCost
        {
            get
            {
                decimal baseCost = MonthlyPrice * MonthsCount;
                decimal discountMultiplier = 1 - (Discount / 100m);
                return baseCost * discountMultiplier;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}