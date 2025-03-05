using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PPMarch.Entites;
using Microsoft.EntityFrameworkCore;

namespace PPMarch.EditWindows;

public partial class EditSuppliersWindow : Window
{
    public PostgresContext _dbContext { get; set; }
    public Supplier _supplier;
    public EditSuppliersWindow(PostgresContext dbContext, Supplier supplier)
    {
        InitializeComponent();
        _dbContext = dbContext;
        _supplier = supplier;
        
        DataContext = _supplier;
        
        //_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        //_supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _dbContext.Suppliers.Update(_supplier);
            await _dbContext.SaveChangesAsync();
            
            var newMain = new MainWindow();
            newMain.Show();
        
            this.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
        }
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        var newMain = new MainWindow();
        newMain.Show();
        
        this.Close();
    }
}