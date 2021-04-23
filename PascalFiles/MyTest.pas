program Main;
	var text : string;
		i, inputLength : integer;
		keepLoopGoing : boolean;
		
begin { Main }
	text := ReadFile('.\AOC\2015\01.txt');
	inputLength := Length(text);
	keepLoopGoing := true;
	i := 0;
	
	while keepLoopGoing do
	begin
		if (i - inputLength = 0) then keepLoopGoing := false
		else
		begin
			writeln(text[i]);
			
			i := i + 1;
		end;
	end;
end.  { Main }
