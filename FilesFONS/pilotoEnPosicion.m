function res = pilotoEnPosicion(posicion,posiciones,npilotos)
	% recibe la matriz de pilotos y solicita una posicion. Devuelve la fila del piloto encontrado en esa posicion
    res=-1;
    for indice=1:npilotos
        if posiciones(indice,2)==posicion
            res=indice;
            break;
        end
    end
end