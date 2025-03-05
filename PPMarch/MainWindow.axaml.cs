using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PPMarch.AddWindows;
using PPMarch.EditWindows;
using PPMarch.Entites;
using System.Reflection;

namespace PPMarch;

public partial class MainWindow : Window
{
    public PostgresContext _dbContext;
    
    public List<Supplier> Suppliers { get; set; }
    public List<Material> Materials { get; set; }
    public List<Purchase> Purchases { get; set; }
    public List<Purchasedetail> Purchasedetails { get; set; }
    public List<Warehouse> Warehouses { get; set; }
    public List<Warehousestock> Warehousestocks { get; set; }
    public List<Materialmovement> Materialmovements { get; set; }
    public List<Materialquality> Materialqualities { get; set; }

    private List<Supplier> _originalSuppliers;
    private List<Material> _originalMaterials;
    private List<Purchase> _originalPurchases;
    private List<Purchasedetail> _originalPurchasedetails;
    private List<Warehouse> _originalWarehouses;
    private List<Warehousestock> _originalWarehousestocks;
    private List<Materialmovement> _originalMaterialmovements;
    private List<Materialquality> _originalMaterialqualities;
    
    private Dictionary<string, string> _supplierColumnMap = new Dictionary<string, string>
    {
        {"Наименование", "Name"},
        {"Адрес", "Address"},
        {"Контактное лицо", "Contactperson"},
        {"Телефон", "Phone"},
        {"Email", "Email"}
    };
    private Dictionary<string, string> _materialColumnMap = new Dictionary<string, string>
    {
        {"Наименование", "Name"},
        {"Описание", "Description"},
        {"Единица измерения", "Unit"},
        {"Минимальный запас", "Minstock"}
    };
    private Dictionary<string, string> _purchaseColumnMap = new Dictionary<string, string>
    {
        {"Дата закупки", "Purchasedate"},
        {"Поставщик", "Supplier.Name"},
        {"Статус закупки", "Status"},
        {"Общая сумма", "Totalamount"}
    };
    private Dictionary<string, string> _purchaseDetailsColumnMap = new Dictionary<string, string>
    {
        {"Дата закупки", "Purchase.Purchasedate"},
        {"Материал", "Material.Name"},
        {"Количество", "Quantity"},
        {"Цена за единицу", "Unitprice"}
    };
    private Dictionary<string, string> _warehouseColumnMap = new Dictionary<string, string>
    {
        {"Название", "Name"},
        {"Адрес", "Address"},
        {"Вместимость", "Capacity"}
    };
    private Dictionary<string, string> _warehouseStockColumnMap = new Dictionary<string, string>
    {
        {"Название склада", "Warehouse.Name"},
        {"Материал", "Material.Name"},
        {"Количество", "Quantity"},
        {"Последнее обновление", "Lastupdated"}
    };
    private Dictionary<string, string> _materialMovementColumnMap = new Dictionary<string, string>
    {
        {"Материал", "Material.Name"},
        {"Название склада", "Warehouse.Name"},
        {"Тип движения", "Movementtype"},
        {"Количество", "Quantity"},
        {"Дата движения", "Movementdate"}
    };
    private Dictionary<string, string> _materialQualityColumnMap = new Dictionary<string, string>
    {
        {"Материал", "Material.Name"},
        {"Дата проверки", "Checkdate"},
        {"Результат", "Result"},
        {"Комментарий", "Comment"}
    };

    public MainWindow()
    {
        InitializeComponent();
        _dbContext = new PostgresContext();

        LoadData();
        
        TableTabs.SelectionChanged += TableTabs_SelectionChanged;
        FilterTextBox.TextChanged += FilterTextBox_TextChanged;
        ColumnComboBox.SelectionChanged += ColumnComboBox_SelectionChanged;
        
        // Инициализация комбобокса при запуске
        PopulateColumnComboBox(DataGridSuppliers);
    }

    private void LoadData()
    {
        Suppliers = _dbContext.Suppliers
            .ToList();
        
        Materials = _dbContext.Materials
            .ToList();

        Purchases = _dbContext.Purchases
            .Include(x => x.Supplier)
            .ToList();
        
        Purchasedetails = _dbContext.Purchasedetails
            .Include(x => x.Purchase)
            .Include(x => x.Material)
            .ToList();

        Warehouses = _dbContext.Warehouses
            .ToList();
        
        Warehousestocks = _dbContext.Warehousestocks
            .Include(x => x.Warehouse)
            .Include(x => x.Material)
            .ToList();
        
        Materialmovements = _dbContext.Materialmovements
            .Include(x => x.Material)
            .Include(x => x.Warehouse)
            .ToList();

        Materialqualities = _dbContext.Materialqualities
            .Include(x => x.Material)
            .ToList();

        _originalSuppliers = Suppliers.ToList();
        _originalMaterials = Materials.ToList();
        _originalPurchases = Purchases.ToList();
        _originalPurchasedetails = Purchasedetails.ToList();
        _originalWarehouses = Warehouses.ToList();
        _originalWarehousestocks = Warehousestocks.ToList();
        _originalMaterialmovements = Materialmovements.ToList();
        _originalMaterialqualities = Materialqualities.ToList();
        
        DataGridSuppliers.ItemsSource = Suppliers;
        DataGridMaterials.ItemsSource = Materials;
        DataGridPurchases.ItemsSource = Purchases;
        DataGridPurchaseDetails.ItemsSource = Purchasedetails;
        DataGridWarehouse.ItemsSource = Warehouses;
        DataGridWarehouseStock.ItemsSource = Warehousestocks;
        DataGridMaterialMovement.ItemsSource = Materialmovements;
        DataGridMaterialQuality.ItemsSource = Materialqualities;
    }

