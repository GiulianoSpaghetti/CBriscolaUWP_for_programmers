using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Search;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core.Preview;
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
        private static giocatore g, cpu, primo, secondo, temp;
        private static mazzo m;
        private static carta c, c1, briscola;
        private static BitmapImage cartaCpu = new BitmapImage(new Uri("ms-appx:///Resources/retro_carte_pc.png"));
        private static Image i, i1;
        private static bool briscolaPunti = false;
        private static bool avvisaTalloneFinito = true;
        private static UInt16 secondi = 1;
        private static TimeSpan delay;
        private static elaboratoreCarteBriscola e;
        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private Windows.Storage.ApplicationDataContainer container;
        public MainPage()
        {
            string s;
            this.InitializeComponent();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += Close;
            e = new elaboratoreCarteBriscola(briscolaPunti);
            m = new mazzo(e);
            carta.inizializza(40, cartaHelperBriscola.getIstanza());
            container = localSettings.CreateContainer("CBriscola", Windows.Storage.ApplicationDataCreateDisposition.Always);
            s = localSettings.Containers["CBriscola"].Values["numeUtente"] as string;
            if (s == null)
                s = "numerone";
            g = new giocatore(new giocatoreHelperUtente(), s, 3);
            s = localSettings.Containers["CBriscola"].Values["nomeCpu"] as string;
            if (s == null)
                s = "Cpu";
            cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), s, 3);
            primo = g;
            secondo = cpu;
            briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
            s=localSettings.Containers["CBriscola"].Values["secondi"] as string;
            try
            {
                secondi = UInt16.Parse(s);
            } catch (Exception ex)
            {
                secondi = 5;
            }
            delay = TimeSpan.FromSeconds(secondi);
            s = localSettings.Containers["CBriscola"].Values["briscolaDaPunti"] as string;
            if (s == null || s == "false")
                briscolaPunti = false;
            else
                briscolaPunti = true;
            s = localSettings.Containers["CBriscola"].Values["avvisaTalloneFinito"] as string;
            if (s == null || s == "false")
                avvisaTalloneFinito = false;
            else
                avvisaTalloneFinito = true;
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
            PuntiCpu.Text = $"Punti di {cpu.getNome()}: {cpu.getPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.getNome()}: {g.getPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.getNumeroCarte()} carte";
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.getSemeStr()}";
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

        private void OnEliminaOpzioni_Click(object sender, TappedRoutedEventArgs e)
        {
            localSettings.Containers["CBriscola"].DeleteContainer("CBriscola");
            container = localSettings.CreateContainer("CBriscola", Windows.Storage.ApplicationDataCreateDisposition.Always);

        }

        private void OnOpzioni_Click(object sender, TappedRoutedEventArgs e)

        {
            txtNomeUtente.Text = g.getNome();
            txtNomeCpu.Text = cpu.getNome();
            txtSecondi.Text = "" + secondi;
            cbBriscolaDaPunti.IsChecked = briscolaPunti;
            cbAvvisaTallone.IsChecked = avvisaTalloneFinito;
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
                    PuntiCpu.Text = $"Punti di {cpu.getNome()}: {cpu.getPunteggio()}";
                    PuntiUtente.Text = $"Punti di {g.getNome()}: {g.getPunteggio()}";
                    if (aggiungiCarte())
                    {
                        NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.getNumeroCarte()} carte";
                        CartaBriscola.Text = $"Il seme di Briscola è: {briscola.getSemeStr()}";
                        if (Briscola.Visibility == Visibility.Visible && m.getNumeroCarte() == 0)
                        {
                            NelMazzoRimangono.Visibility = Visibility.Collapsed;
                            Briscola.Visibility = Visibility.Collapsed;
                            if (avvisaTalloneFinito)
                                new ToastContentBuilder().AddArgument("Tallone Finito").AddText($"Il tallone è finito").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();

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
                            if (cpu.getCartaGiocata().stessoSeme(briscola))
                            {
                                new ToastContentBuilder().AddArgument("Giocata Briscola").AddText($"La cpu ha giocato il {cpu.getCartaGiocata().getValore()+1} di briscola").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                            } else if (cpu.getCartaGiocata().getPunteggio()>0)
                            {
                                new ToastContentBuilder().AddArgument("Giocata Carta di valore").AddText($"La cpu ha giocato il {cpu.getCartaGiocata().getValore() + 1} di {cpu.getCartaGiocata().getSemeStr()}").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                            }
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
                            s = $"{s} {Math.Abs(g.getPunteggio() - cpu.getPunteggio())} punti. Vuoi effertuare una nuova partita?";
                        }
                        risultato.Text = $"La partita è finita. {s}";
                        Greetings.Visibility = Visibility.Visible;
                    }
                });
            }, delay);
        }

        private void SalvaOpzioni()
        {
            localSettings.Containers["CBriscola"].Values["numeUtente"] = txtNomeUtente.Text;
            localSettings.Containers["CBriscola"].Values["numeCpu"] = txtNomeCpu.Text;
            localSettings.Containers["CBriscola"].Values["secondi"] = secondi;
            if (cbBriscolaDaPunti.IsChecked == null || cbBriscolaDaPunti.IsChecked == false)
            {
                briscolaPunti = false;
                localSettings.Containers["CBriscola"].Values["briscolaDaPunti"] = "false";
            }
            else
            {
                localSettings.Containers["CBriscola"].Values["briscolaDaPunti"] = "true";
                briscolaPunti = true;
            }
            if (cbAvvisaTallone.IsChecked == null || cbAvvisaTallone.IsChecked == false)
            {
                avvisaTalloneFinito = false;
                localSettings.Containers["CBriscola"].Values["avvisaTalloneFinito"] = "false";
            }
            else
            {
                avvisaTalloneFinito = true;
                localSettings.Containers["CBriscola"].Values["avvisaTalloneFinito"] = "true";
            }

        }
        private void OnOpOk_Click(object sender, TappedRoutedEventArgs e)
        {

            g.setNome(txtNomeUtente.Text);
            cpu.setNome(txtNomeCpu.Text);
            NomeUtente.Text = g.getNome();
            NomeCpu.Text = cpu.getNome();
            try
            {
                secondi = UInt16.Parse(txtSecondi.Text); 
            } catch (FormatException ex)
            {
                txtSecondi.Text = "Valore non valido";
                return;
            }
            if (cbBriscolaDaPunti.IsChecked == null || cbBriscolaDaPunti.IsChecked == false)
                briscolaPunti = false;
            else
                briscolaPunti = true;
            delay = TimeSpan.FromSeconds(secondi);
            SalvaOpzioni();
            Info.Visibility = Visibility.Collapsed;
            GOpzioni.Visibility = Visibility.Collapsed;
            Applicazione.Visibility = Visibility.Visible;
        }
        private void OnFpOk_Click(object sender, TappedRoutedEventArgs evt)
        {
            bool primoUtente = primo == g;
            Greetings.Visibility = Visibility.Collapsed;
            e = new elaboratoreCarteBriscola(briscolaPunti);
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
            PuntiCpu.Text = $"Punti di {cpu.getNome()}: {cpu.getPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.getNome()}: {g.getPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.getNumeroCarte()} carte";
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.getSemeStr()}";
            PuntiCpu.Text = $"Punti di {cpu.getNome()}: ${cpu.getPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.getNome()}: {g.getPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.getNumeroCarte()} carte";
            NelMazzoRimangono.Visibility = Visibility.Visible;
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.getSemeStr()}";
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

        private async void OnFPShare_Click(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola%20la%20partita%20{g.getNome()}%20contro%20{cpu.getNome()}%20%C3%A8%20finita%20{g.getPunteggio()}%20a%20{cpu.getPunteggio()}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscolauwp_for_programmers"));
        }

        public void Close(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            container.Dispose();
        }

    }
}
