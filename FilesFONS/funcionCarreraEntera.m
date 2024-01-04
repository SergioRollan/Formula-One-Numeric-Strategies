
load('global');

%% Comprobacion de valores

[NUMPILOTOS,c1] = size(pilotos);

% si la matriz de pilotos no tiene 5 columnas abortamos
if c1~=5
    return;
end

% si el neumatico inicial no está entre 1 y 3 es que no existe, abortamos
for index = 1:NUMPILOTOS
    if pilotos(index,4)<1 || pilotos(index,4)>3
        return;
    end
end

% si numpitstops no es un vector, abortamos
n1=size(numPitStops);
if n1(1)~=1
    return;
end

%ya las filas no nos interesan. Nos quedamos solo con las columnas. Si no hay tantas columnas como pilotos está mal. Abortamos
n1=n1(2);
if n1~=NUMPILOTOS
    return;
end

%miramos a ver quien ha hecho mas paradas
maxPits=numPitStops(1);
for index=2:n1
    if numPitStops(index)<0
        return;
    end
    if maxPits<numPitStops(index)
        maxPits=numPitStops(index);
    end
end

%si la matriz pitstops no coincide con ese numero maximo de paradas, o si no hay tantas filas como pilotos, abortamos
[f2,c2]=size(pitstops);
maxc2=maxPits*4;
if f2~=NUMPILOTOS || c2~=maxc2
    return;
end

% si la matriz de compuestos no tiene 6 columnas, abortamos
[f1,c1]=size(compuestos);
if f1~=NUMPILOTOS || c1~=6
    return;
end

% si no hay tantas banderas como vueltas, abortamos
NUMVUELTAS=size(banderas);
if NUMVUELTAS(1)~=1
    return;
end

% si no se empieza con bandera verde, abortamos
NUMVUELTAS=NUMVUELTAS(2);
if banderas(1) ~= 0
    return;
end

% si alguna bandera no esta entre 0 y 3 es que no existe, está mal, abortamos
for index=1:NUMVUELTAS
    n1=banderas(index);
    if n1<0 || n1>3
        return;
    end
end

% si la matriz de circuito no tiene exactamente 14 valores, abortamos
n1=size(circuito);
if n1~=14
    return;
end


matrizFinal=zeros(NUMPILOTOS, NUMVUELTAS+1); %matriz que usaremos para guardar los resultados y poder

matrizFinal(:,1)=pilotos(:,1); % la primera columna de la matriz final serán los dorsales
BASETIME=circuito(1);
RACEPACEDIFF=circuito(2);
GRIDPOSITION_TIMELOSS=circuito(3);
STANDINGSTART_TIMELOSS=circuito(4);
STARTING_FUEL=circuito(5);
FUEL_TIMELOSS=circuito(6);
FUEL_MASSLOSS=circuito(7);
DRS_TIMEGAIN=circuito(8);
OVERTAKING_DELTADIFF=circuito(9);
OVERTAKING_TIMELOSS=circuito(10);
OVERTAKEN_TIMELOSS=circuito(11);
DELTA_VSC_TIME=circuito(12);
PITSTOP_GREENFLAG=circuito(13);
PITSTOP_YELLOWFLAG=circuito(14);
% guardamos en variables el circuito para mayor legibilidad más tarde

BASETIME=BASETIME+RACEPACEDIFF;
OVERTAKING_DELTADIFF=OVERTAKING_DELTADIFF-DRS_TIMEGAIN;
numPitStopsHechos=zeros(1,NUMPILOTOS); % matriz con los pitstops hechos en cada vuelta
posiciones=pilotos(:,[1 3]); % la matriz posiciones tendrá los dorsales y la posicion de ese piloto
tiemposTotales=matrizFinal; % van a ser iguales pero tiemposTotales contendrá los tiempos acumulados

