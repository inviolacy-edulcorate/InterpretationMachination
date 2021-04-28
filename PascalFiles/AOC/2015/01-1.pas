program Main;
	var text : string;
		i, inputLength, floor : integer;
		keepLoopGoing : boolean;
		ex01, ex02, ex03, ex04, ex05,ex06, ex07, ex08, ex09 : string;
		
begin { Main }
	ex01 := '(())';
	ex02 := '()()';
	ex03 := '(((';
	ex04 := '(()(()(';
	ex05 := '))(((((';
	ex06 := '())';
	ex07 := '))(';
	ex08 := ')))';
	ex09 := ')())())';

	text := ReadFile('Input/01.txt');
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
			
			i := i + 1;
		end;
	end;
	
	writeln(floor);
end.  { Main }
