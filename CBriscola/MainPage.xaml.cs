using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace CBriscola
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static giocatore g;
        private static giocatore cpu;
        private static giocatore primo;
        private static giocatore secondo;
        private static mazzo m;
        private static carta c, c1, briscola;
        private static giocatore temp;
        private static BitmapImage cartaCpu = new BitmapImage(new Uri("ms-appx:///Resources/retro_carte_pc.png"));
        private static Image i, i1;
        private static bool enableClick = true;
        private static UInt16 secondi = 1;
        private static TimeSpan delay;
        private static elaboratoreCarteBriscola e;
        public MainPage()
        {
            this.InitializeComponent();
            e = new elaboratoreCarteBriscola();
            m = new mazzo(e);
            carta.inizializza(40, cartaHelperBriscola.getIstanza(e));
            g = new giocatore(new giocatoreHelperUtente(), "numerone", 3);
            cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), "Francesca", 3);
            primo = g;
            secondo = cpu;
            briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
            delay = TimeSpan.FromSeconds(secondi);
            Image[] img = new Image[3];
            for (UInt16 i = 0; i < 3; i++)
            {
                g.addCarta(m);
                cpu.addCarta(m);

            }
            NomeUtente.Text = g.getNome();
            NomeCpu.Text = cpu.getNome();
            Utente0.Source = g.getImmagine(0);
            Utente1.Source = g.getImmagine(1);
            Utente2.Source = g.getImmagine(2);
            Cpu0.Source = cartaCpu;
            Cpu1.Source = cartaCpu;
            Cpu2.Source = cartaCpu;
            PuntiCpu.Text = "Punti di " + cpu.getNome() + ": " + cpu.getPunteggio();
            PuntiUtente.Text = "Punti di " + g.getNome() + ": " + g.getPunteggio();
            NelMazzoRimangono.Text = "Nel mazzo rimangono: "+ m.getNumeroCarte() + " carte";
            CartaBriscola.Text = "Il seme di Briscola è: " + briscola.getSemeStr();
            Briscola.Source = briscola.getImmagine();
        }
        private Image giocaUtente(Image img)
        {
            UInt16 quale = 0;
            Image img1 = Utente0;
            if (img == Utente1)
            {
                quale = 1;
                img1 = Utente1;
            }
            if (img == Utente2)
            {
                quale = 2;
                img1 = Utente2;
            }
            Giocata0.Visibility = Visibility.Visible;
            Giocata0.Source = img1.Source;
            img1.Visibility = Visibility.Collapsed;
            g.gioca(quale);
            return img1;
        }


        private void OnApp_Click(object sender, TappedRoutedEventArgs e)
        {
            Info.Visibility = Visibility.Collapsed;
            GOpzioni.Visibility = Visibility.Collapsed;
            Applicazione.Visibility = Visibility.Visible;
        }

        private void OnInfo_Click(object sender, TappedRoutedEventArgs e)
        {
            GOpzioni.Visibility = Visibility.Collapsed;
            Applicazione.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Visible;

        }
        private void OnOpzioni_Click(object sender, TappedRoutedEventArgs e)
        {
            txtNomeUtente.Text = g.getNome();
            txtNomeCpu.Text = cpu.getNome();
            txtSecondi.Text = "" + secondi;
            Applicazione.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Collapsed;
            GOpzioni.Visibility = Visibility.Visible;


        }

        private Image giocaCpu()
        {
            UInt16 quale = 0;
            Image img1 = Cpu0;
            if (primo == cpu)
                cpu.gioca(0);
            else
                cpu.gioca(0, g);
            quale = cpu.getICartaGiocata();
            if (quale == 1)
                img1 = Cpu1;
            if (quale == 2)
                img1 = Cpu2;
            Giocata1.Visibility = Visibility.Visible;
            Giocata1.Source = cpu.getCartaGiocata().getImmagine();
            img1.Visibility = Visibility.Collapsed;
            return img1;
        }
        private static bool aggiungiCarte()
        {
            try
            {
                primo.addCarta(m);
                secondo.addCarta(m);
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            return true;
        }

        private void Image_Tapped(object Sender, TappedRoutedEventArgs arg)
        {
            if (!enableClick)
                return;
            enableClick = false;
            Image img = (Image)Sender;
            i = giocaUtente(img);
            if (secondo == cpu)
                i1 = giocaCpu();
            ThreadPoolTimer t = ThreadPoolTimer.CreateTimer((source) =>
            {

                IAsyncAction asyncAction = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {

                    c = primo.getCartaGiocata();
                    c1 = secondo.getCartaGiocata();
                    if ((c.CompareTo(c1) > 0 && c.stessoSeme(c1)) || (c1.stessoSeme(briscola) && !c.stessoSeme(briscola)))
                    {
                        temp = secondo;
                        secondo = primo;
                        primo = temp;
                    }

                    primo.aggiornaPunteggio(secondo);
                    PuntiCpu.Text = "Punti di " + cpu.getNome() + ": " + cpu.getPunteggio();
                    PuntiUtente.Text = "Punti di: " + g.getPunteggio();
                    if (aggiungiCarte())
                    {
                        NelMazzoRimangono.Text = "Nelì mazzo rimangono "+ m.getNumeroCarte() + " carte";
                        CartaBriscola.Text = "Il seme di Briscola è: " + briscola.getSemeStr();
                        if (Briscola.Visibility == Visibility.Visible && m.getNumeroCarte() == 0)
                        {
                            NelMazzoRimangono.Visibility = Visibility.Collapsed;
                            Briscola.Visibility = Visibility.Collapsed;
                        }
                        Utente0.Source = g.getImmagine(0);
                        if (cpu.getNumeroCarte() > 1)
                            Utente1.Source = g.getImmagine(1);
                        if (cpu.getNumeroCarte() > 2)
                            Utente2.Source = g.getImmagine(2);
                        i.Visibility = Visibility.Visible;
                        i1.Visibility = Visibility.Visible;
                        Giocata0.Visibility = Visibility.Collapsed;
                        Giocata1.Visibility = Visibility.Collapsed;
                        if (cpu.getNumeroCarte() == 2)
                        {
                            Utente2.Visibility = Visibility.Collapsed;
                            Cpu2.Visibility = Visibility.Collapsed;
                        }
                        if (cpu.getNumeroCarte() == 1)
                        {
                            Utente1.Visibility = Visibility.Collapsed;
                            Cpu1.Visibility = Visibility.Collapsed;
                        }
                        if (primo == cpu)
                        {
                            i1 = giocaCpu();
                        };

                    }
                    else
                    {
                        string s;
                        Applicazione.Visibility = Visibility.Collapsed;
                        if (g.getPunteggio() == cpu.getPunteggio())
                            s = "La partita è patta";
                        else
                        {
                            if (g.getPunteggio() > cpu.getPunteggio())
                                s = "Hai vinto per";
                            else
                                s = "Hai perso per";
                            s = s + " " + Math.Abs(g.getPunteggio() - cpu.getPunteggio()) + " punti. Vuoi effertuare una nuova partita?";
                        }
                        risultato.Text = "La partita è finita. " + s;
                        Greetings.Visibility = Visibility.Visible;
                    }
                });
            }, delay);
            enableClick = true;
        }
        private void OnOpOk_Click(object sender, TappedRoutedEventArgs e)
        {
            g.setNome(txtNomeUtente.Text);
            cpu.setNome(txtNomeCpu.Text);
            NomeUtente.Text = g.getNome();
            NomeCpu.Text = cpu.getNome();
            secondi = UInt16.Parse(txtSecondi.Text);
            delay = TimeSpan.FromSeconds(secondi);
            GOpzioni.Visibility = Visibility.Collapsed;
            Applicazione.Visibility = Visibility.Visible;
        }
        private void OnFpOk_Click(object sender, TappedRoutedEventArgs evt)
        {
            bool primoUtente = primo == g;
            Greetings.Visibility = Visibility.Collapsed;
            e = new elaboratoreCarteBriscola();
            briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
            m = new mazzo(e);
            g = new giocatore(new giocatoreHelperUtente(), g.getNome(), 3);
            cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), cpu.getNome(), 3);
            for (UInt16 i = 0; i < 3; i++)
            {
                g.addCarta(m);
                cpu.addCarta(m);

            }
            Utente0.Source = g.getImmagine(0);
            Utente0.Visibility = Visibility.Visible;
            Utente1.Source = g.getImmagine(1);
            Utente1.Visibility = Visibility.Visible;
            Utente2.Source = g.getImmagine(2);
            Utente2.Visibility = Visibility.Visible;
            Cpu0.Source = cartaCpu;
            Cpu0.Visibility = Visibility.Visible;
            Cpu1.Source = cartaCpu;
            Cpu1.Visibility = Visibility.Visible;
            Cpu2.Source = cartaCpu;
            Cpu2.Visibility = Visibility.Visible;
            Giocata0.Visibility = Visibility.Collapsed;
            Giocata1.Visibility = Visibility.Collapsed;
            PuntiCpu.Text = "Punti di " + cpu.getNome() + ": " + cpu.getPunteggio();
            PuntiUtente.Text = "Punti di " + g.getNome() + ": " + g.getPunteggio();
            NelMazzoRimangono.Text = "Nel mazzo rimangono " + m.getNumeroCarte() + " carte";
            NelMazzoRimangono.Visibility = Visibility.Visible;
            CartaBriscola.Text = "Il seme di Briscola è: " + briscola.getSemeStr();
            CartaBriscola.Visibility = Visibility.Visible;
            Briscola.Source = briscola.getImmagine();
            Briscola.Visibility = Visibility.Visible;

            if (primoUtente)
            {
                secondo = g;
                primo = cpu;
                i1 = giocaCpu();
            }
            else
            {
                primo = g;
                secondo = cpu;
            }
            Applicazione.Visibility = Visibility.Visible;
        }

        private void OnFpCancel_Click(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }

    }
}
