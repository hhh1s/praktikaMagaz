using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using System.Windows.Shapes;

namespace praktikaMagaz
{
    /// <summary>
    /// Логика взаимодействия для katalog.xaml
    /// </summary>
    public partial class katalog : Window
    {
        int count = 0;
        int columns = 0;
        int row = 0;
        string temp = "овальная кнопка";
        int mm = 0;
        public katalog()
        {
            InitializeComponent();
            var contex = new AppDbContext();
            var q = contex.Tovars.Count();
            var l = contex.Korzinas.Where(x => x.Id > 0).ToList();
            int ss = l.Sum(x => Convert.ToInt32(x.Count));
            var w = contex.Tovars.Where(x => x.Id > 0).ToList();
            while (count < q)
            {
                if (columns == 4)
                {
                    columns = 0;
                    row += 3;
                    if (row == 2)
                    {
                        break;
                    }
                }
                Image image = new Image();
                string a = w[count].Image.ToString();
                image.Source = new BitmapImage(new Uri($"{a}", UriKind.RelativeOrAbsolute));
                TextBlock textBlock = new TextBlock();
                textBlock.Text = w[count].Name;
                textBlock.TextWrapping = TextWrapping.Wrap;
                Button button = new Button(); 
                button.Content = w[count].Cost.ToString() + " руб.";
                button.Width = 150;
                button.Height = 35;
                button.Template = (ControlTemplate)Resources["овальная кнопка"];
                button.CommandParameter = a;
                button.Click += Button_Click;
                Button Info = new Button();
                Info.Template = (ControlTemplate)Resources["кнопка"];
                Info.CommandParameter = a;
                Info.Click += Info_Click;
                counts.Text = ss.ToString();
                Grid.SetColumn(image, columns); 
                Grid.SetRow(image, row);
                Grid.SetColumn(textBlock, columns);
                Grid.SetRow(textBlock, row + 1);
                Grid.SetColumn(button, columns); 
                Grid.SetRow(button, row + 2);
                Grid.SetColumn(Info, columns);
                Grid.SetRow(Info, row);
                Grid.SetRowSpan(Info, 2);
                dd.Children.Add(image);
                dd.Children.Add(textBlock);
                dd.Children.Add(button);
                dd.Children.Add(Info);
                columns++;
                count++;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var context = new AppDbContext();
            string par = button.CommandParameter as string;
            var q = context.Tovars.Where(x => x.Image == par).ToList();
            var r = context.Korzinas.Where(x => x.Image == par).ToList(); 
            if (r.Count > 0)
            {
                if (q[0].Id == r[0].Id)
                {
                    string price = (Convert.ToInt32(r[0].Count) + 1).ToString();
                    var h = context.Korzinas.Where(x => x.Id == r[0].Id).AsEnumerable().Select(x => { x.Count = price; return x; });
                    foreach (var x in h)
                    {
                        context.Entry(x).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    context.SaveChanges();
                }
            }
            else
            {
                var tov = new Korzina { Id = q[0].Id, Image = q[0].Image, Name = q[0].Name, Count = "1", Cost = q[0].Cost }; 
                context.Korzinas.Add(tov);
            }
            context.SaveChanges(); 
            var l = context.Korzinas.Where(x => x.Id > 0).ToList();
            int ss = l.Sum(x => Convert.ToInt32(x.Count));
            counts.Text = ss.ToString();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Button Info= sender as Button;
            string par = Info.CommandParameter as string;
            DataStrore.Data = par;
            info Information = new info();
            Information.Show();
            this.Close();
        }

        private void Korz_Click(object sender, RoutedEventArgs e)
        {
            korzina korzina = new korzina();
            korzina.Show();
            this.Close();
        }
    }
}