%% Vuelta 1

    rng('shuffle'); % la semilla de los números aleatorios
	
    %Calcular los tiempos y ponerlos
    for index = 1:NUMPILOTOS
        %Sumamos normal
        tiempoV1 = BASETIME + pilotos(index,2) + (pilotos(index,3)*GRIDPOSITION_TIMELOSS) + STANDINGSTART_TIMELOSS + STARTING_FUEL*FUEL_TIMELOSS + compuestos(index,pilotos(index,4)*2-1)*pilotos(index,5)+compuestos(index,pilotos(index,4)*2)+(rand(1)/100);
        pilotos(index,5)=pilotos(index,5)+1; % le sumamos una vuelta al neumatico
        %Miramos a ver si hay parada en boxes
        numParadasEste=numPitStopsHechos(index);
        if numParadasEste~=numPitStops(index) % si no ha hecho ya todas sus paradas
            if pitstops(index,numParadasEste*4+1) == 1 % en la vuelta actual hay una palabra
                tiempoV1=tiempoV1+PITSTOP_GREENFLAG+pitstops(index, numParadasEste*4+4);
                pilotos(index,4)=pitstops(index,numParadasEste*4+2); % cambiamos el compuesto actual por el nuevo
                pilotos(index,5)=pitstops(index,numParadasEste*4+3); % lo mismo para las vueltas del neumatico
                numPitStopsHechos(index)=numPitStopsHechos(index)+1;
            end
        end
        %Colocamos el tiempo en su sitio
        for i = 1:NUMPILOTOS
            if matrizFinal(i,1) == pilotos(index,1)
                matrizFinal(i,2)=tiempoV1;
            end
            if tiemposTotales(i,1) == pilotos(index,1)
                tiemposTotales(i,2)=tiempoV1;
            end
        end
    end
    
    %Colocar las posiciones como son
    temp=matrizFinal(:,2);
    temp=temp';
    for index=1:NUMPILOTOS
        tiempomin=temp(1);
        numfila=1;
        for i=1:NUMPILOTOS
            if tiempomin>temp(i)
                tiempomin=temp(i);
                numfila=i;
            end
        end
        posiciones(numfila,2)=index; % aqui se pone la posicion en cuestion en la matriz
        temp(numfila)=Inf;
    end
    
    FUEL_NOW = STARTING_FUEL - FUEL_MASSLOSS;
	
