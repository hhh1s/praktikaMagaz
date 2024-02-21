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
    /// Логика взаимодействия для info.xaml
    /// </summary>
    public partial class info : Window
    {

        public info()
        {
            InitializeComponent();
            string data = DataStrore.Data;
            var contex = new AppDbContext();
            string gggg = data;
            var w = contex.Tovars.Where(x => x.Image == gggg).ToList();
            Image image = new Image();
            string a = w[0].Image.ToString();
            image.Source = new BitmapImage(new Uri($"{gggg}", UriKind.RelativeOrAbsolute));
            TextBlock textBlock = new TextBlock();
            textBlock.Text = w[0].Name;

            textBlock.TextWrapping = TextWrapping.Wrap;
            TextBlock textBlock1 = new TextBlock();
            textBlock1.Text = w[0].Info;

            textBlock1.TextWrapping = TextWrapping.Wrap;
            Button button = new Button();
            button.Content = w[0].Cost.ToString() + " руб.";
            button.Width = 150;
            button.Height = 35;
            button.Template = (ControlTemplate)Resources["овальная кнопка"];
            button.CommandParameter = a;
            button.Click += Button_Click;
            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            Grid.SetColumn(textBlock, 0);
            Grid.SetRow(textBlock, 1);
            Grid.SetColumn(button, 2);
            Grid.SetRow(button, 0);

            Grid.SetColumn(textBlock1, 1);
            Grid.SetRow(textBlock1, 0);

            dd.Children.Add(image);
            dd.Children.Add(textBlock);
            dd.Children.Add(button);
            dd.Children.Add(textBlock1);
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
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            katalog katalog = new katalog();
            katalog.Show();
            this.Close();
        }
    }
}
