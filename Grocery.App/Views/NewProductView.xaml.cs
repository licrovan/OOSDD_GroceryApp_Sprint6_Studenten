using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class NewProductView : ContentPage
{
    private NewProductViewModel _viewModel;

    public NewProductView(NewProductViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        
        // Bind DatePicker manually since DateOnly binding is complex
        ShelfLifePicker.SetBinding(DatePicker.DateProperty, new Binding(nameof(NewProductViewModel.ShelfLife), 
            converter: new DateOnlyToDateTimeConverter(), 
            mode: BindingMode.TwoWay));
    }
}

public class DateOnlyToDateTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }
        return DateTime.Now;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }
        return DateOnly.FromDateTime(DateTime.Now);
    }
}