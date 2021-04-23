program Main;
	var text : string;
		i, inputLength, l, w, h : integer;
		keepLoopGoing : boolean;
		ex01, ex02 : string;
	
	function GetNumber(text : string; startFrom: integer) : string;
		var i : integer;
			keepLoopGoing : boolean;
	begin
		keepLoopGoing := true;
		i := 0;

		while keepLoopGoing do
		begin
			if (i - inputLength = 0) then keepLoopGoing := false
			else
			begin
				{Break out if we find an X}
				if (text[i] = 'x') then keepLoopGoing := false;

				i := i + 1;
			end;
		end;

		GetNumber := i;
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
		if (i - inputLength = 0) then keepLoopGoing := false
		else
		begin
			l := GetNumber(text, i);

			i := i + Length(l);
			
			i := i + 1;
		end;
	end;
	
	writeln(1337);
end.  { Main }
