program Main;
	var text : string;
		i, inputLength, l, w, h, side1, side2, side3, lwhTotal, packagingTotal : integer;
		lstr, wstr, hstr : string;
		keepLoopGoing : boolean;
		ex01, ex02 : string;
	
	function GetNumber(text : string; startFrom: integer) : string;
		var i : integer;
			number : string;
			keepLoopGoing : boolean;
	begin
		keepLoopGoing := true;
		i := startFrom;
		number := '';

		while keepLoopGoing do
		begin
			if (i > inputLength - 1) then keepLoopGoing := false
			else
			begin
				{Break out if we find an X}
				if (text[i] = 'x') then keepLoopGoing := false;
                if (text[i] = '\n') then keepLoopGoing := false;
                
                {Otherwise, add more to the string.}
                if (keepLoopGoing) then 
                begin
                    number := number + text[i];
				
                    i := i + 1;
                end;
			end;
		end;

		GetNumber := number;
	end;

begin { Main }
	ex01 := '2x3x4'; {2*6 + 2*12 + 2*8 = 52 + 6 = 58}
	ex02 := '1x1x10';  {2*1 + 2*10 + 2*10 = 42 + 1 = 43}

	text := ReadFile('Input/02.txt');

	{text := ex02;}

	inputLength := Length(text);
	keepLoopGoing := true;
	i := 0;
	
	while keepLoopGoing do
	begin
		if (i > inputLength - 1) then keepLoopGoing := false
		else
		begin
            {Length}
			lstr := GetNumber(text, i);
            l := strtoint(lstr);
			i := i + Length(lstr) + 1; {Add 1 more to skip the x}
            
            {Width}
			wstr := GetNumber(text, i);
            w := strtoint(wstr);
			i := i + Length(wstr) + 1; {Add 1 more to skip the x}
            
            {Height}
			hstr := GetNumber(text, i);
            h := strtoint(hstr);
			i := i + Length(hstr) + 2; {Add 1 more to skip the newline}
            
            Writeln(' == Entry ==');
            Writeln('Length');
            Writeln(l);
            Writeln('Width');
            Writeln(w);
            Writeln('Height');
            Writeln(h);
            
            side1 := l * w;
            side2 := w * h;
            side3 := h * l;
            
            lwhTotal := 2 * side1 + 2 * side2 + 2 * side3;
            Writeln('Total LWH');
            Writeln(lwhTotal);
            
            {1 < 2 && 1 < 3}
            if (side1 < side2) then
                if (side1 < side3) then
                    packagingTotal := lwhTotal + side1;
            {2 < 1 && 2 < 3}
            if (side2 < side1) then
                if (side2 < side3) then
                    packagingTotal := lwhTotal + side2;
            {3 < 1 && 3 < 2}
            if (side3 < side1) then
                if (side3 < side2) then
                    packagingTotal := lwhTotal + side3;
                    
            Writeln('Total Packaging Required');
            Writeln(packagingTotal);
		end;
	end;
end.  { Main }
