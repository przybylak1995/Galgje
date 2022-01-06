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
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace Galgje4._0
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        bool geenDubbeleNamen = true;
        public static List<Players> players = new List<Players>();
        public static int dynalischeTimer = 10;
        public static bool isActive = false; public static bool isActive1 = false;
        public static string player1; public static string player2;
        string show = "";
       
        public Menu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //afsluiten van de volledige app
        }
        // Bij het starten van het spel word gekeken welke modus gekozen is 
        // vervolgens word het spel gestart met de vraag van de ingaven van de speler zijn naam of namen 
        // het spel start niet zolang er een naam is ingegeven 
        // Een naam kan niet 2 keer dezelfde zijn op de high score 
        private void btnPLay_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
             geenDubbeleNamen = true;

            if (isActive == false && isActive1 == false)
            {
                MessageBox.Show("Kies het aantal spelers");

            }
            else if (isActive == true && isActive1 == false) // dubbele speler 
            {

                player1 = Interaction.InputBox("Geef naam player 1", "Player", "Player1");                
                player2 = Interaction.InputBox("Geef naam player 2", "Player", "Player2");
                ControleerOPNaam(player1, player2);
                if (player1 != string.Empty && player2 != string.Empty && geenDubbeleNamen == true)
                {
                    PlayersAddPlayers(player1);
                    PlayersAddPlayers(player2);
                    this.Hide();
                    window.lblInfo.Content = $"{player2} geef een \ngeheim woord in:";
                    window.Show();
                }
             
            }
            else if (isActive1 == true && isActive == false) // enkele speler
            {
                player1 = Interaction.InputBox("Geef naam player 1", "Player", "Player1");
                ControleerOPNaam(player1);
                if (player1 != string.Empty && geenDubbeleNamen == true)
                {
                    PlayersAddPlayers(player1);
                    this.Hide();
                    window.Show();
                }
            }
            window.lblSpelerAanZet.Content = $"{player1}";
        }
        // controleren als de naam van de speler al bestaat in de list
        private void ControleerOPNaam (string pl1 , string pl2 = null) 
        {
            foreach (var item in players)
            {
                if(item.Naam == pl1 || item.Naam == pl2 )
                {
                    MessageBox.Show("Deze naam bestaat al ");
                     geenDubbeleNamen =  false;
                    break;
                }
                
            }

        }
        // wissen van de spelers die het spel vroegtijdig verlaten door terug naar het menu te gaan.
        public void DeletePlayers ()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].Naam == player1 )
                {
                    players.RemoveAt(i);
                }
                if (isActive == true)
                {
                    if(players[i].Naam == player2)
                    {
                        players.RemoveAt(i);
                    }
                }


            }

        }
        // het toevoegen van een nieuwe speler aan de list
        private void PlayersAddPlayers(string ppl) // Toevoegen van de spelers
        {
            Players player = new Players();
            player.Naam = ppl;
            players.Add(player);
        }
        // Instellen van de dynamische timer
        private void btnSetTimer_Click(object sender, RoutedEventArgs e)
        {
            bool fouteIngaven = false;
            do
            {
                fouteIngaven = false;
                string input = Interaction.InputBox("Gewenste tijd van de timer", "Timer Setup", "10");
                bool check = int.TryParse(input, out dynalischeTimer);
                if (check == false || dynalischeTimer < 5 || dynalischeTimer > 20)
                {
                    fouteIngaven = true;
                    MessageBox.Show("Geef een nummer tussen 5 en 20");
                }
               
            } while (fouteIngaven == true);

           
        }
        // De keuze knoppen voor enkel spel
        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            btnDoublePLayer.Background = new SolidColorBrush(Colors.Transparent);
            if (isActive1 != true && isActive != true)
            {
                isActive1 = true;
                btnSinglePlayer.Background = new SolidColorBrush(Colors.Green);
            }
            else if (isActive1 == true)
            {
                isActive1 = false;
                btnSinglePlayer.Background = new SolidColorBrush(Colors.Transparent);
            }

        }
        // De keuze knoppen voor dubbel spel
        private void btnDoublePLayer_Click(object sender, RoutedEventArgs e)
        {
            isActive1 = false;
            btnSinglePlayer.Background = new SolidColorBrush(Colors.Transparent);

            if (isActive != true && isActive1 != true)
            {
                isActive = true;
                btnDoublePLayer.Background = new SolidColorBrush(Colors.Green);
            }
            else if (isActive == true)
            {
                isActive = false;
                btnDoublePLayer.Background = new SolidColorBrush(Colors.Transparent);
            }

        }
        
        public static void AddLevensVerloren () // het toevoegen van al de data van de speler 
        {

            if (isActive1 != true)
            {
                if (MainWindow.rounds % 2 == 0)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if(players[i].Naam == player2 )
                        {
                            players[i].Tijd = DateTime.Now.ToString("HH:mm:ss"); // tijd wanneer speler gespeeld heeft
                            players[i].LevensVerloren = MainWindow.levenVerloren; // hoeveel levens er verloren zijn
                            if(MainWindow.hintGebruikt == true) // als de speler wel of geen hint gebruikt heeft 
                            {
                                players[i].Hint = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].Naam == player1 )
                        {
                            players[i].Tijd = DateTime.Now.ToString("HH:mm:ss");
                            players[i].LevensVerloren = MainWindow.levenVerloren;
                            if (MainWindow.hintGebruikt == true)
                            {
                                players[i].Hint = true;
                            }
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].Naam == player1 )
                    {
                        if(players[i].LevensVerloren < MainWindow.levenVerloren && players[i].LevensVerloren != 0)
                        {
                            players[i].LevensVerloren = players[i].LevensVerloren;
                        }
                        else
                        {
                            players[i].LevensVerloren = MainWindow.levenVerloren;
                        }


                        players[i].Tijd = DateTime.Now.ToString("HH:mm:ss");
                       
                        if (MainWindow.hintGebruikt == true)
                        {
                            players[i].Hint = true;
                        }
                    }
                }
            }


        }
        private void btnHighScores_Click(object sender, RoutedEventArgs e) // Het laten zien van de high scores
        {

            List<Players> order = players.OrderBy(o => o.LevensVerloren).ToList(); // ordernen van de players list op basis van hoeveel levens er verloren zijn.
            show = "";
            int top5 = 1;
            foreach (var item in order)
            {
                if(item.Hint == false && top5 < 6 )      // lest mag alleen de spelers laten zien die geen hint gebruikt hebben en ik wil een top 5              
                {

                    show += $"{top5}.Naam: {item.Naam} Levens Verloren: {item.LevensVerloren} Tijdstip: {item.Tijd} \n";
                    top5++;
                }
               
            }

            if(show != string.Empty)
            {
                MessageBox.Show(show);
            }
            else  // list is leeg dus nog niemand heeft gespeeld 
            {
                MessageBox.Show("Geen spelers gevonden");
            }
          
        }
    }
}
    
    



