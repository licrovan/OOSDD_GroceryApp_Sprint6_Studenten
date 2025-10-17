using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly GlobalViewModel _globalViewModel;
        public ObservableCollection<Product> Products { get; set; }

        public bool IsAdmin => _globalViewModel.Client != null && _globalViewModel.Client.Role == Role.Admin;

        public ProductViewModel(IProductService productService, GlobalViewModel globalViewModel)
        {
            _productService = productService;
            _globalViewModel = globalViewModel;
            Products = [];
            LoadProducts();
        }

        private void LoadProducts()
        {
            Products.Clear();
            foreach (Product p in _productService.GetAll()) 
            {
                Products.Add(p);
            }
        }

        [RelayCommand]
        private async Task AddNewProduct()
        {
            if (!IsAdmin)
            {
                await Application.Current.MainPage.DisplayAlert("Geen toegang", "Alleen administrators mogen nieuwe producten aanmaken.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("NewProductView");
        }

        public void RefreshProducts()
        {
            LoadProducts();
        }
    }
}
