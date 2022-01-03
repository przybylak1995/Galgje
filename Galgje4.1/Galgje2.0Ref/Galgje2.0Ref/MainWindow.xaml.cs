using Galgje2._0;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Galgje2._0Ref
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
     bool checkWord = false, btnRaadIsGeKikt = false;
        bool letterGevonden = false;
        string fouteLetters;
        char[] geheimWoord;
        char[] geradenwoord;
        int pictNumber = 1;
         DispatcherTimer timer2 = new DispatcherTimer();
        int time = 10;

    public MainWindow()
    {
        InitializeComponent();
        Timer1();
        Afteller();
        btnRaad.IsEnabled = false;
        btnVerbergWoord.IsDefault = true;
    }

    private void btnNieuwspel_Click(object sender, RoutedEventArgs e)
    {
        GameEnd();
    }
    private void Timer_Tick(object sender, EventArgs e) // weergeven tijd in label

    {
        lblTime.Content = DateTime.Now.ToString("HH:mm:ss");


    }
    private void btnRaad_Click(object sender, RoutedEventArgs e)
    {
        btnRaadIsGeKikt = true;
        string userInput = txbWoord.Text;
        letterGevonden = false;
        if (pictNumber == 11)
        {
                imgHangMan.Source = new BitmapImage(new Uri(@"img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
                GameEnd();
            return;
        }
        if (txbWoord.Text.Length > 1)
        {
            RaadHetWoord();
            return;
        }
        if (txbWoord.Text.Length == 1)
        {
            RaadLetter(userInput[0]);

            if (letterGevonden == false)
            {
                fouteLetters += userInput;
                lblInfo.Content = $"Foute letters: {fouteLetters}";
                MessageBox.Show("Je hebt fout geraden");
                txbWoord.Text = "";
                    imgHangMan.Source = new BitmapImage(new Uri(@"img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
                    pictNumber++;
                return;
            }

        }
        PlaatsLetter();
        txbWoord.Text = string.Empty;
    }

    private void RaadHetWoord()
    {
        string gwoord = new string(geheimWoord);
        if (gwoord == txbWoord.Text)
        {
            MessageBox.Show($"Je hebt het woord {gwoord} geraden");

            GameEnd();
        }
        else if (gwoord != txbWoord.Text)
        {
                imgHangMan.Source = new BitmapImage(new Uri(@"img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
                txbWoord.Text = "";
            pictNumber++;
        }
    }

    private void RaadLetter(char letter)
    {
        for (int i = 0; i < geheimWoord.Length; i++)                           //Gaat over de lengte van het woord
        {
            if (letter.Equals(geheimWoord[i]))                                  //Kijkt of het ingegeven karakter voorkomt in het te raden woord
            {

                geradenwoord[i] = letter;                                       //Zet de _ om in het juist geraden karakter
                letterGevonden = true;

            }
        }

    }

    private void btnVerbergWoord_Click(object sender, RoutedEventArgs e)
    {
        geheimWoord = txbWoord.Text.ToCharArray();

        geradenwoord = new char[geheimWoord.Length];
        if (string.IsNullOrWhiteSpace(txbWoord.Text) || checkWord == true || txbWoord.Text.Length < 3)
        {
            MessageBox.Show("Check de spelregels");
            GameEnd();
            return;
        }
        else if (checkWord == false)
        {
            timer2.Start();        // timer voor antwoord
            btnRaad.IsEnabled = true;
            txbWoord.Text = string.Empty;
            imgHangMan.Source = new BitmapImage(new Uri(@"img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
            pictNumber++;
            lblInfo.Content = $"Foute letters: {fouteLetters}";
            btnVerbergWoord.Visibility = Visibility.Hidden;
            btnRaad.IsDefault = true;
        }
        for (int i = 0; i < geradenwoord.Length; i++)
        {
            geradenwoord[i] = '*';
        }
        PlaatsLetter();
    }

    private void PlaatsLetter()
    {
        string woord = "";

        for (int i = 0; i < geradenwoord.Length; i++)
        {
            woord += geradenwoord[i] + " ";
        }

        lblArrayWoord.Content = $"{woord}";
    }

    private void GameEnd() // Wat als woord niet word geraden ??
    {
        timer2.IsEnabled = false;
        lblAfteller.Content = "10";
        time = 10;
        btnRaad.IsEnabled = false;
        gridKleur.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        lblInfo.Content = "Geef een geheim woord in:";
        btnVerbergWoord.IsDefault = true;
        txbWoord.Text = "";
        lblArrayWoord.Content = "";
        fouteLetters = "";
        pictNumber = 1;
        btnRaad.IsDefault = false;
        btnVerbergWoord.Visibility = Visibility.Visible;
            imgHangMan.Source = new BitmapImage(new Uri(@"img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
        }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Info info = new Info();
        info.Show();
    }
    private void Timer1() // weergaven van de tijd 
    {
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = new TimeSpan(0, 0, 1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }
    private void Afteller() // weergaven van de tijd 
    {
        timer2.Tick += Timer_Tick1;
        timer2.Interval = new TimeSpan(0, 0, 1);
        // timer2.Start();
    }

    private void Timer_Tick1(object sender, EventArgs e) // afteller timer   nog foutje met overslaan van de teller
    {

        lblAfteller.Content = time;
        time--;
        if (time < 0 && letterGevonden == false)
        {
            timer2.Stop();
            gridKleur.Background = new SolidColorBrush(Colors.Red);
            imgHangMan.Source = new BitmapImage(new Uri(@"C:\Users\krist\source\repos\Galgje2.0\Galgje2.0\img\hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
            pictNumber++;
            time = 10;
        }
        if (btnRaadIsGeKikt == true)
        {
            timer2.Stop();
            time = 10;
            timer2.Start();
            btnRaadIsGeKikt = false;
            letterGevonden = false;
            return;
        }
        if (time > 9)
        {
            MessageBox.Show("Sneller raden");
            timer2.Start();
            gridKleur.Background = new SolidColorBrush(Colors.Black);
        }

    }

}
}
