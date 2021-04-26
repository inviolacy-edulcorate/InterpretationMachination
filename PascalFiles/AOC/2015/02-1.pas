program Main;
	var text : string;
		i, inputLength, l, w, h : integer;
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
				if (text[i] = 'x') then keepLoopGoing := false
                {Otherwise, add more to the string.}
                else
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

	{text := ReadFile('Input\02.txt');}

	text := ex01;

	inputLength := Length(text);
	keepLoopGoing := true;
	i := 0;
	
	while keepLoopGoing do
	begin
		if (i > inputLength - 1) then keepLoopGoing := false
		else
		begin
			lstr := GetNumber(text, i);
            
            writeln(lstr);

			i := i + Length(lstr);
            i := i + 1; {Add 1 more to skip the x}
		end;
	end;
	
	writeln(1337);
end.  { Main }
