/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using System;
namespace CBriscola
{
	class giocatoreHelperUtente : giocatoreHelper
	{
		public giocatoreHelperUtente()
		{
			;
		}
		public UInt16 gioca(carta[] v, UInt16 numeroCarte)
		{
			UInt16 c;
			bool ok;
			for (UInt16 i = 0; i < numeroCarte; i++)
				Console.Write($"{v[i]} ");
			Console.WriteLine();
			Console.Write($"{Program.mgr.GetString("CartaDaGiocare")}: ");
			do
			{
				try { c = System.Convert.ToUInt16(Console.ReadLine()); }
				catch (FormatException e) { Console.WriteLine($"{Program.mgr.GetString("ValoreNonValido")}."); c = (UInt16)(numeroCarte + 1); }
				c--;
				if (!(ok = c < numeroCarte))
					Console.Write($"{Program.mgr.GetString("InserisciNuovoNumero")} { numeroCarte}: ");
			} while (!ok);
			return c;
		}
		public UInt16 gioca(carta[] v, UInt16 numeroCarte, carta c)
		{
			return gioca(v, numeroCarte);
		}
		public void aggiornaPunteggio(ref UInt16 punteggioAttuale, carta c, carta c1)
		{
			punteggioAttuale = (UInt16)(punteggioAttuale + c.getPunteggio() + c1.getPunteggio());
		}
	};
}