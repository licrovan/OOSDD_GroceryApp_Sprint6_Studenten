using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly GlobalViewModel _globalViewModel;

        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private int stock = 0;

        [ObservableProperty]
        private DateOnly shelfLife = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

        [ObservableProperty]
        private decimal price = 0.00m;

        [ObservableProperty]
        private string errorMessage = "";

        [ObservableProperty]
        private bool isAdmin = false;

        public NewProductViewModel(IProductService productService, GlobalViewModel globalViewModel)
        {
            _productService = productService;
            _globalViewModel = globalViewModel;
            
            // Check if current user is admin
            IsAdmin = _globalViewModel.Client?.Role == Role.Admin;
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            ErrorMessage = "";

            if (!IsAdmin)
            {
                ErrorMessage = "Alleen administrators mogen nieuwe producten aanmaken.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Productnaam is verplicht.";
                return;
            }

            if (Stock < 0)
            {
                ErrorMessage = "Voorraad moet 0 of hoger zijn.";
                return;
            }

            if (Price < 0 || Price > 999.99m)
            {
                ErrorMessage = "Prijs moet tussen 0 en 999.99 zijn.";
                return;
            }

            try
            {
                var newProduct = new Product(0, Name, Stock, ShelfLife, Price);
                _productService.Add(newProduct);
                
                // Navigate back to products view
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij het toevoegen van product: {ex.Message}";
            }
        }

        partial void OnShelfLifeChanged(DateOnly value)
        {
            // Ensure the shelf life is not in the past
            if (value < DateOnly.FromDateTime(DateTime.Now))
            {
                ShelfLife = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}