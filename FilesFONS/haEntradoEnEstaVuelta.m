function res = haEntradoEnEstaVuelta(fila,pitstops,vActual,maxPits)
   % recibe un piloto, la vuelta actual y la matriz de pitstops. Devuelve un verdadero si el piloto recibido ha entrado en boxes en esta vuelta
   res=0;
   for indice=0:maxPits-1
       if pitstops(fila,indice*4+1)==vActual
           res=1;
       end
   end
end