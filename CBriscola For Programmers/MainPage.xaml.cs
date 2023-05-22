using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.Foundation;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using org.altervista.numerone.framework;
// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace CBriscola_For_Programmers
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static Giocatore g, cpu, primo, secondo, temp;
        private static Mazzo m;
        private static Carta c, c1, briscola;
        private static BitmapImage cartaCpu = new BitmapImage(new Uri("ms-appx:///Resources/retro_carte_pc.png"));
        private static Image i, i1;
        private static bool briscolaPunti = false;
        private static bool avvisaTalloneFinito = true;
        private static UInt16 secondi = 1;
        private static TimeSpan delay;
        private static ElaboratoreCarteBriscola e;
        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private Windows.Storage.ApplicationDataContainer container;
        private ThreadPoolTimer t;
        public MainPage()
        {
            string s;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "it-IT";
            this.InitializeComponent();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += Close;
            e = new ElaboratoreCarteBriscola(briscolaPunti);
            m = new Mazzo(e);
            Carta.Inizializza(40, CartaHelperBriscola.GetIstanza(e));
            container = localSettings.CreateContainer("CBriscola", Windows.Storage.ApplicationDataCreateDisposition.Always);
            s = localSettings.Containers["CBriscola"].Values["numeUtente"] as string;
            if (s == null)
                s = "numerone";
            g = new Giocatore(new GiocatoreHelperUtente(), s, 3);
            s = localSettings.Containers["CBriscola"].Values["nomeCpu"] as string;
            if (s == null)
                s = "Cpu";
            cpu = new Giocatore(new GiocatoreHelperCpu(ElaboratoreCarteBriscola.GetCartaBriscola()), s, 3);
            primo = g;
            secondo = cpu;
            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
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
                g.AddCarta(m);
                cpu.AddCarta(m);

            }
            NomeUtente.Text = g.GetNome();
            NomeCpu.Text = cpu.GetNome();
            Utente0.Source = g.GetImmagine(0);
            Utente1.Source = g.GetImmagine(1);
            Utente2.Source = g.GetImmagine(2);
            Cpu0.Source = cartaCpu;
            Cpu1.Source = cartaCpu;
            Cpu2.Source = cartaCpu;
            PuntiCpu.Text = $"Punti di {cpu.GetNome()}: {cpu.GetPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.GetNome()}: {g.GetPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.GetNumeroCarte()} carte";
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.GetSemeStr()}";
            Briscola.Source = briscola.GetImmagine();
        }

        private Image GiocaUtente(Image img)
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
            g.Gioca(quale);
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
            txtNomeUtente.Text = g.GetNome();
            txtNomeCpu.Text = cpu.GetNome();
            txtSecondi.Text = "" + secondi;
            cbBriscolaDaPunti.IsChecked = briscolaPunti;
            cbAvvisaTallone.IsChecked = avvisaTalloneFinito;
            Applicazione.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Collapsed;
            GOpzioni.Visibility = Visibility.Visible;


        }

        private Image GiocaCpu()
        {
            UInt16 quale = 0;
            Image img1 = Cpu0;
            if (primo == cpu)
                cpu.Gioca(0);
            else
                cpu.Gioca(0, g);
            quale = cpu.GetICartaGiocata();
            if (quale == 1)
                img1 = Cpu1;
            if (quale == 2)
                img1 = Cpu2;
            Giocata1.Visibility = Visibility.Visible;
            Giocata1.Source = cpu.GetCartaGiocata().GetImmagine();
            img1.Visibility = Visibility.Collapsed;
            return img1;
        }
        private static bool AggiungiCarte()
        {
            try
            {
                primo.AddCarta(m);
                secondo.AddCarta(m);
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
            i = GiocaUtente(img);
            if (secondo == cpu)
                i1 = GiocaCpu();
            t = ThreadPoolTimer.CreateTimer((source) =>
            {

                IAsyncAction asyncAction = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {

                    c = primo.GetCartaGiocata();
                    c1 = secondo.GetCartaGiocata();
                    if ((c.CompareTo(c1) > 0 && c.StessoSeme(c1)) || (c1.StessoSeme(briscola) && !c.StessoSeme(briscola)))
                    {
                        temp = secondo;
                        secondo = primo;
                        primo = temp;
                    }

                    primo.AggiornaPunteggio(secondo);
                    PuntiCpu.Text = $"Punti di {cpu.GetNome()}: {cpu.GetPunteggio()}";
                    PuntiUtente.Text = $"Punti di {g.GetNome()}: {g.GetPunteggio()}";
                    if (AggiungiCarte())
                    {
                        NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.GetNumeroCarte()} carte";
                        CartaBriscola.Text = $"Il seme di Briscola è: {briscola.GetSemeStr()}";
                        if (Briscola.Visibility == Visibility.Visible && m.GetNumeroCarte() == 0)
                        {
                            NelMazzoRimangono.Visibility = Visibility.Collapsed;
                            Briscola.Visibility = Visibility.Collapsed;
                            if (avvisaTalloneFinito)
                                new ToastContentBuilder().AddArgument("Tallone Finito").AddText($"Il tallone è finito").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();

                        }
                        Utente0.Source = g.GetImmagine(0);
                        if (cpu.GetNumeroCarte() > 1)
                            Utente1.Source = g.GetImmagine(1);
                        if (cpu.GetNumeroCarte() > 2)
                            Utente2.Source = g.GetImmagine(2);
                        i.Visibility = Visibility.Visible;
                        i1.Visibility = Visibility.Visible;
                        Giocata0.Visibility = Visibility.Collapsed;
                        Giocata1.Visibility = Visibility.Collapsed;
                        if (cpu.GetNumeroCarte() == 2)
                        {
                            Utente2.Visibility = Visibility.Collapsed;
                            Cpu2.Visibility = Visibility.Collapsed;
                        }
                        if (cpu.GetNumeroCarte() == 1)
                        {
                            Utente1.Visibility = Visibility.Collapsed;
                            Cpu1.Visibility = Visibility.Collapsed;
                        }
                        if (primo == cpu)
                        {
                            i1 = GiocaCpu();
                            if (cpu.GetCartaGiocata().StessoSeme(briscola))
                                new ToastContentBuilder().AddArgument("Giocata Briscola").AddText($"La cpu ha giocato il {cpu.GetCartaGiocata().GetValore()+1} di briscola").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                            else if (cpu.GetCartaGiocata().GetPunteggio()>0)
                                new ToastContentBuilder().AddArgument("Giocata Carta di valore").AddText($"La cpu ha giocato il {cpu.GetCartaGiocata().GetValore() + 1} di {cpu.GetCartaGiocata().GetSemeStr()}").AddAudio(new Uri("ms-winsoundevent:Notification.Reminder")).Show();
                        };

                    }
                    else
                    {
                        string s;
                        Applicazione.Visibility = Visibility.Collapsed;
                        if (g.GetPunteggio() == cpu.GetPunteggio())
                            s = "La partita è patta";
                        else
                        {
                            if (g.GetPunteggio() > cpu.GetPunteggio())
                                s = "Hai vinto per";
                            else
                                s = "Hai perso per";
                            s = $"{s} {Math.Abs(g.GetPunteggio() - cpu.GetPunteggio())} punti. Vuoi effertuare una nuova partita?";
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
            localSettings.Containers["CBriscola"].Values["nomeCpu"] = txtNomeCpu.Text;
            localSettings.Containers["CBriscola"].Values["secondi"] = $"{secondi}";
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

            g.SetNome(txtNomeUtente.Text);
            cpu.SetNome(txtNomeCpu.Text);
            NomeUtente.Text = g.GetNome();
            NomeCpu.Text = cpu.GetNome();
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
            e = new ElaboratoreCarteBriscola(briscolaPunti);
            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
            m = new Mazzo(e);
            g = new Giocatore(new GiocatoreHelperUtente(), g.GetNome(), 3);
            cpu = new Giocatore(new GiocatoreHelperCpu(ElaboratoreCarteBriscola.GetCartaBriscola()), cpu.GetNome(), 3);
            for (UInt16 i = 0; i < 3; i++)
            {
                g.AddCarta(m);
                cpu.AddCarta(m);

            }
            Utente0.Source = g.GetImmagine(0);
            Utente0.Visibility = Visibility.Visible;
            Utente1.Source = g.GetImmagine(1);
            Utente1.Visibility = Visibility.Visible;
            Utente2.Source = g.GetImmagine(2);
            Utente2.Visibility = Visibility.Visible;
            Cpu0.Source = cartaCpu;
            Cpu0.Visibility = Visibility.Visible;
            Cpu1.Source = cartaCpu;
            Cpu1.Visibility = Visibility.Visible;
            Cpu2.Source = cartaCpu;
            Cpu2.Visibility = Visibility.Visible;
            Giocata0.Visibility = Visibility.Collapsed;
            Giocata1.Visibility = Visibility.Collapsed;
            PuntiCpu.Text = $"Punti di {cpu.GetNome()}: {cpu.GetPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.GetNome()}: {g.GetPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.GetNumeroCarte()} carte";
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.GetSemeStr()}";
            PuntiCpu.Text = $"Punti di {cpu.GetNome()}: ${cpu.GetPunteggio()}";
            PuntiUtente.Text = $"Punti di {g.GetNome()}: {g.GetPunteggio()}";
            NelMazzoRimangono.Text = $"Nel mazzo rimangono: {m.GetNumeroCarte()} carte";
            NelMazzoRimangono.Visibility = Visibility.Visible;
            CartaBriscola.Text = $"Il seme di Briscola è: {briscola.GetSemeStr()}";
            CartaBriscola.Visibility = Visibility.Visible;
            Briscola.Source = briscola.GetImmagine();
            Briscola.Visibility = Visibility.Visible;

            if (primoUtente)
            {
                secondo = g;
                primo = cpu;
                i1 = GiocaCpu();
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
            await Launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola%20for%20Programmers%20la%20partita%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{g.GetPunteggio()}%20a%20{cpu.GetPunteggio()}%20su%20piattaforma%20{App.piattaforma}%20col%20mazzo%20Napoletano&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscolauwp_for_programmers"));
        }

        public void Close(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            container.Dispose();
        }

        private async void OnSito_Click(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/numerunix/cbriscolauwp.new"));
        }

    }
}
