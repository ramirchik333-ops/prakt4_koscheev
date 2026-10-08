using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace prakt4_koscheev
{
    public partial class MainWindow : Window
    {
        private const string DataFolder = "SubscriptionsData";
        private Subscription CurrentSubscription;

        public MainWindow()
        {
            InitializeComponent();

         
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            CurrentSubscription = (Subscription)Resources["CurrentSubscription"];

          
            CreateNewSubscription();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            CreateNewSubscription();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSubscription();
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            FindSubscription();
        }

        private void CreateNewSubscription()
        {
     
            int newId = GenerateUniqueId();

            CurrentSubscription.Id = newId;
            CurrentSubscription.Owner = "";
            CurrentSubscription.MonthlyPrice = 0;
            CurrentSubscription.MonthsCount = 1;
            CurrentSubscription.Discount = 0;

            MessageBox.Show($"Создан новый абонемент с ID: {newId}", "Новая запись",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private int GenerateUniqueId()
        {
            int id = 1;

            while (File.Exists(Path.Combine(DataFolder, $"subscription_{id}.json")))
            {
                id++;
            }
            return id;
        }

        private void SaveSubscription()
        {
            try
            {
                string fileName = $"subscription_{CurrentSubscription.Id}.json";
                string filePath = Path.Combine(DataFolder, fileName);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(CurrentSubscription, options);

                File.WriteAllText(filePath, json);

                MessageBox.Show($"Абонемент ID {CurrentSubscription.Id} успешно сохранен!",
                                "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FindSubscription()
        {
            if (string.IsNullOrWhiteSpace(txtSearchId.Text))
            {
                MessageBox.Show("Введите ID для поиска", "Поиск",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtSearchId.Text, out int searchId))
            {
                MessageBox.Show("ID должен быть целым числом", "Ошибка ввода",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string fileName = $"subscription_{searchId}.json";
                string filePath = Path.Combine(DataFolder, fileName);

                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Абонемент с ID {searchId} не найден", "Поиск",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string json = File.ReadAllText(filePath);
                var loadedSubscription = JsonSerializer.Deserialize<Subscription>(json);

                if (loadedSubscription != null)
                {
                   
                    CurrentSubscription.Id = loadedSubscription.Id;
                    CurrentSubscription.Owner = loadedSubscription.Owner;
                    CurrentSubscription.MonthlyPrice = loadedSubscription.MonthlyPrice;
                    CurrentSubscription.MonthsCount = loadedSubscription.MonthsCount;
                    CurrentSubscription.Discount = loadedSubscription.Discount;

                    MessageBox.Show($"Абонемент ID {searchId} успешно загружен", "Поиск",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}