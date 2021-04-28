program Main;
	var text : string;
		i, inputLength, floor : integer;
		keepLoopGoing : boolean;
		ex01, ex02 : string;
		
begin { Main }
	ex01 := ')'; {1}
	ex02 := '()())'; {5}

	text := ReadFile('Input/01.txt');
	{text := ex02;}
	inputLength := Length(text);
	keepLoopGoing := true;
	i := 0;
	floor := 0;
	
	while keepLoopGoing do
	begin
		if (i - inputLength = 0) then keepLoopGoing := false
		else
		begin
			if (text[i] = '(') then floor := floor + 1;
			if (text[i] = ')') then floor := floor - 1;

			if (floor = -1) then keepLoopGoing := false;
			
			{Can leave this here, +1 is needed anyway because of 0 indexing.}
			i := i + 1;
		end;
	end;
	
	writeln(i);
end.  { Main }
