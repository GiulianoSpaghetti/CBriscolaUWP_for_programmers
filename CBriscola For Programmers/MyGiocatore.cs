/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 1.1.3
 *
 *  Created by Giulio Sorrentino (numerone) on 29/01/23.
 *  Copyright 2023 Some rights reserved.
 *
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace org.altervista.numerone.framework
{
  public partial class MyGiocatore: Giocatore
	{
        private List<UInt16> punteggi;
		public MyGiocatore(GiocatoreHelper h, string n, UInt16 carte, bool ordina = true) : base(h, n, carte)
		{
            punteggi = new List<UInt16>();
        }

        public void CancellaPunteggi(GiocatoreHelper h=null)
        {
            punteggi.Clear();
            numeroCarte = dimensioneMano;
			if (h!=null)
	            helper = h;
            iCartaGiocata = (UInt16)(Carta_GIOCATA.NESSUNA_Carta_GIOCATA);
            iCarta = 0;
			punteggio = 0;
            for (UInt16 i = 0; i < dimensioneMano; i++)
                mano[i] = null;
        }

        public void Resetta(GiocatoreHelper h = null, bool resettaPunteggi = true)
        {
            numeroCarte = dimensioneMano;
            if (h != null)
                helper = h;
            iCartaGiocata = (UInt16)(Carta_GIOCATA.NESSUNA_Carta_GIOCATA);
            iCarta = 0;
            for (UInt16 i = 0; i < dimensioneMano; i++)
                mano[i] = null;
            if (resettaPunteggi)
                punteggi.Clear();
            else if (GetPunteggio() > 0)
                punteggi.Add(GetPunteggio());
            punteggio = 0;
        }

        public UInt64 GetPunteggi()
        {
            UInt64 p = 0;
            foreach (UInt16 i in punteggi)
                p += i;
            return p;
        }
    }

}