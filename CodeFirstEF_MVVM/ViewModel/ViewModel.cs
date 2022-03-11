using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace CodeFirstEF_MVVM
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SelectionChangedEventArgs SelectionChanged;
        public void PropertyChanging(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool rb1;
        bool rb2;
        bool rb3;
        string findText;
        ShopContext myShop;
        List<Product> products;
        List<Product> clientProducts;
        public static Product selectedItem;
        public static AddPosition addPosition;
        public static UpdatePosition updatePosition;


        public bool Rb
        {
            get { return rb1; }
            set
            {
                rb1 = value;
                PropertyChanging("Rb1");
                RefreshData(findText);

            }
        }
        public bool Rb2
        {
            get { return rb2; }
            set
            {
                rb2 = value;
                PropertyChanging("Rb2");
                RefreshData(findText);
            }
        }
        public bool Rb3
        {
            get { return rb3; }
            set
            {
                rb3 = value;
                PropertyChanging("Rb3");
                RefreshData(findText);
            }
        }


        public ViewModel()
        {
            myShop = new ShopContext();
            products = new List<Product>();
            clientProducts = new List<Product>();
            RefreshData();
        }

        void RefreshData(string text = "")
        {
            if (rb1)
            {
                products = (from prod in myShop.Products
                            where prod.Name.Contains(text)
                            orderby prod.Name
                            select prod).ToList();
            }
            else if (rb2)
            {
                products = (from prod in myShop.Products
                            where prod.Name.Contains(text)
                            orderby prod.Price
                            select prod).ToList();
            }
            else
            {
                products = (from prod in myShop.Products
                            where prod.Name.Contains(text)
                            orderby prod.Category
                            select prod).ToList();
            }
            Products = products;
        }
        public string FindText
        {
            get { return findText; }
            set
            {
                findText = value;
                RefreshData(findText);
                PropertyChanging("FindText");
            }
        }
        public List<Product> Products
        {
            get { return products; }
            set
            {
                products = new List<Product>(products);
                PropertyChanging("Products");
            }
        }
        public ICommand AddButton
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  addPosition = new AddPosition();
                  addPosition.ShowDialog();
                  RefreshData();
              }
              );
            }
        }

        public ICommand UpdatePosition
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  updatePosition = new UpdatePosition();
                  updatePosition.ShowDialog();
                  RefreshData();
              }
              );
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  if (selectedItem != null)
                  {
                      MessageBoxResult result = MessageBox.Show($"Действительно удалить продукт {SelectedItem.Id} - {SelectedItem.Name}", "Удалить",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                      if (result == MessageBoxResult.Yes)
                      {
                          myShop.Products.Remove(SelectedItem);
                          myShop.SaveChanges();
                          RefreshData();
                      }
                  }
              }
              );
            }
        }

        // Cancel
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        public ICommand Cancel
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        if (ClientProducts.Count != 0)
                        {
                            ClientProducts = new List<Product>();
                        }
                    }
                    );
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////// 


        //Order
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        public ICommand Order
        {
            get
            {
                return new ButtonsCommand(
                    () =>
                    {
                        SortedDictionary<int, double> dictionary = new SortedDictionary<int, double>();

                        for (int i = 0; i < ClientProducts.Count(); ++i)
                        {
                            if (!dictionary.ContainsKey(ClientProducts[i].Id))
                            {
                                dictionary.Add(ClientProducts[i].Id, ClientProducts[i].Price);
                            }
                            else
                            {
                                dictionary[ClientProducts[i].Id] += ClientProducts[i].Price;
                            }
                        }

                        string orderString = string.Empty;

                        foreach (var diction in dictionary)
                        {
                            var clientPoductOne = ClientProducts.Where(prod => prod.Id == diction.Key).Take(1);
                            var count = ClientProducts.Where(prod => prod.Id == diction.Key).Count();
                            foreach (var product in clientPoductOne)
                            {
                                orderString += $"id - {product.Id}, count - {count}, name - {product.Name}, price - {product.Price}\n";
                            }
                        }

                        MessageBox.Show($"{orderString}");
                    }
                    );
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //Реализуйте кнопку «Купить».
        //При нажатии на кнопку, выделенный товар должен добавляться во второй listbox
        //////////////////////////////////////////////////////////////////////////////////////////////////////      
        public ICommand BuyProduct
        {
            get
            {
                return new ButtonsCommand(
              () =>
              {
                  if (selectedItem != null)
                  {
                      clientProducts.Add(selectedItem);
                      List<Product> clone = new List<Product>(clientProducts.Count);
                      clientProducts.ForEach((index) => { clone.Add(index); });
                      ClientProducts = clone;
                  }
              }
              );
            }
        }

        public void NewMethod()
        {
            ClientProducts = new List<Product>();
        }

        public List<Product> ClientProducts
        {
            get { return clientProducts; }
            set
            {
                clientProducts = value;
                PropertyChanging("ClientProducts");
            }
        }

        public Product SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                PropertyChanging("SelectedItem");
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
