## CBriscolaUWP_for_programmers
La cbriscola con GUI in UWP, per xbox
Questo gioco dimostra che la teoria dei giochi è vera: l'algorimo brevettato funziona su tutti i giochi di carte senza piatto.

## Come ricompilare
Per prima cosa occorre ricompilare la libreria cardframework.uwp.
Poi, una volta ottenuta la DLL, bisogna importarla nel progetto, non si può usare nuget perché è nato dopo.
Bisogna cliccare col tasto destro su riferimenti e quindi aggiungi riferimento, poi sfoglia, e selezionare la dll.


## Come funziona
Per festeggiare, vi spiego come funziona il mio algoritmo brevettato:
i punti in totale sono 120, ossia 4 assi che valgono 11 punti ciascuno, 4 3 che valgono 10 punti ciascuno, 4 10 che valgono 4 punti ciascuno, 4 9 che valgono 3 punti ciascuno, 4 8 che valgono 2 punti ciascuno.
Dal momento che la matematica non è una opinione:
4x11+4x10=84.
4x4+4x3+4x2=16+12+8=36

84+36=120 punti totali

120/2 = 60, servono 61 punti per vincere

basandosi solo sui carichi si rischia di perdere, perché

84-61=23, bisogna prenderli quasi tutti e lasciare solo 23 punti di carichi

60-36=24, prendendo tutte le altre carte bastano solo 3 carichi per vincere.

Per cui non metto i livelli, ma vi lascio imparare la teoria delle carte a lungo, da me inventata a 18 anni, con la wxbriscola, che mi ha portato l'amore di Francesca. 
## Come installare

## Come aprire un account da sviluppatore xbox

Andate su https://developer.microsoft.com/it-it/microsoft-store/register/ e registrate il vostro account hotmail (possibilmente non quello principale, ma createne uno nuovo) come developer. Pagate i 17 euro una tantum, ed aspettate l'attivazione, dopodiché aprite sulla vostra SECONDA (non di gioco) xbox one o series x l'app store e scaricate l'app dev kit (quella con le icone della series s/x, non quella con l'icona della one), apritela e seguite le istruzioni a video, facendo attenzione a quando vi dice "aprite windows 10 aprite, aprite aka.ms/quello_che_è e registrate la xbox), ad aprire da pc nel vostro account developer microsoft la sezione "xbox one development consoles", premere il tasto "+" in alto a destra, selezionare "enter activation code" ed indicare il codice mostrato a video; quando l'impostazione è finita la console viene riavviata e si entra nel developer mode.

## The new fashion way

Aprite il devhome della vostra xbox one, nella sezione remote access configurate user e password, e poi apritela col browser.
In Home, sezione My Apps and Games, selezionare ADD, ed indicare il package principale (msixbundle) della release scelta in deploy or install application.
Le dipedenze sono gli appx.

## The old fashion way
Per prima cosa registrate il vostro account di developer nell'apposita sezione in alto a destra, dopo cliccate su "show visual studio pin" e salvatevelo, ora pasate al pc, aprite il progetto, cliccate su dispositivo remoto invece che su computer locale quando dovete compilare, indicate l'ip della xbox, aspettate che vi chiede il pin, inseritelo e compilate.

A breve nella sezione bassa centrale comparirà il programma, basta avviarla per poter giocare.


## Come installarlo sul surface

Il package è sia per arm64 che per amd64, è sufficiente registrare la chiave nel sistema, non nell'account utente, nella sezione persone attendibili per installare l'msixbundle ed ottenere l'app.
Può essere usata per creare chioschi.


## Screenshot dell'app

## Donazione

http://numerone.altervista.org/donazioni.php
