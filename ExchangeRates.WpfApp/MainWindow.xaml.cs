using ExchangeRates.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ExchangeRates.WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CurrencyCourse> _exhangeRates = new List<CurrencyCourse>();

        public MainWindow()
        {
            InitializeComponent();

            exchangeRatesDataGrid.IsReadOnly = true;
            UpdateExchageRates();
        }

        private void ExchangeRatesUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateExchageRates();
        }

        private void UpdateExchageRates()
        {
            _exhangeRates.Clear();

            XmlDocument rssXmlDoc = new XmlDocument();

            rssXmlDoc.Load("http://nationalbank.kz/?getpg=outurl&out=https://nationalbank.kz/rss/rates_all.xml");

            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string currencyName = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string currencyCost = rssSubNode != null ? rssSubNode.InnerText : "";

                _exhangeRates.Add(new CurrencyCourse() { Name = currencyName, Cost = Convert.ToDouble(currencyCost.Replace('.', ',')) });
            }

            exchangeRatesDataGrid.ItemsSource = _exhangeRates;
        }
    }
}
