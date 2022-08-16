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
	interface giocatoreHelper
	{
		abstract UInt16 gioca(carta[] v, UInt16 numeroCarte);
		abstract UInt16 gioca(carta[] v, UInt16 numeroCarte, carta c);
		abstract void aggiornaPunteggio(ref UInt16 punteggio, carta c, carta c1);
	};
}