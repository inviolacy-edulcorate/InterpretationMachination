program Main;
	var text : string;
		i, inputLength, l, w, h, side1, side2, side3, ribbonForAll, line, ribbonPerimeter, ribbonBow : integer;
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
                if (text[i] = #10) then keepLoopGoing := false;
                
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
	ex01 := '2x3x4'; {2*2 + 2*3 = 10 + 2*3*4 = 34}
	ex02 := '1x1x10'; {2*1 + 2*1 = 4 + 1*1*10 = 14}

	text := ReadFile('Input/02.txt');

	{text := ex02;}

	inputLength := Length(text);
	keepLoopGoing := true;
	i := 0;
    ribbonForAll := 0;
    line := 0;
    ribbonPerimeter := 0;
	
	while keepLoopGoing do
	begin
		if (i > inputLength - 1) then keepLoopGoing := false
		else
		begin
            line := line + 1;
            {Writeln('line=' + line);}
        
            {Length}
			lstr := GetNumber(text, i);
            l := strtoint(lstr);
			i := i + Length(lstr) + 1; {Add 1 more to skip the x.}
            
            {Width}
			wstr := GetNumber(text, i);
            w := strtoint(wstr);
			i := i + Length(wstr) + 1; {Add 1 more to skip the x.}
            
            {Height}
			hstr := GetNumber(text, i);
            h := strtoint(hstr);
			i := i + Length(hstr) + 1; {Add 1 more to skip the newline.}
            
            {Calculate the sides to compare them.}
            side1 := l * w;
            side2 := w * h;
            side3 := h * l;
            
            {1 <= 2 && 1 <= 3}
            if (side1 < side2 + 1) then
                if (side1 < side3 + 1) then
                    ribbonPerimeter := l + l + w + w;
            {2 <= 1 && 2 <= 3}
            if (side2 < side1 + 1) then
                if (side2 < side3 + 1) then
                    ribbonPerimeter :=  w + w + h + h;
            {3 <= 1 && 3 <= 2}
            if (side3 < side1 + 1) then
                if (side3 < side2 + 1) then
                    ribbonPerimeter :=  h + h + l + l;
            
            ribbonBow := l * w * h;
            
            ribbonForAll := ribbonForAll + ribbonBow + ribbonPerimeter;
            
            Writeln(line + '-' + ribbonForAll + ': ' +l + 'x' + w + 'x' + h + '=>' + ribbonPerimeter + '+' + ribbonBow + '=' + (ribbonPerimeter + ribbonBow));
		end;
	end;
    Writeln(ribbonForAll);
end.  { Main }
