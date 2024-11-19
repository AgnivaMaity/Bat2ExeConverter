Discontinued batch 2 exe converter.

This uses the built-in C# compiler to compile the final EXE.
The main process which is involved in converting the batch to exe is by simplifying the batch code to single line cmd code. ( Adding ' && ' in the place of spaces ).
The simplified code is then fed into the C# code template (Compile.cs) which contains fixed arguments like application name etc.
For hidden window style, it simply uses the window visibility mode in application runner.

You are free to use codes from this repo if you like.
