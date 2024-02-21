using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;


namespace praktikaMagaz
{
    /// <summary>
    /// Логика взаимодействия для korzina.xaml
    /// </summary>
    public partial class korzina : Window
    {
        public int count = 0;
        int columns = 0;
        int row = 0;
        int mm = 0;

        List<TextBlock> dynamicTextBlocks = new List<TextBlock>();
        List<TextBlock> dynamicTextCost = new List<TextBlock>();
        public korzina()
        {
            InitializeComponent();


            var contex = new AppDbContext();
            var q = contex.Korzinas.Count();
            var l = contex.Korzinas.Where(x => x.Id > 0).ToList();
            while (count < q)
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                string a = l[count].Image.ToString();
                image.Source = new BitmapImage(new Uri($"{a}", UriKind.RelativeOrAbsolute));
                
                TextBlock textBlock = new TextBlock();
                textBlock.Text = l[count].Name;
                textBlock.TextWrapping = TextWrapping.Wrap;
                
                Button button = new Button();
                button.Content = "Удалить";
                button.Width = 90;
                button.Height = 35;
                button.Template = (ControlTemplate)Resources["овальная кнопка"];
                button.CommandParameter = a;
                button.Click += Button_Click;

                TextBlock textCost = new TextBlock();
                textCost.Text = l[count].Cost;
                textCost.TextWrapping = TextWrapping.Wrap;
                textCost.Name = "TB1_" + count.ToString();

                TextBlock textCount = new TextBlock();
                textCount.Text = l[count].Count;
                textCount.TextWrapping = TextWrapping.Wrap;
                textCount.Name = "TB_"+ count.ToString();

                

                Button buttonPlus = new Button();
                buttonPlus.Content = "+";
                buttonPlus.Width = 35;
                buttonPlus.Height = 35;
                buttonPlus.Template = (ControlTemplate)Resources["овальная кнопка"];
                buttonPlus.CommandParameter = textCount.Name;
                buttonPlus.Click += ButtonPlus_Click;

                Button buttonMinus = new Button();
                buttonMinus.Content = "-";
                buttonMinus.Width = 35;
                buttonMinus.Height = 35;
                buttonMinus.Template = (ControlTemplate)Resources["овальная кнопка"];
                buttonMinus.CommandParameter = textCount.Name;
                buttonMinus.Click += ButtonMinus_Click;

                
                

                Grid.SetColumn(image, columns);
                Grid.SetRow(image, row);
                Grid.SetColumn(textBlock, columns + 1);
                Grid.SetRow(textBlock, row);
                Grid.SetColumn(button, columns + 6);
                Grid.SetRow(button, row);
                Grid.SetColumn(buttonPlus, columns + 4);
                Grid.SetRow(buttonPlus, row);
                Grid.SetColumn(buttonMinus, columns + 2);
                Grid.SetRow(buttonMinus, row);
                Grid.SetColumn(textCount, columns + 3);
                Grid.SetRow(textCount, row);
                Grid.SetColumn(textCost, columns + 5);
                Grid.SetRow(textCost, row);
               
                dd.Children.Add(image);
                dd.Children.Add(textBlock);
                dd.Children.Add(button);
                dd.Children.Add(buttonPlus);
                dd.Children.Add(buttonMinus);
                dd.Children.Add(textCount);
                dd.Children.Add(textCost);
                
                dynamicTextBlocks.Add(textCount);
                dynamicTextCost.Add(textCost);
                row++;
                count++;

                string[] name = textCount.Name.Split("_");
                int num = Convert.ToInt32(l[Convert.ToInt32(name[1])].Cost);
                int countNum = Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text);
                dynamicTextCost[Convert.ToInt32(name[1])].Text = (countNum * num).ToString();


                mm += Convert.ToInt32(dynamicTextCost[count - 1].Text);
            }
            TextBlock Summa = new TextBlock();
            Summa.TextWrapping = TextWrapping.Wrap;
            dd.Children.Add(Summa);
            Grid.SetColumn(Summa, columns);
            Grid.SetRow(Summa, row+8);
            Grid.SetColumnSpan(Summa, 2);
            Summa.Text = "Итоговая сумма: " + mm.ToString();
        }

        private void Kata_Click(object sender, RoutedEventArgs e)
        {
            katalog katalog = new katalog();
            this.Close();
            katalog.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var context = new AppDbContext();
            string par = button.CommandParameter as string;
            var r = context.Korzinas.Where(x => x.Image == par).ToList();
            context.Korzinas.Remove(r[0]);
            context.SaveChanges();
            korzina korzina = new korzina();
            
            korzina.Show();
            this.Close();
        }

        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            
            Button buttonPlus = sender as Button;
            string text = buttonPlus.CommandParameter as string;
            var l = context.Korzinas.Where(x => x.Id > 0).ToList();
            string[] name = text.Split("_");
            dynamicTextBlocks[Convert.ToInt32(name[1])].Text = (Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text) + 1).ToString();
            int num = Convert.ToInt32(l[Convert.ToInt32(name[1])].Cost);
            int countNum = Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text);
            dynamicTextCost[Convert.ToInt32(name[1])].Text = (countNum * num).ToString();
            
            var h = context.Korzinas.Where(x => x.Id == l[0].Id).AsEnumerable().Select(x => { x.Count = dynamicTextBlocks[Convert.ToInt32(name[1])].Text; return x; });
            foreach (var x in h)
            {
                context.Entry(x).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            context.SaveChanges();

        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            var l = context.Korzinas.Where(x => x.Id > 0).ToList();

            Button buttonMinus = sender as Button;
            string text = buttonMinus.CommandParameter as string;

            string[] name = text.Split("_");
            if (Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text) > 1)
            {
                dynamicTextBlocks[Convert.ToInt32(name[1])].Text = (Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text) - 1).ToString();
                int num = Convert.ToInt32(l[Convert.ToInt32(name[1])].Cost);
                int countNum = Convert.ToInt32(dynamicTextBlocks[Convert.ToInt32(name[1])].Text);
                dynamicTextCost[Convert.ToInt32(name[1])].Text = ( countNum * num).ToString();
            }
            var h = context.Korzinas.Where(x => x.Id == l[0].Id).AsEnumerable().Select(x => { x.Count = dynamicTextBlocks[Convert.ToInt32(name[1])].Text; return x; });
            foreach (var x in h)
            {
                context.Entry(x).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            context.SaveChanges();

        }

        private void byu_Click(object sender, RoutedEventArgs e)
        {
            var context = new AppDbContext();
            var t = context.Korzinas.ToList();
            MessageBox.Show($"Ваш заказ оформлен");
            context.Korzinas.RemoveRange(t);
            context.SaveChanges();
            korzina basket = new korzina();
            basket.Show();
            this.Close();
        }
    }
}
