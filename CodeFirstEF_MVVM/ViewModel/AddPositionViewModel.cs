using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace CodeFirstEF_MVVM
{
    public class AddPositionViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void PropertyChanging(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string name;
        int price;
        string category;
        List<string> categories;
        ShopContext context;

        public ICommand CloseButton
        {
            get
            {
                return new ButtonsCommand(() =>
           {
               ViewModel.addPosition.Close();
           }
           );
            }
        }

        public ICommand UpdateButton
        {
            get
            {
                return new ButtonsCommand(() =>
          {
              Product update = context.Products.Find(ViewModel.selectedItem.Id);
              update.Name = Name;
              update.Price = Price;
              update.Category = Category;
              context.SaveChanges();
          });
            }
        }
        public ICommand AddButton
        {
            get
            {
                return new ButtonsCommand(() =>
                {
                    context.Products.Add(
                        new Product()
                        {
                            Price = price,
                            Name = name,
                            Category = category
                        }
                        );
                    context.SaveChanges();
                    ViewModel.addPosition.Close();
                });
            }
        }

        public AddPositionViewModel()
        {
            if (ViewModel.selectedItem == null) return;
            Name = ViewModel.selectedItem.Name;
            Price = ViewModel.selectedItem.Price;
            Category = ViewModel.selectedItem.Category;
            context = new ShopContext();
            //Categories = (from prod in context.Products
            //             where true
            //             select prod.Category).ToList();

            Categories = new List<string>();
            foreach (Product cat in context.Products)
            {
                if (categories.Contains(cat.Category) == false)
                {
                    Categories.Add(cat.Category);
                }
            }
            Categories = Categories;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanging("Name");
            }
        }
        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                PropertyChanging("Price");
            }
        }
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                PropertyChanging("Category");
            }
        }
        public List<string> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                categories = new List<string>(categories);
                PropertyChanging("Categories");
            }
        }

    }
}
