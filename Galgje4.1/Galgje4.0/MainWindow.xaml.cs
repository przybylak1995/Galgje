using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Galgje4._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int randomNmber;
        public static int levenVerloren;
        bool checkWord = false, btnRaadIsGeKikt = false;
        bool letterGevonden = false; public static bool hintGebruikt = false;
        string fouteLetters;
        char[] geheimWoord;
        char[] geradenwoord;
        int pictNumber = 1; public static int rounds = 1;
        Menu menuGame = new Menu();
        //List<Players> players = new List<Players>();
        public DispatcherTimer timer2 = new DispatcherTimer();
        private string[] galgjeWoorden = new string[]
            {
                "grafeem","tjiftjaf","maquette","kitsch","pochet","convocaat",
                "jakkeren","collaps","zuivel","cesium","voyant","spitten","pancake",
                "gietlepel","karwats","dehydreren","viswijf","flater","cretonne","sennhut",
                "tichel","wijten","cadeau","trotyl","chopper","pielen","vigeren","vrijuit",
                "dimorf",
                "kolchoz",
                "janhen",
                "plexus",
                "borium",
                "ontweien",
                "quiche",
                "ijverig",
                "mecenaat",
                "falset",
                "telexen",
                "hieruit",
                "femelaar",
                "cohesie",
                "exogeen",
                "plebejer",
                "opbouw",
                "zodiak",
                "volder",
                "vrezen",
                "convex",
                "verzenden",
                "ijstijd",
                "fetisj",
                "gerekt",
                "necrose",
                "conclaaf",
                "clipper",
                "poppetjes",
                "looikuip",
                "hinten",
                "inbreng",
                "arbitraal",
                "dewijl",
                "kapzaag",
                "welletjes",
                "bissen",
                "catgut",
                "oxymoron",
                "heerschaar",
                "ureter",
                "kijkbuis",
                "dryade",
                "grofweg",
                "laudanum",
                "excitatie",
                "revolte",
                "heugel",
                "geroerd",
                "hierbij",
                "glazig",
                "pussen",
                "liquide",
                "aquarium",
                "formol",
                "kwelder",
                "zwager",
                "vuldop",
                "halfaap",
                "hansop",
                "windvaan",
                "bewogen",
                "vulstuk",
                "efemeer",
                "decisief",
                "omslag",
                "prairie",
                "schuit", "weivlies","ontzeggen","schijn","sousafoon"
            };

        int time;
        public MainWindow()
        {
            InitializeComponent();
            CheckMenuActive();
            Timer1();
            Afteller();
            btnRaad.IsEnabled = false;
            btnVerbergWoord.IsDefault = true;
        }

        private void CheckMenuActive()
        {
            if (Menu.isActive1 == true)
            {
                lblInfo.Content = "Klik op random";
                txbWoord.IsEnabled = false;
                btnVerbergWoord.Content = "Random woord";

            }

        }
  
        private void SinglePlayer() // als single player optie is gekozen word een een random woord gemaakt
        {
            Random rnd = new Random();
            txbWoord.IsEnabled = true;
            int random = rnd.Next(0, galgjeWoorden.Length);
            string randomWoord = galgjeWoorden[random];
            txbWoord.Visibility = Visibility.Hidden;
            txbWoord.Text = randomWoord;
        }

        private void btnNieuwspel_Click(object sender, RoutedEventArgs e) //Huidige spel met zelfde spelers resetten
        {
            mnuTimer.IsEnabled = true;
            rounds = 1;
            GameEnd();

        }
        private void AantalSpelers2() // aanpassen van het info menu en welke speler er aan zet is 
        {

            if (Menu.isActive1 != true)
            {
                if (rounds % 2 == 0)
                {
                    lblSpelerAanZet.Content = $"{Menu.player2}";
                    lblInfo.Content = $"{Menu.player1} geef een \ngeheim woord in:";
                }
                else
                {
                    lblSpelerAanZet.Content = $"{Menu.player1}";
                    lblInfo.Content = $"{Menu.player2} geef een \ngeheim woord in:";
                }
            }

        }

        private void btnVerbergWoord_Click(object sender, RoutedEventArgs e) // woord generegen voor spel te spelen
        {
            mnuTimer.IsEnabled = false;
            if (Menu.isActive1 == true)// als de speler de single player modus heeft gekozen
            {
                SinglePlayer(); //Random woord generator
            }

            geheimWoord = txbWoord.Text.Trim(' ').ToCharArray();
            time = Menu.dynalischeTimer;
            geradenwoord = new char[geheimWoord.Length];
            GeenCijfers();
            if (string.IsNullOrWhiteSpace(txbWoord.Text) || checkWord == true || txbWoord.Text.Length < 3)
            {
                MessageBox.Show("Check de spelregels");
                txbWoord.Text = "";
                return;
            }
            else if (checkWord == false)
            {

                timer2.Start();        // timer voor antwoord
                btnRaad.IsEnabled = true;
                btnHint.IsEnabled = true;
                txbWoord.Text = string.Empty;
                txbWoord.Visibility = Visibility.Visible;
                HangManAfb();
                pictNumber++;
                lblInfo.Content = $"Foute letters: {fouteLetters}";
                btnVerbergWoord.Visibility = Visibility.Hidden;
                btnRaad.IsDefault = true;
            }
            for (int i = 0; i < geradenwoord.Length; i++)
            {
                geradenwoord[i] = '*';
            }
            PlaatsLetter(); // Laad de lengte van het geheime wooord            
        }

        private void GeenCijfers()
        {
            bool check = int.TryParse(txbWoord.Text, out int letter);

            var regexItem = new Regex("^[a-zA-Z-09]*$"); // controle op speciale char 

            if (regexItem.IsMatch(txbWoord.Text))
            {
                check = false;
            }
            else
            {
                check = true;
            }

            if (check == true)
            {
                checkWord = true; // zolang checkword true is zijn er foute ingaven dus kan het spel niet starten
            }
            else
            {
                checkWord = false; // checkword is false dus geen fouten gevonden game kan starten
            }
        }
        // het raden van een char of een woord word eerst gecotroleerd als de letter al in de lijst met foute letters zit 
        // vervolgens is er een controle als je nog levens over hebt 
        // als het geraden woord meer dan 1 char bevat word het verwerkt als een woord anders als een letter
        //
        private void btnRaad_Click(object sender, RoutedEventArgs e)
        {

            btnRaadIsGeKikt = true;
            string userInput = txbWoord.Text;
            letterGevonden = false;
            CheckFouteLetters(userInput);
            if (pictNumber == 11)
            {
                levenVerloren++;
                TimerEnd();
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

                if (letterGevonden == false) // Foute letter nog niet ingegeven dus word toegevoegd en je verliest een leven 
                {
                    levenVerloren++;
                    fouteLetters += userInput;
                    lblInfo.Content = $"Foute letters: {fouteLetters}";
                    MessageBox.Show("Je hebt fout geraden");
                    txbWoord.Text = "";
                    HangManAfb();
                    pictNumber++;
                    return;
                }

            }
            PlaatsLetter();
            txbWoord.Text = string.Empty;

        }
        private void RaadLetter(char letter) // er word maar 1 char ingegeven en deze word gecontroleerd in het geheime woord.
        {
            for (int i = 0; i < geheimWoord.Length; i++)
            {
                if (letter.Equals(geheimWoord[i]))
                {

                    geradenwoord[i] = letter;
                    letterGevonden = true;

                }
            }
        }
        // controle als het woord dat getypt is overeenkomt met het geheime woord , zo jah word het spel afgesloten 
        // zo niet dan verlied je een leven en gaat het spel verder.
        private void RaadHetWoord()
        {
            string gwoord = new string(geheimWoord);
            if (gwoord == txbWoord.Text)
            {

                Menu.AddLevensVerloren();
                rounds++;
                MessageBox.Show($"Je hebt het woord {gwoord} geraden");

                GameEnd();
            }
            else if (gwoord != txbWoord.Text)
            {
                levenVerloren++;
                HangManAfb();
                txbWoord.Text = "";
                pictNumber++;
            }
        }
        // klok voor het weergeven van de tijd 
        private void Timer1()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }
        // De dynamische afteller
        private void Afteller()
        {
            timer2.Tick += Timer2_Tick;
            timer2.Interval = new TimeSpan(0, 0, 1);
        }
        // tick event van de afteller hier controleer ik al de teller op 0 staat maar er nog geen letter is ingegeven 
        // de knop menu om terug naar het start scherm tegaan verdwijnt zodra het spel start
        // als je levens opzijn word het spel gestopt 
        // als je een letter of woord ingeeft word de timer gereset 
        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblAfteller.Content = --time;

            if (time == 0 && letterGevonden == false)
            {
                TeTraagGeraden();
            }
            if (time == 0)
            {
                TeTraagGeraden();
            }
            if (pictNumber == 12)
            {

                TimerEnd();
            }
            if (btnRaadIsGeKikt == true)
            {
                time = Menu.dynalischeTimer;
                btnRaadIsGeKikt = false;
                letterGevonden = false;
                return;
            }
         
        }
        // je tijd is afgelopen dus krijg je een rode melding , vervolgens verlies je een leven en het spel gaat verder
        private void TeTraagGeraden()
        {
            time = Menu.dynalischeTimer;
            gridKleur.Background = new SolidColorBrush(Colors.Red);
            MessageBox.Show("Je tijd is om Sneller raden");
            gridKleur.Background = new SolidColorBrush(Colors.Black);
            levenVerloren++;
            HangManAfb();
            pictNumber++;


        }
        // je verliest het spel omdat je te traag.
        private void TimerEnd()
        {
            string gWoord = new string(geheimWoord);
            Menu.AddLevensVerloren();
            HangManAfb();
            timer2.Stop();
            MessageBox.Show($"Je hebt het spel verloren!! Het woord was {gWoord}");
            rounds++;
            GameEnd();
        }

        private void PlaatsLetter() // Plaats de gevonden juiste letter op de juiste plaats
        {
            string woord = "";

            for (int i = 0; i < geradenwoord.Length; i++)
            {
                woord += geradenwoord[i] + " ";
            }

            lblArrayWoord.Content = $"{woord}";
        }
        // alles word volledig gereset voor een nieuw spel te kunnen starten.
        private void GameEnd()
        {
            if (Menu.isActive1 == true)
            {
                txbWoord.IsEnabled = false;

            }
            lblInfo.Content = "Klik op random";
            hintGebruikt = false;
            levenVerloren = 0;
            AantalSpelers2();
            timer2.Stop();
            lblAfteller.Content = $"{Menu.dynalischeTimer}";
            time = Menu.dynalischeTimer;
            btnRaad.IsEnabled = false;
            gridKleur.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            btnVerbergWoord.IsDefault = true;
            txbWoord.Text = "";
            lblArrayWoord.Content = "";
            fouteLetters = "";
            pictNumber = 1;
            btnRaad.IsDefault = false;
            btnVerbergWoord.Visibility = Visibility.Visible;
            imgHangMan.Source = new BitmapImage(new Uri(@"img/game.jpg", UriKind.RelativeOrAbsolute));
            if (rounds == 3)
            {
                this.Hide();
                menuGame.Show();
                rounds = 1;
            }
        }
        // het inladen van de verschillende afbeeldingen van de hangman.
        private void HangManAfb()
        {
            imgHangMan.Source = new BitmapImage(new Uri(@"img/hang" + pictNumber + ".png", UriKind.RelativeOrAbsolute));
        }

        private void btnAfsluiten_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }    

        // het random verkijgen van een letter die niet in het woord voorkomt 
        // als hier op geklikt word kom je niet meer in aamerking voor het score bord
        private void btnHint_Click(object sender, RoutedEventArgs e)
        {
            HintVragen();


        }
        private void HintVragen ()
        {


            string oke = "";
            bool rndHintOke = true;
            Random rnd = new Random();
            if (btnRaad.IsEnabled == true)
            {

                hintGebruikt = true;

                do
                {
                    rndHintOke = true;
                    char[] hint = new char[]
                     {
                     'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                     };
                    randomNmber = rnd.Next(0, hint.Length);
                    for (int i = 0; i < geheimWoord.Length; i++)
                    {
                        if (geheimWoord[i] == hint[randomNmber])
                        {
                            rndHintOke = false;
                        }
                        else
                        {
                            oke = hint[randomNmber].ToString();
                        }

                    }
                } while (rndHintOke == false);

                MessageBox.Show(oke);
            }
            else
            {
                MessageBox.Show("Je Hebt nog geen geheim woord");
                btnHint.IsEnabled = false;
            }

        }

        // Menu item click event voor het volledige spel af te sluiten
        private void MnuAfsluiten(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Het laten zien van de hoogste scores je kan deze in de game zien de namen van de huidige spelers staan er al tussen 
        // maar als het spel niet word gespeeld verdijwnen deze van de high scores
        private void MnuHighscores(object sender, RoutedEventArgs e)
        {
            Menu.HIscores();
        }

        // Terug naar het hoofmenu voor het starten van een volledig nieuw spel de huidige spelers en punten worden gewist
        private void MnuNieuwSpel(object sender, RoutedEventArgs e)
        {
            this.Hide();
            menuGame.DeletePlayers();
            GameEnd();
            rounds = 1;
            Menu.isActive1 = false;
            Menu.isActive = false;
            menuGame.Show();
        }
        //Info menu voor de spelregels
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Show();
        }
        //Hint vragen via menu item
        private void MnuHint(object sender, RoutedEventArgs e)
        {
            HintVragen();
        }

        private void MnuTimer(object sender, RoutedEventArgs e)
        {
            Menu.SetTimerGame();
        }



        // Als je een foute letter voor meerdere keren raad krijg je een melding dat je deze letter al hebt geraden
        private void CheckFouteLetters(string userInput) // controlen als je dezelfde letter dubben raad
        {
            if (fouteLetters != null)
            {
                if (fouteLetters.Contains(userInput))
                {
                    MessageBox.Show("Deze letter heb je al geraden!!");
                    letterGevonden = true;
                }

            }


        }
    
    }
}
