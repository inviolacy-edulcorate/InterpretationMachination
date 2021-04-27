# Interpretation Machination
![Build & Test Badge](https://github.com/inviolacy-edulcorate/InterpretationMachination/actions/workflows/dotnet.yml/badge.svg?branch=main)

This is the project containing the Interpretation Machination code.
For creating this project, I followed these tutorials.
 - [lsbasi-part1](https://ruslanspivak.com/lsbasi-part1/)
 - [lsbasi-part2](https://ruslanspivak.com/lsbasi-part2/)
 - [lsbasi-part3](https://ruslanspivak.com/lsbasi-part3/)
 - [lsbasi-part4](https://ruslanspivak.com/lsbasi-part4/)
 - [lsbasi-part5](https://ruslanspivak.com/lsbasi-part5/)
 - [lsbasi-part6](https://ruslanspivak.com/lsbasi-part6/)
 - [lsbasi-part7](https://ruslanspivak.com/lsbasi-part7/)
 - [lsbasi-part8](https://ruslanspivak.com/lsbasi-part8/)
 - [lsbasi-part9](https://ruslanspivak.com/lsbasi-part9/)
 - [lsbasi-part10](https://ruslanspivak.com/lsbasi-part10/)
 - [lsbasi-part11](https://ruslanspivak.com/lsbasi-part11/)
 - [lsbasi-part12](https://ruslanspivak.com/lsbasi-part12/)
 - [lsbasi-part13](https://ruslanspivak.com/lsbasi-part13/)
 - [lsbasi-part14](https://ruslanspivak.com/lsbasi-part14/)
 - [lsbasi-part15](https://ruslanspivak.com/lsbasi-part15/)
 - [lsbasi-part16](https://ruslanspivak.com/lsbasi-part16/)
 - [lsbasi-part17](https://ruslanspivak.com/lsbasi-part17/)
 - [lsbasi-part18](https://ruslanspivak.com/lsbasi-part18/)
 - [lsbasi-part19](https://ruslanspivak.com/lsbasi-part19/)

After this, I proceeded to do my own thing (until the next part comes out).

## Todo
 - [x] Change ScopedSymbolTable to just return null instead of error when no entry found. This should be handled by client.
 - [x] Change ScopedSymbolTable to lookup to parent.
 - [x] Expand grammar definition to support procedure parameters.
 - [x] Expand lexer to support procedure parameters.
 - [x] Expand parser to support procedure parameters.
 - [x] AstVisitor: Renamve all functions to "Visit...Node"
 - [ ] Change ProgramNode.Name to string rather than node.
 - [ ] Numbered and documented exceptions.
 - [x] Create Source2Source compiler as in part 14.
 - [ ] VarDeclNode.Name to string only.
 - [x] Extend S2S to subscript built-in types.
 - [ ] Change to use more interfaces.
 - [x] Usable with files.
 - [x] Add built-in print functionality so interpreter can be "standalone", no printing of memory.
 - [ ] Handle types of constants (double/int) for calculations properly.
 - [ ] Add AR/StackFrame Type.
 - [ ] Add debug (breakpoint) functionality.
 - [x] If Support.
 - [x] Else Support.
 - [ ] For Loop support.
 - [ ] Procedure overloading.
 - [ ] Cleanup todos.
 - [ ] Expand print functionality to support strings etc.
 - [x] String support.
 - [x] Add function support.
 - [ ] Allow function call without return type usage.
 - [ ] Add string operations.
   - [x] String Index.
 - [x] Extract all Pascal specifics out of the data structures.
 - [ ] Type handling.
 - [x] Add file reading function (ReadFile()).
 - [x] Add string length function (Length()).
 - [x] While loop support.
 - [x] Extract Lexer into more generic class.
 - [ ] Implement typed operators (+) to support different actions when called with different types.
 - [x] Implement boolean operations other than =

## Notes
There are 2 kinds of expressions/grammar rules/somethings.
1. Those which return something/are to be used (expr and below).
2. Those which are collections/higher structures. Don't contain logic and handle program flow.
Should these have different implementations? Are they different?

## How to run the AOC code
1. Go to https://adventofcode.com/ and sign in
2. Go to any of the input download pages
3. Open the dev-tools of your browser
4. Open the "network" tab and refresh the page
5. Copy the value of the "Cookie" header (without session=, just the hash)
6. Copy cookiesession.example to cookiesession in PascalFiles/AOC
7. Paste the hash value of the cookie in this cookiesession file
8. Run the dl.sh script
9. All the input files are now loaded in the YYYY/Input folder
