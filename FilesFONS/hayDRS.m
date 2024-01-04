function canDRS = hayDRS(vActual,bds)

	%informa de si en la vuelta actual los pilotos pueden utilizar el DRS

    %vuelta 2
    canDRS=0;
    if vActual==2
        return;
    end
    
    %vuelta 3 en adelante
    ahora=bds(vActual);
    hace1=bds(vActual-1);
    hace2=bds(vActual-2);
    
    if (ahora==0||ahora==3)&&(hace1==0||hace1==3)&&(hace2==0||hace2==3)
        canDRS=1;
    end
end