%% Resto de la carrera

    for vueltaActual=2:NUMVUELTAS
        banderaAhora=banderas(vueltaActual);
        %Calcular los tiempos y ponerlos
        for index = 1:NUMPILOTOS
            %Sumamos normal
			if banderaAhora==1 || (banderaAhora==2&&posiciones(index,2)==1)
                tiempoVX= DELTA_VSC_TIME;
			else
                tiempoVX=BASETIME + pilotos(index,2) + FUEL_NOW*FUEL_TIMELOSS + compuestos(index,pilotos(index,4)*2-1)*pilotos(index,5)+compuestos(index,pilotos(index,4)*2)+(rand(1)/100);
            end
            pilotos(index,5)=pilotos(index,5)+1; % le sumas una vuelta a las ruedas
            %Miramos a ver si hay parada en boxes
            numParadasEste=numPitStopsHechos(index);
            if numParadasEste~=numPitStops(index)
                if pitstops(index,numParadasEste*4+1) == vueltaActual
                    if banderaAhora==0 || banderaAhora==3
                        tiempoVX=tiempoVX+PITSTOP_GREENFLAG+pitstops(index, numParadasEste*4+4);
                    else % banderaAhora==1 || banderaAhora==2
                        tiempoVX=tiempoVX+PITSTOP_YELLOWFLAG+pitstops(index, numParadasEste*4+4);
                    end
                    pilotos(index,4)=pitstops(index,numParadasEste*4+2);
                    pilotos(index,5)=pitstops(index,numParadasEste*4+3);
                    numPitStopsHechos(index)=numPitStopsHechos(index)+1;
                end
            end
            %Colocamos el tiempo en su sitio
			
            for i = 1:NUMPILOTOS
                if matrizFinal(i,1) == pilotos(index,1) % buscamos el piloto q es y cuando le encontramos, toca sumarle el tiempo
                    matrizFinal(i,vueltaActual+1)=tiempoVX;
                    tiemposTotales(i,vueltaActual+1)= tiemposTotales(index,vueltaActual) + tiempoVX;
                end
            end
        end
        
        %Hora de las maniobras
        podemosIrnos=0;
        while ~podemosIrnos
            DRSEnEstaVuelta=zeros(1,NUMPILOTOS);
            podemosIrnos=1;
            if banderaAhora==3 || banderaAhora==1
                for index=2:NUMPILOTOS
                    filaPiloto=pilotoEnPosicion(index,posiciones,NUMPILOTOS);
                    filaDelante=pilotoEnPosicion(index-1,posiciones,NUMPILOTOS);
                    if filaPiloto==-1||filaDelante==-1
                        matrizFinal=zeros(NUMPILOTOS,NUMVUELTAS+1);
                        return;
                    end
                    if tiemposTotales(filaPiloto,vueltaActual+1) < tiemposTotales(filaDelante, vueltaActual+1) % si esto se cumple, el tiempo es menor, deberia estar delante si fueran fantasmas
                        if haEntradoEnEstaVuelta(filaDelante,pitstops,vueltaActual,maxPits) % si es porque ha entrado en boxes, si puede
                            posiciones(filaDelante,2)=posiciones(filaDelante,2)+1;
                            posiciones(filaPiloto,2)=posiciones(filaPiloto,2)-1;
                            podemosIrnos=0;
                        else
                            while tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1) + 0.9)
                                tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+0.05;
                                matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+0.05;
                            end
                        end
                    elseif tiemposTotales(filaPiloto,vueltaActual+1) < tiemposTotales(filaDelante, vueltaActual+1)+0.9 % tiene que haber un tiempo minimo entre coches en la meta
                        while tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1) + 0.9)
                            tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+0.05; % le vamos sumando hasta que lo cumpla
                            matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+0.05;
                        end
                    end
                end
            else % banderaAhora==0 o 2
                for index=2:NUMPILOTOS
                    filaPiloto=pilotoEnPosicion(index,posiciones,NUMPILOTOS);
                    filaDelante=pilotoEnPosicion(index-1,posiciones,NUMPILOTOS);
                    if filaPiloto==-1||filaDelante==-1
                        matrizFinal=zeros(NUMPILOTOS,NUMVUELTAS+1);
                        return;
                    end
                    if tiemposTotales(filaPiloto,vueltaActual+1) < tiemposTotales(filaDelante, vueltaActual+1)
                        if haEntradoEnEstaVuelta(filaDelante,pitstops,vueltaActual,maxPits)
                            posiciones(filaDelante,2)=posiciones(filaDelante,2)+1;
                            posiciones(filaPiloto,2)=posiciones(filaPiloto,2)-1;
                            podemosIrnos=0;
                        else
							if banderaAhora==2
                                while tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1) + 0.9) 
                                    tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+0.05;
                                    matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+0.05;
                                end
							else								
								temp=tiemposTotales(filaPiloto,vueltaActual+1);
								DRS_Active=((hayDRS(vueltaActual, banderas)) && (tiemposTotales(filaPiloto,vueltaActual-1) < tiemposTotales(filaDelante, vueltaActual-1)+1)); % si hay drs en esta vuelta y ademas esta a menos de un segundo
								if DRS_Active
									temp=temp-DRS_TIMEGAIN;
								end
								temp=temp+OVERTAKING_DELTADIFF;
								if temp<tiemposTotales(filaDelante, vueltaActual+1)
									if DRS_Active&&~DRSEnEstaVuelta(index)
										tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)-DRS_TIMEGAIN; % se lo restas de verdad porque ya sí que ha adelantado
										matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)-DRS_TIMEGAIN;
										DRSEnEstaVuelta(index)=1;
									end
									tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+OVERTAKING_TIMELOSS;
									matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+OVERTAKING_TIMELOSS;
									tiemposTotales(filaDelante,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+OVERTAKEN_TIMELOSS;
									matrizFinal(filaDelante,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+OVERTAKEN_TIMELOSS;
									posiciones(filaDelante,2)=posiciones(filaDelante,2)+1;
									posiciones(filaPiloto,2)=posiciones(filaPiloto,2)-1;
									podemosIrnos=0;
								else
									while tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1) + 0.9)
										tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+0.05;
										matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+0.05;
									end
								end
                            end
                        end
                    elseif tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1)+0.9)
                        while tiemposTotales(filaPiloto,vueltaActual+1) < (tiemposTotales(filaDelante, vueltaActual+1) + 0.9)
                            tiemposTotales(filaPiloto,vueltaActual+1)=tiemposTotales(filaPiloto,vueltaActual+1)+0.05;
                            matrizFinal(filaPiloto,vueltaActual+1)=matrizFinal(filaPiloto,vueltaActual+1)+0.05;
                        end
                    end
                end
            end
        end
   
        FUEL_NOW = FUEL_NOW - FUEL_MASSLOSS;
        if FUEL_NOW<=0 % si se le ha acabado la gasofa a la gente abortamos
            matrizFinal=zeros(NUMPILOTOS, NUMVUELTAS+1);
            return;
        end
    end


%% Representasion

% Calculamos el lider virtual
filaPiloto=pilotoEnPosicion(1,posiciones,NUMPILOTOS);
tiempoPrimero=tiemposTotales(filaPiloto,NUMVUELTAS+1);
tiempoMedioVuelta=tiempoPrimero/NUMVUELTAS;
% Calculamos las diferencias 
matrizPlot=tiemposTotales(:,2:NUMVUELTAS+1);
for i=1:NUMVUELTAS
    for j=1:NUMPILOTOS % restamos el tiempo medio por vuelta cada vez mayor a cada piloto
        matrizPlot(j,i)=matrizPlot(j,i)-(i*tiempoMedioVuelta);
    end
end
% Sacamos la gráfica
tsim=1:1:NUMVUELTAS;
hold off;
figure(1);
plot(tsim, matrizPlot(1,1:NUMVUELTAS),'DisplayName',num2str(pilotos(1,1)));
hold on;
for index=2:NUMPILOTOS
    plot(tsim, matrizPlot(index,1:NUMVUELTAS),'DisplayName',num2str(pilotos(index,1)));
end
lgnd=legend;
set(lgnd,'color','none');
set(lgnd,'Location','NorthWest');
ax=gca;
ax.Legend


saveas(gcf,'grafica.png')