    private void ClearFilter_Click(object? sender, RoutedEventArgs e)
    {
        FilterTextBox.Text = string.Empty;
        ApplyFilter();
    }

    private void TableTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TableTabs.SelectedItem is TabItem selectedTab)
        {
            // Восстановление оригинальных данных и обновление DataGrid
            switch (selectedTab.Header.ToString())
            {
                case "Поставщики":
                    Suppliers = _originalSuppliers.ToList(); // Восстанавливаем оригинальные данные
                    DataGridSuppliers.ItemsSource = Suppliers;
                    PopulateColumnComboBox(DataGridSuppliers);
                    break;
                case "Материалы":
                    Materials = _originalMaterials.ToList(); // Восстанавливаем оригинальные данные
                    DataGridMaterials.ItemsSource = Materials;
                    PopulateColumnComboBox(DataGridMaterials);
                    break;
                case "Закупки":
                    Purchases = _originalPurchases.ToList(); // Восстанавливаем оригинальные данные
                    DataGridPurchases.ItemsSource = Purchases;
                    PopulateColumnComboBox(DataGridPurchases);
                    break;
                case "Детали закупок":
                    Purchasedetails = _originalPurchasedetails.ToList(); // Восстанавливаем оригинальные данные
                    DataGridPurchaseDetails.ItemsSource = Purchasedetails;
                    PopulateColumnComboBox(DataGridPurchaseDetails);
                    break;
                case "Склады":
                    Warehouses = _originalWarehouses.ToList(); // Восстанавливаем оригинальные данные
                    DataGridWarehouse.ItemsSource = Warehouses;
                    PopulateColumnComboBox(DataGridWarehouse);
                    break;
                case "Остатки на складах":
                    Warehousestocks = _originalWarehousestocks.ToList(); // Восстанавливаем оригинальные данные
                    DataGridWarehouseStock.ItemsSource = Warehousestocks;
                    PopulateColumnComboBox(DataGridWarehouseStock);
                    break;
                case "Движение материалов":
                    Materialmovements = _originalMaterialmovements.ToList(); // Восстанавливаем оригинальные данные
                    DataGridMaterialMovement.ItemsSource = Materialmovements;
                    PopulateColumnComboBox(DataGridMaterialMovement);
                    break;
                case "Качество материалов":
                    Materialqualities = _originalMaterialqualities.ToList(); // Восстанавливаем оригинальные данные
                    DataGridMaterialQuality.ItemsSource = Materialqualities;
                    PopulateColumnComboBox(DataGridMaterialQuality);
                    break;
            }

            ApplyFilter(); // Применяем фильтр (если он есть)
        }
    }
    
    // Метод для заполнения комбобокса названиями столбцов
    private void PopulateColumnComboBox(DataGrid dataGrid)
    {
        ColumnComboBox.Items.Clear();
        Dictionary<string, string> columnMap = GetColumnMap(dataGrid);
        foreach (var columnHeader in columnMap.Keys)
        {
            ColumnComboBox.Items.Add(columnHeader);
        }
        ColumnComboBox.SelectedIndex = 0;
    }
    
    // Метод для получения словаря соответствия для текущей таблицы
    private Dictionary<string, string> GetColumnMap(DataGrid dataGrid)
    {
        if (dataGrid == DataGridSuppliers)
        {
            return _supplierColumnMap;
        }
        else if (dataGrid == DataGridMaterials)
        {
            return _materialColumnMap;
        }
        else if (dataGrid == DataGridPurchases)
        {
            return _purchaseColumnMap;
        }
        else if (dataGrid == DataGridPurchaseDetails)
        {
            return _purchaseDetailsColumnMap;
        }
        else if (dataGrid == DataGridWarehouse)
        {
            return _warehouseColumnMap;
        }
        else if (dataGrid == DataGridWarehouseStock)
        {
            return _warehouseStockColumnMap;
        }
        else if (dataGrid == DataGridMaterialMovement)
        {
            return _materialMovementColumnMap;
        }
        else if (dataGrid == DataGridMaterialQuality)
        {
            return _materialQualityColumnMap;
        }
        return new Dictionary<string, string>(); // Возвращаем пустой словарь, если таблица не найдена
    }
    
    // Обработчик изменения текста в поле фильтра
    private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }
    
    // Обработчик изменения выбранного столбца в комбобоксе
    private void ColumnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        string filterText = FilterTextBox.Text?.ToLower() ?? string.Empty;
        string selectedColumnHeader = ColumnComboBox.SelectedItem?.ToString() ?? string.Empty;

        if (TableTabs.SelectedItem is TabItem selectedTab)
        {
            // Фильтрация в зависимости от выбранной вкладки
            switch (selectedTab.Header.ToString())
            {
                case "Поставщики":
                    ApplyFilterToTable(Suppliers, _originalSuppliers, DataGridSuppliers, _supplierColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Материалы":
                    ApplyFilterToTable(Materials, _originalMaterials, DataGridMaterials, _materialColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Закупки":
                    ApplyFilterToTable(Purchases, _originalPurchases, DataGridPurchases, _purchaseColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Детали закупок":
                    ApplyFilterToTable(Purchasedetails, _originalPurchasedetails, DataGridPurchaseDetails, _purchaseDetailsColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Склады":
                    ApplyFilterToTable(Warehouses, _originalWarehouses, DataGridWarehouse, _warehouseColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Остатки на складах":
                    ApplyFilterToTable(Warehousestocks, _originalWarehousestocks, DataGridWarehouseStock, _warehouseStockColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Движение материалов":
                    ApplyFilterToTable(Materialmovements, _originalMaterialmovements, DataGridMaterialMovement, _materialMovementColumnMap, selectedColumnHeader, filterText);
                    break;
                case "Качество материалов":
                    ApplyFilterToTable(Materialqualities, _originalMaterialqualities, DataGridMaterialQuality, _materialQualityColumnMap, selectedColumnHeader, filterText);
                    break;
            }
        }
    }
    
    private void ApplyFilterToTable<T>(List<T> filteredList, List<T> originalList, DataGrid dataGrid, Dictionary<string, string> columnMap, string selectedColumnHeader, string filterText)
    {
        if (string.IsNullOrEmpty(selectedColumnHeader) || !columnMap.ContainsKey(selectedColumnHeader))
        {
            dataGrid.ItemsSource = originalList.ToList();
            return;
        }

        string propertyName = columnMap[selectedColumnHeader];
        filteredList.Clear();
        filteredList.AddRange(originalList.Where(item => GetPropertyValue(item, propertyName).ToLower().Contains(filterText)));
        dataGrid.ItemsSource = filteredList.ToList();
    }
    
    private string GetPropertyValue(object obj, string propertyName)
    {
        if (obj == null || string.IsNullOrEmpty(propertyName))
            return string.Empty;

        try
        {
            // Разбиваем имя свойства на части, если это связанное свойство (например, "Client.Name")
            string[] propertyParts = propertyName.Split('.');
            object currentObj = obj;

            // Проходим по всем частям свойства
            foreach (string part in propertyParts)
            {
                if (currentObj == null)
                    return string.Empty;

                PropertyInfo propertyInfo = currentObj.GetType().GetProperty(part);

                if (propertyInfo == null)
                    return string.Empty;

                currentObj = propertyInfo.GetValue(currentObj);
            }

            // Если currentObj все еще null, возвращаем пустую строку
            if (currentObj == null)
                return string.Empty;

            // Возвращаем строковое представление значения
            return currentObj.ToString();
        }
        catch (Exception ex)
        {
            // В случае ошибки возвращаем пустую строку и выводим сообщение об ошибке в консоль
            Console.WriteLine($"Error getting property value for {propertyName}: {ex.Message}");
            return string.Empty;
        }
    }

    private void Add_Click(object? sender, RoutedEventArgs e)
    {
        if (TableTabs.SelectedItem is TabItem selectedTab)
        {
            switch (selectedTab.Header.ToString())
            {
                case "Поставщики":
                    AddSupplier();
                    break;
            }
        }
    }

    private void AddSupplier()
    {
        var newSupplier = new Supplier();
        _dbContext.Suppliers.Add(newSupplier);
        
        var newSupplierAdd = new AddSuppliersWindow(this._dbContext, newSupplier);
        newSupplierAdd.Show();
        
        this.Close();
    }
    
    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        if (TableTabs.SelectedItem is TabItem selectedTab)
        {
            try
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Поставщики":
                        DeleteSupplier();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
            }
        }
    }

    private void DeleteSupplier()
    {
        var SelItem = DataGridSuppliers.SelectedItem as Supplier;
        if (SelItem == null)
        {
            return;
        }

        _dbContext.Suppliers.Remove(SelItem);
        _dbContext.SaveChanges();
        LoadData();
    }
    
    private void Edit_Click(object? sender, RoutedEventArgs e)
    {
        if (TableTabs.SelectedItem is TabItem selectedTab)
        {
            try
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Поставщики":
                        EditSupplier();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
            }
        }
    }

    private void EditSupplier()
    {
        var selectedSupplier = DataGridSuppliers.SelectedItem as Supplier;
        if (selectedSupplier == null)
        {
            return;
        }

        var editSupplierWindow = new EditSuppliersWindow(_dbContext, selectedSupplier);
        editSupplierWindow.Show();
       
        this.Close();
    }
}