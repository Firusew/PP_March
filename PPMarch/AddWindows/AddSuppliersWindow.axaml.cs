using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PPMarch.Entites;

namespace PPMarch.AddWindows;

public partial class AddSuppliersWindow : Window
{
    public PostgresContext _dbContext { get; set; }
    public Supplier CurrentSupplier { get; set; }
    public AddSuppliersWindow(PostgresContext dbContext, Supplier currentSupplier)
    {
        InitializeComponent();
        this._dbContext = dbContext;
        this.CurrentSupplier = currentSupplier;
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var nameTextBox = this.FindControl<TextBox>("NameTextBox");
        var addressTextBox = this.FindControl<TextBox>("AddressTextBox");
        var contactpersonTextBox = this.FindControl<TextBox>("ContactpersonTextBox");
        var phoneTextBox = this.FindControl<TextBox>("PhoneTextBox");
        var emailTextBox = this.FindControl<TextBox>("EmailTextBox");
        
        var name = nameTextBox.Text;
        var address = addressTextBox.Text;
        var contactperson = contactpersonTextBox.Text;
        var phone = phoneTextBox.Text;
        var email = emailTextBox.Text;

            try
            {
                if (name != null)
                {
                    if (CurrentSupplier != null)
                    {
                        this.CurrentSupplier.Name = name;
                        this.CurrentSupplier.Address = address;
                        this.CurrentSupplier.Contactperson = contactperson;
                        this.CurrentSupplier.Phone = phone;
                        this.CurrentSupplier.Email = email;
                        _dbContext.SaveChanges();
                        
                        var newMain = new MainWindow();
                        newMain.Show();
        
                        this.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Пожалуйста, заполните не заполненные поля.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        var newMain = new MainWindow();
        newMain.Show();
        
        this.Close();
    }
}