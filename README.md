# CBriscolaUWP for programers
La cbriscola con GUI in UWP, ossia Project Carmela

Se il sistema pare non prendere il click sulla carta, fare di nuovo click sulla stessa carta, non selezionare una carta diversa, se no si falsa il gioco.

[![youtube](https://i.ibb.co/qkbNyzb/mq2.jpg)](https://www.youtube.com/watch?v=BG12__cPoGg)

Questo vuole essere una mod del project carmela, che si deve evolvere su pc col multiplayer a la tetrinet, fatta%localappdata% delle aziende.

Se volete contribuire siete i benvenuti

# Novità
- E' stato aggiunto un controllo per evitare le lotte per avere l'ultima carta se disegna un seme di briscola alto.
Se si hanno problemi nel salvare l'opzione, eliminare il file in %localappdata%\Packages\21961GiulioSorrentino.CBriscola_gzdzm2aja2gcm\LocalState
- Sono stati aggiunti due pulsanti nella schermata delle opzioni: uno per caricare le opzioni da file json ed uno per salvarle.
Il pulsante di salvataggio verrà visualizzato solo dopo il pulsante di carica e per salvare le opzioni è necesario prima validarle.

# Come installare

[![microsoft](https://get.microsoft.com/images/en-us%20dark.svg)](https://www.microsoft.com/store/apps/9NGV8ZD2HN70)


# The old fashion compiled way

Installa visual studio 2022, scarica il progetto usando git selezionando download from existing git repository e quindi compilalo.

# Come provarlo su xbox.
Andate su https://developer.microsoft.com/it-it/microsoft-store/register/ e registrate il vostro account hotmail (possibilmente non quello principale, ma createne uno  nuovo) come developer.
Pagate i 17 euro, non si capisce se una tantum oppure annui, ed aspettate l'attivazione, dopodiché aprite sulla vostra SECONDA (non di gioco) xbox one o series x l'app store e scaricate l'app dev kit (quella con le icone della series s/x, non quella con l'icona della one), apritela e seguite le istruzioni a video, facendo attenzione a quando vi dice "aprite windows 10 aprite, aprite aka.ms/quello_che_è e registrate la xbox), ad aprire da pc nel vostro account developer microsoft la sezione "xbox one development consoles", premere il tasto "+" in alto a destra, selezionare "enter activation code" ed indicare il codice mostrato a video; quando l'impostazione è finita la console viene riavviata e si entra nel developer mode.

Per prima cosa registrate il vostro account di developer nell'apposita sezione in alto a destra, dopo cliccate su "show visual studio pin" e salvatevelo, ora pasate al pc, aprite il progetto, cliccate su dispositivo remoto invece che su computer locale quando dovete compilare, indicate l'ip della xbox, aspettate che vi chiede il pin, inseritelo e compilate.

A breve nella sezione bassa centrale comparirà la cbriscola, basta avviarla per poter giocare.

# Donazioni

[![paypal](https://www.paypalobjects.com/it_IT/IT/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=H4ZHTFRCETWXG)

Puoi donare anche tramite carta Hype a patto di avere il mio numero di cellulare nella rubrica. Sai dove lo puoi trovare? Sul mio curriculum.
Apri l'app Hype, fai il login, seleziona PAGAMENTI, INVIA DENARO, seleziona il mio numero nella rubrica, imposta l'importo, INSERISCI LA CAUSALE e segui le istruzioni a video.

# Bibliografia
https://docs.microsoft.com/it-it/windows/uwp/threading-async/use-a-timer-to-submit-a-work-item
